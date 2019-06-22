using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
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
            _action = () =>_hub.Clients.All.SendAsync("sendbuslocation", ReturnLocation(locations, GetIndex(locations, ref indx, ref reverse))).Wait();
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
            _timeStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            _action();

            if ((DateTime.Now - TimeStarted).Seconds > 3600)
            {
                _timer.Dispose();
            }
        }

        private Location ReturnLocation(List<Location> allLocations, int indx)
        {
            if (indx >= 0 && indx < allLocations.Count)
            {
                return allLocations[indx];
            }
            else
            {
                return allLocations[0];
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
    }
}
