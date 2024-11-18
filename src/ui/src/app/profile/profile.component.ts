import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { IProfileForm } from './iprofile-from';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink,RouterLinkActive,ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
userForm:FormGroup<IProfileForm>;
inDate=new Date();

  constructor(private fb:FormBuilder) {
    this.userForm=this.fb.group({
      firstName:new FormControl('',Validators.required),
      lastName:new FormControl(),
      gender:new FormControl('',Validators.required),
      birthday:new FormControl(this.inDate,Validators.required),
      joiningDate:new FormControl(this.inDate,Validators.required),
      phoneNumber:new FormControl('',[Validators.required, Validators.minLength(10), Validators.pattern(/^\d{1,9}$/)]),
      email:new FormControl('',Validators.email),
      address:new FormControl('',Validators.required),
      country:new FormControl('',Validators.required),
      state:new FormControl('',Validators.required)
    })
    
  }
  onSubmit(){
    if (this.userForm.valid) {
      console.log('Form Values:', this.userForm.value);
    }

  }

}
