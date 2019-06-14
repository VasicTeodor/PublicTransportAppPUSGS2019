import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { NewPricelist } from 'src/app/_models/newPricelist';
import { AdminService } from 'src/app/_services/admin.service';
import { PricelistItem } from 'src/app/_models/pricelistItem';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { UserDiscount } from 'src/app/_models/userDiscount';

@Component({
  selector: 'app-newPricelist',
  templateUrl: './newPricelist.component.html',
  styleUrls: ['./newPricelist.component.css']
})
export class NewPricelistComponent implements OnInit {
  isCollapsed = true;
  pricelistForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  newPricelist: NewPricelist;
  editPricelist: PricelistItem;
  ticketType = 'Hourly';
  discountType = 'Student';
  userDiscount: UserDiscount = {} as UserDiscount;

  constructor(private authService: AuthService, private fb: FormBuilder, private router: ActivatedRoute, private route: Router,
              private alertify: AlertifyService, private adminService: AdminService) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-orange'
    };

    this.createPricelistForm();

    const id = this.router.snapshot.paramMap.get('pricelistId');

    if (id !== null) {
      this.adminService.getPricelist(+id).subscribe(next => {
        this.editPricelist = next as PricelistItem;
        this.createPricelistUpdateForm();
      }, error => {
        this.route.navigate(['/viewPricelist']);
        this.alertify.error('Error while getting pricelist');
      });
    }
  }

  createPricelistForm() {
    this.pricelistForm = this.fb.group({
      from: ['', Validators.required],
      to: ['', Validators.required],
      price: ['', Validators.required]
    });
  }

  createPricelistUpdateForm() {
    const myMomentFrom: moment.Moment = moment(this.editPricelist.pricelist.from);
    const myMomentTo: moment.Moment = moment(this.editPricelist.pricelist.to);

    this.pricelistForm = this.fb.group({
      from: [myMomentFrom.toDate(), Validators.required],
      to: [myMomentTo.toDate(), Validators.required],
      price: [this.editPricelist.price, Validators.required]
    });
  }

  createPricelist() {
    if (this.editPricelist !== null && this.editPricelist !== undefined) {
      if (this.pricelistForm.valid) {
        this.newPricelist = Object.assign({}, this.pricelistForm.value);
        this.newPricelist.type = this.ticketType;
        this.newPricelist.active = true;
        this.adminService.updatePricelist(this.newPricelist, this.editPricelist.id).subscribe(() => {
          this.alertify.success('Successfully created pricelist');
          this.route.navigate(['/viewPricelist']);
        }, error => {
          this.alertify.error(error);
        });
      }
    } else {
      if (this.pricelistForm.valid) {
        this.newPricelist = Object.assign({}, this.pricelistForm.value);
        this.newPricelist.type = this.ticketType;
        this.newPricelist.active = true;
        this.adminService.createPricelist(this.newPricelist).subscribe(() => {
          this.alertify.success('Successfully created pricelist');
          this.route.navigate(['/viewPricelist']);
        }, error => {
          this.alertify.error(error);
        });
      }
    }
  }

  ticketTypeChanged(type: string) {
    this.ticketType = type;
  }

  discountTypeChanged(type: string) {
    this.discountType = type;
    this.adminService.getUserDiscount(this.discountType).subscribe(next => {
      this.userDiscount = next;
    }, error => {
      this.alertify.error('Failed to get user discount');
    });
  }

  updateDiscount() {
    this.adminService.updateUserDiscount(this.discountType, this.userDiscount).subscribe(next => {
      this.alertify.success('User discount updated');
    }, error => {
      this.alertify.error('User discount failed to updated');
    });
  }
}
