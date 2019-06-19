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
  editPricelist: NewPricelist;
  ticketType = 'Hourly';
  discountType = 'Student';
  userDiscount: UserDiscount = {} as UserDiscount;
  idForUpdate: number;

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
        this.editPricelist = next as NewPricelist;
        this.idForUpdate = +id;
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
      priceHourly: ['', Validators.required],
      priceDaily: ['', Validators.required],
      priceMonthly: ['', Validators.required],
      priceAnnual: ['', Validators.required]
    });
  }

  createPricelistUpdateForm() {
    const myMomentFrom: moment.Moment = moment(this.editPricelist.from);
    const myMomentTo: moment.Moment = moment(this.editPricelist.to);

    this.pricelistForm = this.fb.group({
      from: [myMomentFrom.toDate(), Validators.required],
      to: [myMomentTo.toDate(), Validators.required],
      priceHourly: [this.editPricelist.priceHourly, Validators.required],
      priceDaily: [this.editPricelist.priceDaily, Validators.required],
      priceMonthly: [this.editPricelist.priceMonthly, Validators.required],
      priceAnnual: [this.editPricelist.priceAnnual, Validators.required]
    });
  }

  createPricelist() {
    if (this.editPricelist !== null && this.editPricelist !== undefined) {
      if (this.pricelistForm.valid) {
        this.newPricelist = Object.assign({}, this.pricelistForm.value);
        this.newPricelist.type = this.ticketType;
        this.newPricelist.idHourly = this.editPricelist.idHourly;
        this.newPricelist.idDaily = this.editPricelist.idDaily;
        this.newPricelist.idMonthly = this.editPricelist.idMonthly;
        this.newPricelist.idAnnual = this.editPricelist.idAnnual;
        this.newPricelist.active = true;
        this.adminService.updatePricelist(this.newPricelist, this.idForUpdate).subscribe(() => {
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
