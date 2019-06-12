import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Line } from 'src/app/_models/line';
import { AdminService } from 'src/app/_services/admin.service';
import { Station } from 'src/app/_models/station';
import { StationLine } from 'src/app/_models/stationLine';
import { NewStation } from 'src/app/_models/newStation';

@Component({
  selector: 'app-newStation',
  templateUrl: './newStation.component.html',
  styleUrls: ['./newStation.component.css']
})
export class NewStationComponent implements OnInit {
  isCollapsed = true;
  stationForm: FormGroup;
  station: NewStation;
  newStationLines: Line[];
  allLines: Line[];

  constructor(private authService: AuthService, private fb: FormBuilder, private alertify: AlertifyService,
              private adminService: AdminService) { }

  ngOnInit() {
    this.createRegiserForm();
  }

  createRegiserForm() {
    this.stationForm = this.fb.group({
      name: ['', Validators.required],
      street: ['', Validators.required],
      number: ['', Validators.required],
      city: ['', Validators.required],
      x: ['', Validators.required],
      y: ['', Validators.required],
    });
  }

  createStation() {
    if (this.stationForm.valid) {
      this.station = Object.assign({}, this.stationForm.value);
    }
  }
}
