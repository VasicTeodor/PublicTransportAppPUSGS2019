import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewStation } from '../_models/newStation';
import { Observable } from 'rxjs';
import { Station } from '../_models/station';
import { NewLine } from '../_models/newLine';
import { Line } from '../_models/line';

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

updateStation(stationId: number, station: NewStation) {
  return this.http.put(this.baseUrl + 'admin/updateStation?stationId=' + stationId, station);
}

createNewLine(line: NewLine) {
  return this.http.post(this.baseUrl + 'admin/addLine', line);
}

getLines() {
  return this.http.get<Observable<Line[]>>(this.baseUrl + 'admin/getLines');
}

getLine(lineId: number) {
  return this.http.get<Line>(this.baseUrl + 'admin/getLine?lineId=' + lineId);
}

deleteLine(lineId) {
  return this.http.delete(this.baseUrl + 'admin/removeLine?lineId=' + lineId);
}

updateLine(line: NewLine, lineId: number) {
  return this.http.put(this.baseUrl + 'admin/updateLine?lineId=' + lineId, line);
}
}
