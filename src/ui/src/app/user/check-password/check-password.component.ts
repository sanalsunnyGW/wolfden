import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../service/wolf-den.service';
import { EmployeeService } from '../../service/employee.service';
import { ToastrService } from 'ngx-toastr';
import { IresetPassword } from '../../interface/ireset-password';
@Component({
  selector: 'app-check-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './check-password.component.html',
  styleUrl: './check-password.component.scss'
})
export class CheckPasswordComponent {
  userForm: FormGroup;

  passwordMatchValidator: ValidatorFn = (control: AbstractControl) : null => {
    const password = control.get('password')
    const confirmPassword = control.get('confirmPassword')

    if(password && confirmPassword && password.value != confirmPassword.value)
      confirmPassword?.setErrors({ passwordMismatch: true })
    else
    confirmPassword?.setErrors(null)
  
    return null;
  }

  constructor(private fb: FormBuilder,
    private router: Router,
    private userService: WolfDenService,
    private toastr: ToastrService
  ) {
    this.userForm = this.fb.group<IresetPassword>({
    
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', Validators.required),
    },{validators:this.passwordMatchValidator});
  }

  isSubmitted: boolean = false;

  onSubmit() {
    this.isSubmitted = true;
    if (this.userForm.valid) {
      this.userService.resetPassword(this.userService.userId, this.userForm.value.password).subscribe({
        next: (response: boolean) => {
          if (response===true) {
            this.toastr.success('password Changed')
            this.router.navigate(['/portal/dashboard']);        
            }
            else{              
              this.toastr.error('Invalid')
           }
        },
        error: (error) => {
          this.toastr.error('Invalid password')        
        }
        })
        }            
  }
  hasDisplayableError(controlName: string): Boolean {
    const control = this.userForm.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
  }
}
