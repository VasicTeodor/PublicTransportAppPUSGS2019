import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Line } from 'src/app/_models/line';
import { AdminService } from 'src/app/_services/admin.service';
import { Station } from 'src/app/_models/station';
import { StationLine } from 'src/app/_models/stationLine';
import { NewStation } from 'src/app/_models/newStation';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-newStation',
  templateUrl: './newStation.component.html',
  styleUrls: ['./newStation.component.css']
})
export class NewStationComponent implements OnInit {
  isCollapsed = true;
  stationForm: FormGroup;
  station: NewStation;
  editStation: Station;
  newStationLines: Line[];
  allLines: Line[];

  constructor(private authService: AuthService, private fb: FormBuilder, private alertify: AlertifyService,
              private adminService: AdminService, private router: ActivatedRoute) { }

  ngOnInit() {
    this.createStationForm();

    const id = this.router.snapshot.paramMap.get('stationId');

    if (id !== null) {
      this.adminService.getStation(+id).subscribe(next => {
        this.editStation = next as Station;
        this.editStation.stationLines.forEach(element => {
         this.newStationLines.push(element.line);
        });
      });
    }
  }

  createStationForm() {
    this.stationForm = this.fb.group({
      name: ['', Validators.required],
      street: ['', Validators.required],
      number: ['', Validators.required],
      city: ['', Validators.required],
      x: ['', Validators.required],
      y: ['', Validators.required],
    });
  }

  createStationFormForUpdate() {
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
      this.station.lines = this.newStationLines;
      this.adminService.createNewStation(this.station).subscribe(next => {
        this.alertify.success('New station added!');
      }, error => {
        this.alertify.error('Error while adding new station');
      });
    }
  }
}
