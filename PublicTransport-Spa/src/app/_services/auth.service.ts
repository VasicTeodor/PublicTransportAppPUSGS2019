import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl = environment.apiUrl + 'authorization/';

constructor(private http: HttpClient) { }

register(user: any) {
  return this.http.post(this.baseUrl + 'register', user);
}

}
