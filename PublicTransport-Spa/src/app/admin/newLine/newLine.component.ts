import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NewLine } from 'src/app/_models/newLine';
import { Line } from 'src/app/_models/line';
import { Station } from 'src/app/_models/station';
import { AdminService } from 'src/app/_services/admin.service';
import { ActivatedRoute } from '@angular/router';
import { Bus } from 'src/app/_models/bus';

@Component({
  selector: 'app-newLine',
  templateUrl: './newLine.component.html',
  styleUrls: ['./newLine.component.css']
})
export class NewLineComponent implements OnInit {
  isCollapsed = true;
  lineForm: FormGroup;
  line: NewLine;
  editLine: Line;
  newLineStations: Station[];
  allStations: Station[];
  newLineBuses: Bus[];
  allBuses: Bus[];
  selectedStation: number;
  selectedBus: number;
  selectedStationToAdd: number;
  selectedBusToAdd: number;

  constructor(private authService: AuthService, private fb: FormBuilder, private alertify: AlertifyService,
              private adminService: AdminService, private router: ActivatedRoute) { }

  ngOnInit() {
    this.createLineForm();

    const id = this.router.snapshot.paramMap.get('lineId');

    if (id !== null) {
      this.adminService.getLine(+id).subscribe(next => {
        this.editLine = next as Line;
        this.editLine.stations.forEach(element => {
         this.newLineStations.push(element.station);
        });
        this.editLine.buses.forEach(element => {
          this.newLineBuses.push(element);
          });
        this.createLineFormForUpdate();
      }, error => {
        this.alertify.error('Error while getting line');
      });
    }
  }

  createLineForm() {
    this.lineForm = this.fb.group({
      name: ['', Validators.required],
      number: ['', Validators.required]
    });
  }

  createLineFormForUpdate() {
    this.lineForm = this.fb.group({
      name: [this.editLine.name, Validators.required],
      number: [this.editLine.lineNumber, Validators.required]
    });
  }

  createLine() {
    if (this.lineForm.valid) {
      this.line = Object.assign({}, this.lineForm.value);
      this.line.stations = this.newLineStations;
      this.line.buses = this.newLineBuses;
      this.adminService.createNewLine(this.line).subscribe(next => {
        this.alertify.success('New line added!');
      }, error => {
        this.alertify.error('Error while adding new line');
      });
    }
  }

  stationChanged(id: number) {
    this.selectedStation = id;
  }

  removeStation() {
    const index = this.newLineStations.indexOf(this.newLineStations.find(station => station.id === this.selectedStation));
    this.newLineStations.splice(index, 1);
  }

  stationChangedAdd(id: number) {
    this.selectedStationToAdd = id;
  }

  addStation() {
    const index = this.allStations.indexOf(this.allStations.find(station => station.id === this.selectedStationToAdd));
    this.newLineStations.push(this.allStations[index]);
  }

  busChanged(id: number) {
    this.selectedBus = id;
  }

  removeBus() {
    const index = this.newLineBuses.indexOf(this.newLineBuses.find(bus => bus.id === this.selectedBus));
    this.newLineBuses.splice(index, 1);
  }

  busChangedAdd(id: number) {
    this.selectedBusToAdd = id;
  }

  addBus() {
    const index = this.allBuses.indexOf(this.allBuses.find(bus => bus.id === this.selectedBusToAdd));
    this.newLineBuses.push(this.allBuses[index]);
  }


}
