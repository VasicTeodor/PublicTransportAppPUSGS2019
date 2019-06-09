import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsModalService, BsModalRef  } from 'ngx-bootstrap/modal';
import { LoginComponent } from '../user/login/login.component';
import { RegisterComponent } from '../user/register/register.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  modalRef: BsModalRef;

  constructor(private router: Router, private authService: AuthService, private alertify: AlertifyService,
              private modalService: BsModalService) { }

  ngOnInit() {
  }

  loggedIn() {
    /* return this.authService.loggedIn(); */
    return true;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

  openModalLogin() {
    this.modalRef = this.modalService.show(LoginComponent);
  }

  openModalRegister() {
    this.modalRef = this.modalService.show(RegisterComponent);
  }
}
