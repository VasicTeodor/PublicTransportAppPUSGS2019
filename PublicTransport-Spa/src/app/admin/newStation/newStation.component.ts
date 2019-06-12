import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-newStation',
  templateUrl: './newStation.component.html',
  styleUrls: ['./newStation.component.css']
})
export class NewStationComponent implements OnInit {
  isCollapsed = true;
  registerForm: FormGroup;

  constructor(private authService: AuthService, private fb: FormBuilder, private alertify: AlertifyService) { }

  ngOnInit() {
    this.createRegiserForm();
  }

  createRegiserForm() {
    this.registerForm = this.fb.group({
      userType: ['regular'],
      userName: ['', Validators.required],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      street: ['', Validators.required],
      number: ['', Validators.required],
      city: ['', Validators.required],
      x: ['', Validators.required],
      y: ['', Validators.required],
    });
  }

}
