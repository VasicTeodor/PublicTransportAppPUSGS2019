import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Pricelist } from '../_models/pricelist';
import { map } from 'rxjs/operators';
import { PricelistItem } from '../_models/pricelistItem';
import { UserRegister } from '../_models/userRegister';
import { User } from '../_models/user';
import { AllPricelists } from '../_models/allPricelists';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getTicketPrices(active: boolean = true): Observable<PricelistItem[]> {
  let params = new HttpParams();
  params = params.append('active', JSON.stringify(active));
  return this.http.get<PricelistItem[]>(this.baseUrl + 'publictransport/pricelists', {params});
}

getAllPricelists(active: boolean = true): Observable<AllPricelists> {
  let params = new HttpParams();
  params = params.append('active', JSON.stringify(active));
  params = params.append('pricelistForAll', JSON.stringify(true));
  return this.http.get<AllPricelists>(this.baseUrl + 'publictransport/pricelists', {params});
}

getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + 'user/' + id);
}

getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl + 'user/');
}

updateAccount(user: UserRegister, id: number) {
  return this.http.put(this.baseUrl + 'user/' + id, user);
}

buyTicketAnonimus(ticketType, email) {
  let params = new HttpParams();
  params = params.append('type', ticketType);
  params = params.append('email', email);
  return this.http.put(this.baseUrl + 'user/buyTicketUnRegistered', params);
}

buyTicketUser(ticketType, userId) {
  let params = new HttpParams();
  params = params.append('type', ticketType);
  params = params.append('userId', userId);
  return this.http.put(this.baseUrl + 'user/buyTicket', params);
}
}
