import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewStation } from '../_models/newStation';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

createNewStation(station: NewStation) {
  this.http.post(this.baseUrl + 'admin/addStation', station);
}
}
