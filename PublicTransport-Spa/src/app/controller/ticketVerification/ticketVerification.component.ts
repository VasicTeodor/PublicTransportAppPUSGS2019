import { Component, OnInit } from '@angular/core';
import { Ticket } from 'src/app/_models/ticket';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ControllerService } from 'src/app/_services/controller.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-ticketVerification',
  templateUrl: './ticketVerification.component.html',
  styleUrls: ['./ticketVerification.component.css']
})
export class TicketVerificationComponent implements OnInit {
  tickets: Ticket[];

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
              private controllerService: ControllerService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.tickets = data.tickets;
    });
  }

}
