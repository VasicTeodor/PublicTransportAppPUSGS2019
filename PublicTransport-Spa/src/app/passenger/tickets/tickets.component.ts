import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PricelistItem } from 'src/app/_models/pricelistItem';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { PaypalHelper } from 'src/app/_models/paypalHelper';
import { IPayPalConfig, ICreateOrderRequest } from 'ngx-paypal';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  pricelists: PricelistItem[];
  email = '';
  ticketType = '';
  public payPalConfig ? : IPayPalConfig;
  public defaultPrice: string = '9.99';

  public showSuccess: boolean = false;
  public showCancel: boolean = false;
  public showError: boolean = false;
  
  constructor(private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit() {
    const success = this.route.snapshot.paramMap.get('success');
    console.log(success);
    if (success != null) {
      if (+success === 1) {
        this.alertify.success('Ticket bought successfuly via PayPal.')
      } else if (+success === 0) {
        this.alertify.error('You cancelled ticket.')
      }
    }

    this.route.data.subscribe(data => {
      this.pricelists = data.pricelists;
    });
    this.initConfig('9.99');
 }

  getTickets() {
    this.userService.getTicketPrices().subscribe((res: any[]) => {
      this.alertify.success('Got tickets');
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

  buyTicketPayPal(ticketType) {
    // tslint:disable-next-line:max-line-length
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (this.loggedIn()) {
      this.userService.buyTicketPayPal(ticketType, this.authService.decodedToken.nameid, null).subscribe(next => {
        console.log(next);
        const address = next as PaypalHelper;
        window.location.href = address.address;
        this.alertify.message('Redirecting to PayPal');
      }, error => {
        this.alertify.error('Error while buying ticket');
      });
    } else {
      if (this.email !== null && re.test(this.email)) {
        this.userService.buyTicketPayPal(ticketType, -1, this.email).subscribe(next => {
          console.log(next);
          const address = next as PaypalHelper;
          window.location.href = address.address;
          this.alertify.message('Redirecting to PayPal');
        }, error => {
          this.alertify.error('Error while buying ticket');
        });
      } else {
        this.alertify.error('Please enter email!');
      }
    }
  }

  private initConfig(price: string): void {
    this.payPalConfig = {
      currency: 'EUR',
      clientId: environment.clientId,
      createOrderOnClient: (data) => <ICreateOrderRequest>{
        intent: 'CAPTURE',
        purchase_units: [
          {
            amount: {
              currency_code: 'EUR',
              value: price,
              breakdown: {
                item_total: {
                  currency_code: 'EUR',
                  value: price
                }
              }
            },
            items: [
              {
                name: 'Enterprise Subscription',
                quantity: '1',
                category: 'DIGITAL_GOODS',
                unit_amount: {
                  currency_code: 'EUR',
                  value: price,
                },
              }
            ]
          }
        ]
      },
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical'
      },
      onApprove: (data, actions) => {
        console.log('onApprove - transaction was approved, but not authorized', data, actions);
        actions.order.get().then((details: any) => {
          console.log('onApprove - you can get full order details inside onApprove: ', details);
        });
  
      },
      onClientAuthorization: (data) => {
        console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
        this.showSuccess = true;
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
        this.showCancel = true;
  
      },
      onError: err => {
        console.log('OnError', err);
        this.showError = true;
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
        this.resetStatus();
      },
      onInit: (data, actions) => {
        console.log('onInit', data, actions);
      }
    };
  }
  
    private resetStatus(): void {
      this.showError = false;
      this.showSuccess = false;
      this.showCancel = false;
    }
}
