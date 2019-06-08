import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Pricelist } from '../_models/pricelist';
import { map } from 'rxjs/operators';
import { PricelistItem } from '../_models/pricelistItem';
import { UserRegister } from '../_models/userRegister';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getPricelists(active: boolean = true): Observable<PricelistItem[]> {
  let params = new HttpParams();
  params = params.append('active', JSON.stringify(active));
  return this.http.get<PricelistItem[]>(this.baseUrl + 'publictransport/pricelists', {params});
}

getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + 'user/' + id);
}

getUsers(): Observable<UserRegister[]> {
  return this.http.get<UserRegister[]>(this.baseUrl + 'user/');
}

updateAccount(user: UserRegister, id: number) {
  return this.http.put(this.baseUrl + 'user/' + id, user);
}
}
