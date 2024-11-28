import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { IProfileForm } from '../interface/iprofile-from';
import { Employee } from '../interface/iemployee';
import { Gender } from '../enum/gender-enum';
import { EmploymentType } from '../enum/employment-type-enum';
import { EmployeeService } from '../Service/employee.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  userForm!: FormGroup<IProfileForm>;

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
      password: new FormControl('')
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
      password: null
    });

  }
  loadEmployeeData() {
    const employee = this.employeeService.decodeToken();
    this.employeeService.getEmployeeProfile(employee.EmployeeId).subscribe({
      next: (response: any) => {
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

  onSubmit() {
    if (this.userForm.valid) {
      const formData = this.userForm.value;
      const employee = this.employeeService.decodeToken();
      const params = {
        id: employee.EmployeeId,
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        dateofBirth: formData.dateofBirth,
        gender: Number(formData.gender),
        address: formData.address,
        country: formData.country,
        state: formData.state,
        photo: formData.photo,
        password: formData.password
      }
      this.employeeService.employeeUpdateEmployee(params).subscribe({
        next: (response: any) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')

            this.loadEmployeeData();
          }
          else {
            this.toastr.error('Profile Update Failed')

          }
        }
      })
    } else {
    }

  }

}
