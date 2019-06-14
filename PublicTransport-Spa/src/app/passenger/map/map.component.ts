import { Component, OnInit, NgZone } from '@angular/core';
import { MarkerInfo } from 'src/app/_models/marker-info.model';
import { GeoLocation } from 'src/app/_models/geolocation';
import { Polyline } from 'src/app/_models/polyline';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { Station } from 'src/app/_models/station';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css'],
  styles: ['agm-map {height: 100%; width: 100%;}']
})
export class MapComponent implements OnInit {
  markerInfo: MarkerInfo;
  public polyline: Polyline;
  public zoom: number;
  allDir: any[] = new Array<any>();
  dir = undefined;
  allLines: Line[];
  allStations: Station[];

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allLines = data.lines;
      this.allStations = data.stations;
    });

    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954), '',
      'Jugodrvo' , '' , 'http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka');

    // this.polyline = new Polyline([], 'blue', { url: '', scaledSize: {width: 50, height: 50}});
    this.getDirection();
  }

  constructor(private ngZone: NgZone, private alertify: AlertifyService, private route: ActivatedRoute) {
  }

  public getDirection() {
    this.dir = {
      origin: { lat: 45.254203, lng: 19.852039 },
      destination: { lat: 45.261705, lng: 19.837223 }
    };
  }

  initializeRoutes() {

  }

  placeMarker($event) {
    this.polyline.addLocation(new GeoLocation($event.coords.lat, $event.coords.lng));
    console.log(this.polyline);
  }

}
