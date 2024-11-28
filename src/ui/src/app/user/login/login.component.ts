import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../service/wolf-den.service';
import { ILoginForm } from './ilogin-form';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../../service/employee.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder,
    private router: Router,
    private userService: WolfDenService,
    private employeeService: EmployeeService,
    private toastr: ToastrService
  ) {
    this.userForm = this.fb.group<ILoginForm>({
      email: new FormControl(
        '',
        [Validators.required,]
      ),
      password: new FormControl('', Validators.required)
    });
  }

  isSubmitted: boolean = false;

  onSubmit() {
    this.isSubmitted = true;
    if (this.userForm.valid) {
      this.userService.getEmployeeLogin(this.userForm.value.email, this.userForm.value.password).subscribe({
        next: (response: any) => {
          localStorage.setItem('token', response.token);
          const employee = this.employeeService.decodeToken();
          console.log(employee);
          this.userService.userId=employee.EmployeeId;
          this.userService.firstName=employee.FirstName
          console.log(this.userService.userId);
          this.toastr.success('Login sucessfull')
          this.router.navigate(['/portal/dashboard']);
        }
      });
    }
    else {
      this.toastr.error("Invalid Credentials")
    }
  }
  hasDisplayableError(controlName: string): Boolean {
    const control = this.userForm.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
  }
}


