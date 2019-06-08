import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PricelistItem } from 'src/app/_models/pricelistItem';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  pricelists: PricelistItem[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.pricelists = data.pricelists;
    });
}

  getTickets() {
    this.userService.getPricelists().subscribe((res: any[]) => {
      this.alertify.success('Logged in succesfully');
      // this.router.navigate(['/members']);
      console.log(res);
    }, error => {
      this.alertify.error(error);
    }, () => {
      console.log('Ovo odradi i posel errora i  poslse uspeha');
    });
    console.log();
  }
}
