import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../wolf-den.service'; 
import { Observable } from 'rxjs';
import { ILoginForm } from './ilogin-form';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private userService: WolfDenService) {
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
    //api logic
    this.router.navigate(['/dashboard']);
    }
    hasDisplayableError(controlName: string ):Boolean {
      const control = this.userForm.get(controlName);
      return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
  }
  }


