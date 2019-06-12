import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-newTimetable',
  templateUrl: './newTimetable.component.html',
  styleUrls: ['./newTimetable.component.css']
})
export class NewTimetableComponent implements OnInit {
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
      y: ['', Validators.required]
    });
  }

}
