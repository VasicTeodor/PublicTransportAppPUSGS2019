import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Ticket } from '../_models/ticket';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/Pagination';
import { map } from 'rxjs/operators';

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

  verificateTicket(ticketId: number) {
    let params = new HttpParams();
    params = params.append('ticketId', JSON.stringify(ticketId));
    return this.http.put(this.baseUrl + 'moderator/validateTicket', params);
  }

  getTickets(page?, itemsPerPage?): Observable<PaginatedResult<Ticket[]>> {
    const paginatedResult: PaginatedResult<Ticket[]> = new PaginatedResult<Ticket[]>();

    let params = new HttpParams()

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<Ticket[]>(this.baseUrl + 'moderator', { observe: 'response', params}).pipe(map(response => {
      paginatedResult.result = response.body;
      if (response.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResult;
    }));
  }

}
