import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NewLine } from 'src/app/_models/newLine';
import { Line } from 'src/app/_models/line';
import { Station } from 'src/app/_models/station';
import { AdminService } from 'src/app/_services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
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
  editingLine = false;
  newLineStations: Station[] = new Array<Station>();
  allStations: Station[];
  newLineBuses: Bus[] = new Array<Bus>();
  allBuses: Bus[];
  selectedStation: number;
  selectedBus: number;
  selectedStationToAdd: number;
  selectedBusToAdd: number;
  model: Bus = {} as Bus;

  constructor(private authService: AuthService, private fb: FormBuilder, private alertify: AlertifyService,
              private adminService: AdminService, private router: ActivatedRoute, private route: Router) { }

  ngOnInit() {
    this.createLineForm();

    this.router.data.subscribe(data => {
      this.allBuses = data.busses;
      this.allStations = data.stations;
    });

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
        this.editingLine = true;
      }, error => {
        this.route.navigate(['/viewLines']);
        this.alertify.error('Error while getting line');
      });
    } else {
      this.editingLine = false;
    }
  }

  createLineForm() {
    this.lineForm = this.fb.group({
      name: ['', Validators.required],
      lineNumber: ['', Validators.required]
    });
  }

  createLineFormForUpdate() {
    this.lineForm = this.fb.group({
      name: [this.editLine.name, Validators.required],
      lineNumber: [this.editLine.lineNumber, Validators.required]
    });
  }

  createLine() {
    if (this.editLine !== null && this.editLine !== undefined) {
      if (this.lineForm.valid) {
        this.line = Object.assign({}, this.lineForm.value);
        this.line.stations = this.newLineStations;
        this.line.buses = this.newLineBuses;
        this.adminService.updateLine(this.line, this.editLine.id).subscribe(next => {
          this.alertify.success('Line updated!');
          this.route.navigate(['/viewLines']);
        }, error => {
          this.alertify.error('Error while updating line');
        });
      }
    } else {
      if (this.lineForm.valid) {
        this.line = Object.assign({}, this.lineForm.value);
        this.line.stations = this.newLineStations;
        this.line.buses = this.newLineBuses;
        this.adminService.createNewLine(this.line).subscribe(next => {
          this.alertify.success('New line added!');
          this.route.navigate(['/viewLines']);
        }, error => {
          this.alertify.error('Error while adding new line');
        });
      }
    }
  }

  stationChanged(id: number) {
    this.selectedStation = id;
  }

  removeStation() {
    const index = this.newLineStations.indexOf(this.newLineStations.find(station => +station.id === +this.selectedStation));
    this.newLineStations.splice(index, 1);
  }

  stationChangedAdd(id: number) {
    this.selectedStationToAdd = id;
    console.log(id);
  }

  addStation() {
    const index = this.allStations.indexOf(this.allStations.find(station => +station.id === +this.selectedStationToAdd));
    this.newLineStations.push(this.allStations[index]);
    console.log(this.allStations.indexOf(this.allStations.find(station => +station.id === +this.selectedStationToAdd)));
    console.log(this.allStations[index]);
  }

  busChanged(id: number) {
    this.selectedBus = id;
  }

  removeBus() {
    const index = this.newLineBuses.indexOf(this.newLineBuses.find(bus => +bus.id === +this.selectedBus));
    this.newLineBuses.splice(index, 1);
  }

  busChangedAdd(id: number) {
    this.selectedBusToAdd = id;
  }

  addBus() {
    const index = this.allBuses.indexOf(this.allBuses.find(bus => +bus.id === +this.selectedBusToAdd));
    this.newLineBuses.push(this.allBuses[index]);
  }

  addNewBus() {
    this.adminService.createNewBus(this.model).subscribe(next => {
      this.alertify.success('Bus added');
      this.adminService.getBusses().subscribe( next => {
        this.allBuses = (next as unknown) as Bus[];
      }, error => {
        this.alertify.error('Error while getting busses');
      });
    }, error => {
      this.alertify.error('Error while adding bus');
    });
  }
}
