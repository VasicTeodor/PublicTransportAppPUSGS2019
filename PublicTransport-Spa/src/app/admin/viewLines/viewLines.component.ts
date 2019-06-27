import { Component, OnInit } from '@angular/core';
import { Line } from 'src/app/_models/line';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-viewLines',
  templateUrl: './viewLines.component.html',
  styleUrls: ['./viewLines.component.css']
})
export class ViewLinesComponent implements OnInit {
  isCollapsedStations = true;
  isCollapsedBuses = true;
  allLines: Line[];
  pagination: Pagination;

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allLines = data.lines.result;
      this.pagination = data.lines.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadLines();
  }

  loadLines() {
    this.adminService.getLines(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<Line[]>) => {
      this.allLines = res.result;
      this.pagination = this.pagination;
    }, error => {
      this.alertify.error(error);
    })
  }

  deleteLine(lineId: number) {
    this.adminService.deleteLine(lineId).subscribe(next => {
      this.alertify.success('Line deleted');
      const indx = this.allLines.indexOf(this.allLines.find(line => line.id === lineId));
      this.allLines.splice(indx, 1);
    }, error => {
      this.alertify.error('Failed to delete line');
    });
  }

}
