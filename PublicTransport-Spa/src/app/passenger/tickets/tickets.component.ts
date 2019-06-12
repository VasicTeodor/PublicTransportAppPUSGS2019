import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PricelistItem } from 'src/app/_models/pricelistItem';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  pricelists: PricelistItem[];
  email = '';

  constructor(private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.pricelists = data.pricelists;
    });
 }

  getTickets() {
    this.userService.getTicketPrices().subscribe((res: any[]) => {
      this.alertify.success('Logged in succesfully');
      // this.router.navigate(['/members']);
      console.log(res);
    }, error => {
      this.alertify.error(error);
    }, () => {
      console.log('Ovo odradi i posel errora i  poslse uspeha');
    });
  }

  loggedIn() {
    return this.authService.isPassenger() && this.authService.loggedIn();
  }

  buyTicket(ticketType) {
    // tslint:disable-next-line:max-line-length
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (this.loggedIn()) {
      this.userService.buyTicketUser(ticketType, this.authService.decodedToken.nameid).subscribe(next => {
        this.alertify.success('Ticket bought');
      }, error => {
        this.alertify.error('Error while buying ticket');
      });
    } else {
      if (this.email !== null && re.test(this.email)) {
        this.userService.buyTicketAnonimus(ticketType, this.email).subscribe(next => {
          this.alertify.success('Ticket bought');
        }, error => {
          this.alertify.error('Error while buying ticket');
        });
      } else {
        this.alertify.error('Please enter email!');
      }
    }
  }
}
