import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { NewUser } from '../models/new_user';
import { ValidateResponse } from '../models/api_response';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = environment.apiUrl + '/register';

  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) {}

  register(newUser: NewUser) {
    return this.http.post<ValidateResponse>(this.apiUrl, newUser);
  }
}
