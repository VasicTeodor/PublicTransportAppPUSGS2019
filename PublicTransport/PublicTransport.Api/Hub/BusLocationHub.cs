using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Hub
{
    public class BusLocationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        private int indx = -1;
        private bool reverse = false;
        private IHubContext<BusLocationHub> _hub;
        private DateTime _timeStarted;

    public DateTime TimeStarted
        {
            get => _timeStarted;
            set
            {
                if (_timeStarted != value)
                {
                    _timeStarted = value;
                }
            }
        }

        public BusLocationHub(IHubContext<BusLocationHub> hub)
        {
            _hub = hub;
        }

        public void InitializeHub(List<Location> locations, int lineId)
        {
            _action = () =>_hub.Clients.All.SendAsync("sendbuslocation", ReturnLocation(locations, lineId, GetIndex(locations, ref indx, ref reverse))).Wait();
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
            _timeStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            _action();

            if ((DateTime.Now - TimeStarted).Minutes > 5)
            {
                _timer.Dispose();
            }
        }

        public void StopTimer()
        {
            try
            {
                _timer.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private BusLocationDto ReturnLocation(List<Location> allLocations, int lineId, int indx)
        {
            if (indx >= 0 && indx < allLocations.Count)
            {
                var busLocation = new BusLocationDto()
                {
                    X = allLocations[indx].X,
                    Y = allLocations[indx].Y,
                    LineId = lineId
                };

                return busLocation;
            }
            else
            {
                var busLocation = new BusLocationDto()
                {
                    X = allLocations[0].X,
                    Y = allLocations[0].Y,
                    LineId = lineId
                };

                return busLocation;
            }
        }

        private int GetIndex(List<Location> allLocations, ref int indx, ref bool reverse)
        {
            if ((indx >= 0 && indx + 1 < allLocations.Count && !reverse) || indx == -1)
            {
                reverse = false;
                indx += 1;
                return indx;
            }
            else if (indx + 1 == allLocations.Count)
            {
                reverse = true;
                indx -= 1;
                return indx;
            }
            else if (reverse)
            {
                indx -= 1;

                if (indx == 0)
                    reverse = false;

                return indx;
            }
            else
            {
                return 0;
            }
        }

        ~BusLocationHub()
        {
            try
            {
                _timer.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}
