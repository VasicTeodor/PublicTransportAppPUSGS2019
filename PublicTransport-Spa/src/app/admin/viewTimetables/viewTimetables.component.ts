import { Component, OnInit } from '@angular/core';
import { TimeTable } from 'src/app/_models/timeTable';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-viewTimetables',
  templateUrl: './viewTimetables.component.html',
  styleUrls: ['./viewTimetables.component.css']
})
export class ViewTimetablesComponent implements OnInit {
  isCollapsed = true;
  allTimetables: TimeTable[];
  allLines: Line[];
  departures: string[];
  pagination: Pagination;

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allTimetables = data.timetables.result;
      this.pagination = data.timetables.pagination;
      this.allLines = data.lines;
    });

    this.allTimetables.forEach(tmt => {
      let line = this.allLines.find(l => l.id === tmt.lineId);
      tmt.line = line;
    });

  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadTimetables();
  }

  loadTimetables() {
    this.adminService.getTimetables(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<TimeTable[]>) => {
      this.allTimetables = res.result;
      this.pagination = this.pagination;

      this.allTimetables.forEach(tmt => {
        let line = this.allLines.find(l => l.id === tmt.lineId);
        tmt.line = line;
      });
    }, error => {
      this.alertify.error(error);
    })
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
