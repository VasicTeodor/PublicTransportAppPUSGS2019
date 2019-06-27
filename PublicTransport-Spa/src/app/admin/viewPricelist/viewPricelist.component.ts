import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { PricelistItem } from 'src/app/_models/pricelistItem';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-viewPricelist',
  templateUrl: './viewPricelist.component.html',
  styleUrls: ['./viewPricelist.component.css']
})
export class ViewPricelistComponent implements OnInit {
  isCollapsedPrices = true;
  allPricelists: PricelistItem[];
  pagination: Pagination

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allPricelists = data.pricelists.result;
      this.pagination = data.pricelists.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadPricelists();
  }

  loadPricelists() {
    this.adminService.getPricelists(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<PricelistItem[]>) => {
      this.allPricelists = res.result;
      this.pagination = this.pagination;
    }, error => {
      this.alertify.error(error);
    })
  }

  removePricelist(pricelistId: number) {
    this.adminService.deletePricelist(pricelistId).subscribe(next => {
      this.alertify.success('Pricelist deleted');
      const indx = this.allPricelists.indexOf(this.allPricelists.find(pr => pr.id === pricelistId));
      this.allPricelists.splice(indx, 1);
    }, error => {
      this.alertify.error('Failed to delete pricelist');
    });
  }
}
