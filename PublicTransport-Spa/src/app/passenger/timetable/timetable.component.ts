import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { TimeTable } from 'src/app/_models/timeTable';
import { Departures } from 'src/app/_models/departures';
import { Directions } from 'src/app/_models/directions';
import { Station } from 'src/app/_models/station';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.css'],
  styles: ['agm-map {height: 100%; width: 100%;}']
})
export class TimetableComponent implements OnInit {
  allTimetables: TimeTable[];
  allLines: Line[];
  day: string;
  type: string;
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


  constructor(private alertify: AlertifyService, private router: ActivatedRoute, private route: Router) { }

  ngOnInit() {
    this.router.data.subscribe(data => {
      this.allTimetables = data.timetables;
      this.allLines = data.lines;
    });

    this.day = 'Working day';
    this.type = 'In City';
  }

  dayChanged(day: string) {
    this.day = day;
  }

  typeChanged(type: string) {
    this.type = type;
  }

  lineChanged(id: number) {
    this.selectedLine = id;
    console.log(this.selectedLine);
    this.allStations = null;
    this.allStations = new Array<Station>();
    this.allDir.forEach(element => {
      element.show = false;
    });
    this.allDir = null;
    this.allDir = new Array<Directions>();
    this.showTimetable();
    this.initializeRoutes();
  }

  showTimetable() {
    console.log('mrs');
    this.timetable.day = this.day;
    this.timetable.type = this.type;
    let index = this.allLines.indexOf(this.allLines.find(line => +line.id === +this.selectedLine));
    this.line = this.allLines[index] as Line;
    console.log(this.line.name);
    console.log(this.line.id);
    this.timetable.line = this.line;
    this.timetable.departures = "";

    this.allTimetables.forEach(tmtable => {
      if (tmtable.day === this.day && tmtable.type === this.type && tmtable.lineId === this.line.id)
      {
        this.timetable.id = tmtable.id;
        this.timetable.type = tmtable.type;
        this.timetable.day = tmtable.day;
        this.timetable.line = this.line;
        this.timetable.departures = tmtable.departures;
        console.log(this.timetable.id);
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
    console.table(this.allLines);
    if (this.line.stations !== null || this.line.stations !== undefined || this.line.stations.length > 0) {
      let dir = new Directions();
      dir.waypoints = new Array<any>();
      if (this.line.stations[0] !== undefined) {
        console.log(this.line.stations[0]);
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

    console.log(this.allDir);
  }
}
