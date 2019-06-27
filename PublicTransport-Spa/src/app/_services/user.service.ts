import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Pricelist } from '../_models/pricelist';
import { map } from 'rxjs/operators';
import { PricelistItem } from '../_models/pricelistItem';
import { UserRegister } from '../_models/userRegister';
import { User } from '../_models/user';
import { AllPricelists } from '../_models/allPricelists';
import { PaginatedResult } from '../_models/Pagination';
import { TimeTable } from '../_models/timeTable';
import { Line } from '../_models/line';

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

getUsers(page?, itemsPerPage?): Observable<PaginatedResult<User[]>> {
  const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams()

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http.get<User[]>(this.baseUrl + 'user/', {observe: 'response', params}).pipe(map(response => {
    paginatedResult.result = response.body;
    if (response.headers.get('Pagination') != null) {
      paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
    }
    return paginatedResult;
  }));;
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

buyTicketPayPal(ticketType, userId, email) {
  const httpOptions = {
    headers: new HttpHeaders({ 
      'Access-Control-Allow-Origin':'*'
    })
  };
  return this.http.get(this.baseUrl + 'paypal/create?ticketType=' + ticketType + "&userId=" + userId + "&email=" + email, httpOptions);
}

getTimetables(type?: string, day?: string) {
  let params = new HttpParams();
  params = params.append('type', type);
  params = params.append('day', day);
  return this.http.get<TimeTable[]>(this.baseUrl + 'publictransport/timetables', {params});
}

getLines(type?: string, day?: string) {
  let params = new HttpParams();
  params = params.append('type', type);
  params = params.append('day', day);
  return this.http.get<Line[]>(this.baseUrl + 'publictransport/lines', {params});
}
}
