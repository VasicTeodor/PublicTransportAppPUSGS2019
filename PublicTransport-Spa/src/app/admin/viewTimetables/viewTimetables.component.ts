import { Component, OnInit } from '@angular/core';
import { TimeTable } from 'src/app/_models/timeTable';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-viewTimetables',
  templateUrl: './viewTimetables.component.html',
  styleUrls: ['./viewTimetables.component.css']
})
export class ViewTimetablesComponent implements OnInit {
  isCollapsed = true;
  allTimetables: TimeTable[];
  departures: string[];

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allTimetables = data.timetables;
    });
  }

  deleteTimetable(timetableId: number) {
    this.adminService.deleteTimetable(timetableId).subscribe(next => {
      this.alertify.success('Timetable deleted');
      const indx = this.allTimetables.indexOf(this.allTimetables.find(timetable => timetable.id === timetableId));
      this.allTimetables.splice(indx, 1);
    }, error => {
      this.alertify.error('Failed to delete timetable');
    });
  }

}
