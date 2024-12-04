import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ISignupForm } from './iSignup-form';
import { Router, RouterLink } from '@angular/router';
import { MatNativeDateModule } from '@angular/material/core'; // For native date adapter
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select'; // Import MatSelectModule
import { formatDate } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { WolfDenService } from '../../service/wolf-den.service'



@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatNativeDateModule, MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule, MatSelectModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss'
})
export class SigninComponent {
  userForm: FormGroup;

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password')
    const confirmPassword = control.get('confirmPassword')

    if (password && confirmPassword && password.value != confirmPassword.value)
      confirmPassword?.setErrors({ passwordMismatch: true })
    else
      confirmPassword?.setErrors(null)

    return null;
  }

  onImageUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64Image = e.target.result;
        this.userForm.value.photo = base64Image;
      };
      reader.readAsDataURL(file);
    }
  }

  constructor(private fb: FormBuilder,
    private userService: WolfDenService,
    private router: Router,
    private toastr: ToastrService
  ) {

    const isdate = new Date();

    this.userForm = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      dateofBirth: new FormControl(isdate, Validators.required),
      gender: new FormControl<number | null>(null, Validators.required),
      phoneNumber: new FormControl<string | null>(null, Validators.required),
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl<string | null>(null, Validators.required),
      address: new FormControl<string | null>(null),
      country: new FormControl<string | null>(null),
      state: new FormControl<string | null>(null),
      photo: new FormControl<string | null>(null),

    }, { validators: this.passwordMatchValidator })
  }



  isFormDetails: boolean = false;
  isSubmitted: boolean = false;


  onSubmit() {
    this.isSubmitted = true;
    console.log('form', this.userForm.value);

    const dateOfBirth = this.userForm.value.dateofBirth;
    const formattedDate = dateOfBirth ? formatDate(dateOfBirth, 'yyyy-MM-dd', 'en-US') : '';
    if (this.userForm.valid) {
      const userData = {

        id: this.userService.userId ?? '',
        firstName: this.userForm.value.firstName ?? '',
        lastName: this.userForm.value.lastName ?? '',
        email: this.userForm.value.email ?? '',
        phoneNumber: this.userForm.value.phoneNumber ?? '',
        dateofBirth: formattedDate,
        gender: this.userForm.value.gender ?? '',
        address: this.userForm.value.address ?? '',
        country: this.userForm.value.country ?? '',
        state: this.userForm.value.state ?? '',
        photo: this.userForm.value.photo ?? '',


      }

      this.userService.signIn(userData).subscribe({
        next: (response) => {
          console.log(response)
          if (response) {
            this.userService.resetPassword(this.userService.userId, this.userForm.value.password).subscribe({
              next: (response: boolean) => {
                if (response) {
                  this.toastr.success('Registration Sucessfull')
                  this.router.navigate(['/portal/dashboard']);
                }
                else {
                  this.toastr.error('Invalid Pasword')
                }
              },
              error: (error) => {
                this.toastr.error('Password Error')
              }
            });
          }
          else {
            this.toastr.error("form invalid");
          }


        },
        error: (error: any) => {
          this.toastr.error('User was not created', 'Registration Unsuccessful')

        }
      });

    } else {
      this.toastr.error('Enter valid details', 'Registration Unsuccessful')
    }
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.userForm.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
  }

}