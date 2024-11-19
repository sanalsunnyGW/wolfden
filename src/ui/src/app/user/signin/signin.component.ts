import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { first } from 'rxjs';
import { ISignupForm } from './iSignup-form';
import { WolfDenService } from '../../wolf-den.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss'
})
export class SigninComponent {
  userForm : FormGroup<ISignupForm>;
  toastr: any;

  constructor(private fb: FormBuilder, 
    private userService: WolfDenService, 
    private router: Router,
  ) {

    this.userForm = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName:new FormControl('',Validators.required),
      email: new FormControl('', [
        Validators.required,Validators.email]),
      
    })
  }


  isSubmitted: boolean = false;


  onSubmit() {
    this.isSubmitted = true;
    if (this.userForm.valid) {
       const userData = {
        firstName: this.userForm.value.firstName ?? '',
        lastName:this.userForm.value.lastName??'',
        email : this.userForm.value.email ?? '',
      }


      // Call addUser method from the UserService
      this.userService.signIn(userData).subscribe({
        next: (response: any) => {
          // console.log('User registered successfully:', response);
          // alert("User registered successfully");
          this.toastr.success('New user created!','Registration Successful')


          this.router.navigate(['user/login']);
        },
        error: (error: any) => {
          // console.error('Error during registration:', error);
          this.toastr.error('User was not created','Registration Unsuccessful')

        }
      });
    } else {
      // console.log('Form is invalid');
      this.toastr.error('Enter valid details','Registration Unsuccessful')
    }
  }

  hasDisplayableError(controlName: string ):Boolean {
      const control = this.userForm.get(controlName);
      return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))

  }
}

