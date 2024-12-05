import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Gender } from '../enum/gender-enum';
import { EmploymentType } from '../enum/employment-type-enum';
import { ToastrService } from 'ngx-toastr';
import { Employee } from '../interface/iemployee';
import { IProfileForm } from '../interface/iprofile-from';
import { EmployeeService } from '../service/employee.service';
import { IEmployeeUpdate } from '../interface/iemployee-update';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  userForm!: FormGroup<IProfileForm>;
  employeeUpdate: IEmployeeUpdate = {} as IEmployeeUpdate;
  inDate = new Date();
  employeeData: Employee = {
    id: 0,
    rfId: '',
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateofBirth: this.inDate,
    joiningDate: this.inDate,
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
  constructor(private fb: FormBuilder, private employeeService: EmployeeService, private toastr: ToastrService
  ) {
    this.buildForm();
  }
  private buildForm() {
    this.userForm = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl(''),
      gender: new FormControl<number | null>(null, Validators.required),
      dateofBirth: new FormControl(this.inDate, Validators.required),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.pattern(/^\d{1,10}$/)]),
      email: new FormControl('', Validators.email),
      address: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      state: new FormControl('', Validators.required),
      photo: new FormControl(''),
    })
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
  profileImage() {
    if (this.employeeData.gender == Gender.Male) {
      return "male.png"
    }
    else if (this.employeeData.gender == Gender.Female) {
      return "female.jpg"
    }
    else {
      return "default.png"
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
  loadForm(employeeData: Employee) {
    this.userForm.patchValue({
      firstName: this.employeeData.firstName,
      lastName: this.employeeData.lastName,
      gender: this.employeeData.gender,
      dateofBirth: this.employeeData.dateofBirth,
      phoneNumber: this.employeeData.phoneNumber,
      email: this.employeeData.email,
      address: this.employeeData.address,
      country: this.employeeData.country,
      state: this.employeeData.state,
      photo: this.employeeData.photo,
    });

  }
  loadEmployeeData() {
    const employee = this.employeeService.decodeToken();
    this.employeeService.getEmployeeProfile(employee.EmployeeId).subscribe({
      next: (response: Employee) => {
        if (response) {
          this.employeeData = response;
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while  Displaying Profile')
      }
    })
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

  removePicture() {
    this.userForm.patchValue({
      photo: null,
    });
    this.onSubmit()
  }
  onSubmit() {
    if (this.userForm.valid) {
      const employee = this.employeeService.decodeToken();
      const updateDetails: IEmployeeUpdate = {
        id : employee.EmployeeId,
        firstName: this.userForm.value.firstName ?? '',
        lastName: this.userForm.value.lastName ?? '',
        email: this.userForm.value.email ?? '',
        phoneNumber: this.userForm.value.phoneNumber ?? '',
        gender: Number(this.userForm.value.gender),
        address: this.userForm.value.address ?? '',
        country: this.userForm.value.country ?? '',
        state : this.userForm.value.state ?? '',
        photo: this.userForm.value.photo ?? '',
        dateofBirth: this.userForm.value.dateofBirth ?? this.inDate,
      }
      this.employeeService.employeeUpdateEmployee(updateDetails).subscribe({
        next: (response: boolean) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while  Updating Profile')
        }
      })
    }
  }
}
