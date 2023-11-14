import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private apiUrl = environment.apiUrl + '/login';

  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) {}
  login(credentials: User) {
    return this.http.post<any>(this.apiUrl, credentials);
  }
}
