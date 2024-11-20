import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ISignupForm } from './iSignup-form';
import { WolfDenService } from '../../wolf-den.service';
import { Router, RouterLink } from '@angular/router';
import { MatNativeDateModule } from '@angular/material/core'; // For native date adapter
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select'; // Import MatSelectModule
import { formatDate } from '@angular/common';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatNativeDateModule,MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule, MatSelectModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss'
})
export class SigninComponent {
  userForm : FormGroup<ISignupForm>;
  
  constructor(private fb: FormBuilder, 
    private userService: WolfDenService, 
    private router: Router,
    private toastr: ToastrService
  ) {

    const isdate=new Date();
    this.userForm = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName:new FormControl('',Validators.required),
      email: new FormControl('', [
        Validators.required,Validators.email]),
        dateofBirth:new FormControl(isdate,Validators.required),
        gender: new FormControl<number | null>(null, Validators.required),
        phoneNumber:new FormControl<string|null>(null,Validators.required)
    })
  }


  isSubmitted: boolean = false;


  onSubmit() {
    this.isSubmitted = true;
    console.log('form',this.userForm.value);
  
    const dateOfBirth = this.userForm.value.dateofBirth;
    const formattedDate = dateOfBirth ? formatDate(dateOfBirth, 'yyyy-MM-dd', 'en-US') : '';
    if (this.userForm.valid) {
       const userData = {
        id: this.userService.userId ?? '',
        firstName: this.userForm.value.firstName ?? '',
        lastName:this.userForm.value.lastName ??'',
        email : this.userForm.value.email ?? '',
        phoneNumber:this.userForm.value.phoneNumber??'',
        //dateofBirth:this.userForm.value.dateofBirth ??'',
        dateofBirth: formattedDate, 
        joiningDate: formatDate,
        gender:this.userForm.value.gender?? ''

      }

      // "id": 0,
      // "firstName": "string",
      // "lastName": "string",
      // "email": "string",
      // "phoneNumber": "string",
      // "dateofBirth": "2024-11-19",
      // "joiningDate": "2024-11-19",
      // "gender": 1


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

