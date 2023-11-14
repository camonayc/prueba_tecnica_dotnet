import { Component } from '@angular/core';
import { People } from '../../models/people';
import { User } from '../../models/user';
import { RegisterService } from '../../services/register.service';
import { NewUser } from '../../models/new_user';
import { MessageService } from 'primeng/api';
import { ValidateResponse } from '../../models/api_response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  user: User = {
    userName: '',
    password: '',
  };

  personalData: People = {
    name: '',
    lastName: '',
    numIdentification: '',
    email: '',
    typeIdentification: '',
  };

  constructor(
    private registerService: RegisterService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  register() {
    const userRegister: NewUser = {
      user: this.user,
      person: this.personalData,
    };
    this.registerService.register(userRegister).subscribe(
      (response: ValidateResponse) => {
        console.log(response.response);
        if (!response.response) {
          console.error('Error al registrarte, intenta mas tarde:');
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail:
              'Hubo un problema al registrar, verifica los datos e intenta más tarde.',
          });
          return;
        }
        console.log('Registro exitoso:', response);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Registro exitoso!',
        });
        this.router.navigate(['/home']);
        return;
      },
      (error: any) => {
        console.error('Error al registrarte, intenta mas tarde:', error);
        console.log(`Error ${error}`);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            'Hubo un problema al registrar, verifica los datos e intenta más tarde.',
        });
        return;
      }
    );
  }
}
