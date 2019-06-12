import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewStation } from '../_models/newStation';
import { Observable } from 'rxjs';
import { Station } from '../_models/station';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

createNewStation(station: NewStation) {
  return this.http.post(this.baseUrl + 'admin/addStation', station);
}

getStations() {
  return this.http.get<Observable<Station[]>>(this.baseUrl + 'admin/GetStations');
}

getStation(stationId: number) {
  return this.http.get<Station>(this.baseUrl + 'admin/getStation?stationId=' + stationId);
}

deleteStation(stationId) {
  return this.http.delete(this.baseUrl + 'admin/removeStation?stationId=' + stationId);
}
}
