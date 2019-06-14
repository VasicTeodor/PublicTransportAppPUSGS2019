import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { TimeTable } from 'src/app/_models/timeTable';
import { Departures } from 'src/app/_models/departures';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.css']
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
    this.showTimetable();
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

}
