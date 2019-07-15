import { Component, OnInit, OnDestroy } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { TimeTable } from 'src/app/_models/timeTable';
import { Departures } from 'src/app/_models/departures';
import { Directions } from 'src/app/_models/directions';
import { Station } from 'src/app/_models/station';
import { SignalRService } from 'src/app/_services/signal-r.service';
import { HttpClient } from '@angular/common/http';
import { BusLocation } from 'src/app/_models/busLocation';
import { UserService } from 'src/app/_services/user.service';
import { Observable } from 'rxjs';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.css'],
  styles: ['agm-map {height: 100%; width: 100%;}']
})
export class TimetableComponent implements OnInit, OnDestroy {
  allTimetables: TimeTable[];
  allLines: Line[] = new Array<Line>();
  day: string = 'Working day';
  type: string = 'In City';
  selectedLine: number;
  line: Line;
  timetable= {} as TimeTable;
  isInitialized: boolean = false;
  departures: Departures[] = new Array(24);
  allDir: Directions[] = new Array<Directions>();
  dir = undefined;
  allStations: Station[];
  options = {
      suppressMarkers: true,
  };
  busLocation: BusLocation;

  icon = {
    url: '../../../assets/busicon.png',
    scaledSize: {
      width: 75,
      height: 75
    }
  };

  constructor(private alertify: AlertifyService, private router: ActivatedRoute, private route: Router, private adminService: AdminService,
              public signalRService: SignalRService, private http: HttpClient, private userService: UserService) { }

  ngOnInit() {
    this.router.data.subscribe(data => {
      this.allTimetables = data.timetables;
      this.allTimetables.forEach(timetable => {
        this.allLines.push(data.lines.find(l => l.id === timetable.lineId));
      });
      this.signalRService.startConnection();
      const busLocationObservable = this.signalRService.addTransferBusLocationListener();
      busLocationObservable.subscribe((locationData: BusLocation) => {
          if (locationData.lineId == this.selectedLine) {
            this.busLocation = locationData;
          }
        });
  });

    this.day = this.getDate();
    this.type = 'In City';
  }

  private getDate(): string {
    let result = '';
    let d = new Date();
    let day = d.getDay();

    switch (day) {
        case 6:
            result = 'Saturday';
            break;
        case 0:
            result = 'Sunday';
            break;
        default:
            result = 'Working day'
            break;
    }

    return result;
}

  ngOnDestroy() {
    this.signalRService.stopConnection();
  }

  private startHttpRequest = (lineId: number) => {
    this.http.get('http://localhost:5000/api/busLocation?lineId=' + lineId)
      .subscribe(res => {
        console.log(res);
      })
  }

  private getTimetables() {
    this.userService.getTimetables(this.type, this.day).subscribe(next => {
      this.allTimetables = next;
      this.userService.getLines().subscribe(next => {
        console.log(next);
        this.allLines.splice(0, this.allLines.length);
        this.allTimetables.forEach(timetable => {
        const lines = next;
        this.allLines.push(lines.find(l => l.id === timetable.lineId));
      });
      }, error => {
        this.alertify.error(error);
      })
    }, error => {
      this.alertify.error(error);
    })
  }

  dayChanged(day: string) {
    this.day = day;
    this.getTimetables();
  }

  typeChanged(type: string) {
    this.type = type;
    this.getTimetables();
  }

  lineChanged(id: number) {
    console.log("Selektovana linija je " + id);
    this.selectedLine = id;
    this.allStations = null;
    this.allStations = new Array<Station>();
    this.hideDirections();
    this.showTimetable();
    this.initializeRoutes();
    this.startHttpRequest(id);
  }

  hideDirections() {
    console.log(this.allDir);
    for (let index = 0; index < this.allDir.length; index++) {
      this.allDir[index].show = false;
    }
  }

  showTimetable() {
    this.timetable.day = this.day;
    this.timetable.type = this.type;
    let index = this.allLines.indexOf(this.allLines.find(line => +line.id === +this.selectedLine));
    this.line = this.allLines[index] as Line;
    this.timetable.line = this.line;
    this.timetable.departures = "";

    this.departures.forEach(departure => {
      departure.description = '';
    });

    this.allTimetables.forEach(tmtable => {
      if (tmtable.day === this.day && tmtable.type === this.type && tmtable.lineId === this.line.id)
      {
        this.timetable.id = tmtable.id;
        this.timetable.type = tmtable.type;
        this.timetable.day = tmtable.day;
        this.timetable.line = this.line;
        this.timetable.departures = tmtable.departures;
      }
    });

    if (!this.isInitialized)
    {
      for(let i=0; i<this.departures.length; i++)
      {
        this.departures[i] = new Departures();
      }
      this.isInitialized = true;
    }

    let departuresToEdit = this.timetable.departures.split('/');
    for(let i=0; i<departuresToEdit.length; i++)
    {
      if (departuresToEdit[i] !== undefined)
      {
        let depToEdit = departuresToEdit[i].split(':'); //departures[].name = depToEdit[0]
        if (depToEdit[1] !== undefined)
        {
          /* let desc = depToEdit[1].replace('-', '  ') */
          this.departures[i].name = depToEdit[0];
          this.departures[i].description = depToEdit[1];
        }
      }
    }
  }

  initializeRoutes() {
    if (this.line.stations !== null || this.line.stations !== undefined || this.line.stations.length > 0) {
      let dir = new Directions();
      dir.waypoints = new Array<any>();
      if (this.line.stations[0] !== undefined) {
        dir.origin = {lat: this.line.stations[0].station.location.x, lng: this.line.stations[0].station.location.y};
        dir.destination = {lat: this.line.stations[this.line.stations.length - 1].station.location.x,
            lng: this.line.stations[this.line.stations.length - 1].station.location.y};
        this.line.stations.forEach(stl => {
          dir.waypoints.push({ location: { lat:  stl.station.location.x, lng: stl.station.location.y }, stopover: true });
          this.allStations.push(stl.station);
        });
        this.allDir.push(dir);
      }
    }
  }
}
