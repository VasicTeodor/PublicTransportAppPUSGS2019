import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Pricelist } from '../_models/pricelist';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getPricelists(): Observable<Pricelist[]> {
  return this.http.get<Pricelist[]>(this.baseUrl + 'user/pricelists');
}
}
