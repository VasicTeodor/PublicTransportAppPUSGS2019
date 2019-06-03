import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in succesfully');
      // this.router.navigate(['/members']);
    }, error => {
      this.alertify.error(error);
    }, () => {
      console.log('Ovo odradi i posel errora i  poslse uspeha');
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }
}
