import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { Observable } from 'rxjs';
import { WolfDenService } from '../../Service/wolf-den.service';
import { ILoginForm } from './ilogin-form';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, 
              private router: Router, 
              private userService: WolfDenService,
              private toastr :ToastrService
            ) {
    this.userForm = this.fb.group<ILoginForm>({
      email: new FormControl(
        '',
        [Validators.required, ]
      ),
      password: new FormControl('', Validators.required)
    });
  }

  isSubmitted :boolean= false;

  onSubmit() {
    this.isSubmitted = true;
    if(this.userForm.valid){

      this.userService.getEmployeeLogin(this.userForm.value.Email,this.userForm.value.Password).subscribe({
        next:(response:any)=>{
            this.userService.userId=response.id;
            this.toastr.success('Login sucessfull')
            this.router.navigate(['/dashboard']);
        }
      });
    }
    else{
      this.toastr.error("Invalid Credentials")
    }
    }
    hasDisplayableError(controlName: string ):Boolean {
      const control = this.userForm.get(controlName);
      return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    }
  }


