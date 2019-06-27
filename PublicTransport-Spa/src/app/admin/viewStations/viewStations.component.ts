import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Station } from 'src/app/_models/station';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-viewStations',
  templateUrl: './viewStations.component.html',
  styleUrls: ['./viewStations.component.css']
})
export class ViewStationsComponent implements OnInit {
  isCollapsed = true;
  allStations: Station[];
  pagination: Pagination;

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allStations = data.stations.result;
      this.pagination = data.stations.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadStations();
  }

  loadStations() {
    this.adminService.getStations(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<Station[]>) => {
      this.allStations = res.result;
      this.pagination = this.pagination;
    }, error => {
      this.alertify.error(error);
    })
  }

  deleteStation(stationId: number) {
    this.adminService.deleteStation(stationId).subscribe(next => {
      this.alertify.success('Station deleted');
      const indx = this.allStations.indexOf(this.allStations.find(station => station.id === stationId));
      this.allStations.splice(indx, 1);
    }, error => {
      this.alertify.error('Failed to delete station');
    });
  }

}
