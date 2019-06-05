import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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

getPricelists(active: boolean = true): Observable<any[]> {
  let params = new HttpParams();
  params = params.append('active', JSON.stringify(active));
  return this.http.get<any[]>(this.baseUrl + 'user/pricelists', {params});
}
}
