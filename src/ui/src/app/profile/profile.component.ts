import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { IProfileForm } from '../Interface/iprofile-from';
import { EmployeeServiceService } from '../services/employee-service.service';
import { Employee } from '../../models/iemployee';
import { Gender } from '../enum/gender-enum';
import { EmploymentType } from '../enum/employment-type-enum';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  userForm: FormGroup<IProfileForm>;
  inDate = new Date();
  employeeData: Employee;
  service = inject(EmployeeServiceService)

  constructor(private fb: FormBuilder) {
    this.userForm = this.fb.group({
      id: new FormControl<number | null>(null, Validators.required), 
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl(''),
      gender: new FormControl<number | null>(null, Validators.required), 
      dateofBirth: new FormControl(this.inDate, Validators.required),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.pattern(/^\d{1,10}$/)]),
      email: new FormControl('', Validators.email),
      address: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      state: new FormControl('', Validators.required),
      photo:new FormControl('')
    }),
      this.employeeData = {
        id: 0,
        rfId: '',
        employeeCode: 0,
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        dateofBirth: this.inDate,
        joiningDate:this.inDate,
        gender: 0,
        designationId: 0,
        designationName: '',
        departmentId: 0,
        departmentName: '',
        managerId: null,
        managerName: null,
        isActive: false,
        address: '',
        country: '',
        state: '',
        employmentType: 0,
        photo: ''
      };
  }
  getGenderDisplay(gender: number): string {
    switch (gender) {
      case Gender.Male:
        return "Male";
      case Gender.Female:
        return "Female";
      case Gender.Other:
        return "Other";
      default:
        return "Unknown"
    }
  }

  getEmploymentType(employmentType: number): string {
    switch (employmentType) {
      case EmploymentType.Contract:
        return "Contract";
      case EmploymentType.Permanent:
        return "Permanent";
      default:
        return "Unknown"
    }
  }
  ngOnInit() {
    this.loadEmployeeData();
  }
  loadEmployeeData() {
    this.service.getEmployeeProfile().subscribe({
      next: (response: any) => {
        if (response) {
          this.employeeData = response;
          this.userForm.patchValue({
            id:1,                                        //local storage id
            firstName: this.employeeData.firstName,
            lastName: this.employeeData.lastName,
            gender: this.employeeData.gender,
            dateofBirth: this.employeeData.dateofBirth,
            phoneNumber:this.employeeData.phoneNumber,
            email: this.employeeData.email,
            address: this.employeeData.address,
            country: this.employeeData.country,
            state: this.employeeData.state,
            photo: this.employeeData.photo 
          });
        }
      },
      error: (error) => {
        alert("An error occurred while  Displaying Profile");
      }
    })
  }
  onImageUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64Image = e.target.result;
        this.userForm.value.photo= base64Image; 
      };
      reader.readAsDataURL(file);  
    }
  }


  onSubmit() {
    if (this.userForm.valid) {
      console.log('Form Values:', this.userForm.value);
      const formData = this.userForm.value;
      formData.gender=Number(formData.gender);
      this.service.employeeUpdateEmployee(formData).subscribe({
        next:(response:any)=>
          {
            if(response==true){
              alert("Profile Updated Successfully")
            }
            else{
              alert("Profile Update Failed")
            }
          }
      })
    } else {
      console.log('Form is invalid');
    }

  }

}
