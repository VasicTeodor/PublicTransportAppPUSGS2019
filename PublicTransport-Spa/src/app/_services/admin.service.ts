import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewStation } from '../_models/newStation';
import { Observable } from 'rxjs';
import { Station } from '../_models/station';
import { NewLine } from '../_models/newLine';
import { Line } from '../_models/line';
import { Bus } from '../_models/bus';
import { Pricelist } from '../_models/pricelist';
import { NewPricelist } from '../_models/newPricelist';
import { PricelistItem } from '../_models/pricelistItem';
import { TimeTable } from '../_models/timeTable';
import { UserDiscount } from '../_models/userDiscount';

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

getBusses() {
  return this.http.get<Observable<Bus[]>>(this.baseUrl + 'admin/getBusses');
}

createPricelist(pricelist: NewPricelist) {
  return this.http.post(this.baseUrl + 'admin/addPricelist', pricelist);
}

getPricelists() {
  return this.http.get<Observable<PricelistItem[]>>(this.baseUrl + 'admin/getAllPricelists');
}

getPricelist(pricelistId: number) {
  return this.http.get<NewPricelist>(this.baseUrl + 'admin/getPricelist?pricelistId=' + pricelistId);
}

deletePricelist(pricelistId) {
  return this.http.delete(this.baseUrl + 'admin/removePricelist?pricelistId=' + pricelistId);
}

updatePricelist(pricelist: NewPricelist, pricelistId: number) {
  return this.http.put(this.baseUrl + 'admin/updatePricelist?pricelistId=' + pricelistId, pricelist);
}

createNewTimetable(timetable: TimeTable) {
  return this.http.post(this.baseUrl + 'admin/addTimetable', timetable);
}

getTimetables() {
  return this.http.get<Observable<TimeTable[]>>(this.baseUrl + 'admin/getTimetables');
}

getTimetable(timetableId: number) {
  return this.http.get<TimeTable>(this.baseUrl + 'admin/getTimetable?timetableId=' + timetableId);
}

deleteTimetable(timetableId) {
  return this.http.delete(this.baseUrl + 'admin/removeTimetable?timetableId=' + timetableId);
}

updateTimetable(timetableId: number, timetable: TimeTable) {
  return this.http.put(this.baseUrl + 'admin/updateTimetable?timetableId=' + timetableId, timetable);
}

createNewBus(bus: any) {
  return this.http.post(this.baseUrl + 'admin/addBus', bus);
}

getUserDiscount(type: string) {
  return this.http.get<UserDiscount>(this.baseUrl + 'admin/getUserDiscount?type=' + type);
}

updateUserDiscount(type: string, discount: UserDiscount) {
  return this.http.put(this.baseUrl + 'admin/updateUserDiscount?type=' + type, discount);
}
}
