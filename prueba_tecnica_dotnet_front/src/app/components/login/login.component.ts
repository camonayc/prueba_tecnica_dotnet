import { Component } from '@angular/core';
import { User } from '../../models/user';
import { LoginService } from '../../services/login.service';
import { MessageService } from 'primeng/api';
import { ValidateResponse } from '../../models/api_response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  user: User = {
    userName: '',
    password: '',
  };

  constructor(
    private loginService: LoginService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  login() {
    this.loginService.login(this.user).subscribe(
      (response: ValidateResponse) => {
        if (response.response) {
          console.log('Login exitoso:', response);
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Inicio de sesion exitoso.',
          });
          this.router.navigate(['/dashboard'])
        } else {
          console.log('Login exitoso:', response);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail:
              'No se pudo iniciar sesion, verifica los datos e intenta más tarde.',
          });
        }
      },
      (error: any) => {
        console.error('Error de inicio de sesión:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            'No se pudo iniciar sesion, verifica los datos e intenta más tarde.',
        });
      }
    );
  }
}
