import { Component, OnInit, NgZone } from '@angular/core';
import { MarkerInfo } from 'src/app/_models/marker-info.model';
import { GeoLocation } from 'src/app/_models/geolocation';
import { Polyline } from 'src/app/_models/polyline';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Line } from 'src/app/_models/line';
import { Station } from 'src/app/_models/station';
import { Directions } from 'src/app/_models/directions';

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
  allDir: Directions[] = new Array<Directions>();
  dir = undefined;
  allLines: Line[];
  allStations: Station[];

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allLines = data.lines;
      this.allStations = data.stations;
    });

    this.initializeRoutes();
  }

  constructor(private ngZone: NgZone, private alertify: AlertifyService, private route: ActivatedRoute) {
  }

  initializeRoutes() {
    console.table(this.allLines);
    this.allLines.forEach(element => {
      if (element.stations !== null || element.stations !== undefined || element.stations.length > 0) {
        let dir = new Directions();
        dir.waypoints = new Array<any>();
        if (element.stations[0] !== undefined) {
          console.log(element.stations[0]);
          dir.origin = {lat: element.stations[0].station.location.x, lng: element.stations[0].station.location.y};
          dir.destination = {lat: element.stations[element.stations.length - 1].station.location.x,
             lng: element.stations[element.stations.length - 1].station.location.y};
          element.stations.forEach(stl => {
            dir.waypoints.push({ location: { lat:  stl.station.location.x, lng: stl.station.location.y }, stopover: true });
          });
          this.allDir.push(dir);
        }
      }
    });

    console.log(this.allDir);
  }

}
