import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ControllerService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  verificateUser(userId: number, valid: boolean) {
    let params = new HttpParams();
    params = params.append('userId', JSON.stringify(userId));
    params = params.append('valid', JSON.stringify(valid));
    return this.http.put(this.baseUrl + 'moderator', params);
  }

}
