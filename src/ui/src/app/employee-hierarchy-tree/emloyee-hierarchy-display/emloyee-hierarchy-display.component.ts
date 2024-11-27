import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IProfileForm } from '../../Interface/iprofile-from';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '../../Interface/iemployee';
import { EmployeeService } from '../../Service/employee.service';
import { Gender } from '../../enum/gender-enum';
import { EmploymentType } from '../../enum/employment-type-enum';
import { IadminForm } from '../../Interface/iadmin-form';

@Component({
  selector: 'app-emloyee-hierarchy-display',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './emloyee-hierarchy-display.component.html',
  styleUrl: './emloyee-hierarchy-display.component.scss'
})
export class EmloyeeHierarchyDisplayComponent {
  inDate = new Date();
  userForm!: FormGroup<IadminForm>
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



  constructor(private route: ActivatedRoute, private employeeService: EmployeeService, private fb: FormBuilder) {
    this.buildForm();
  }
  employeeId: number = 0;
  private buildForm() {
    this.userForm = this.fb.group({
      designationId: new FormControl<number | null>(null, Validators.required),
      departmanetId: new FormControl<number | null>(null, Validators.required),
      managerId: new FormControl<number | null>(null, Validators.required),
      isActive: new FormControl<boolean | null>(null),
      joiningDate: new FormControl(this.inDate, Validators.required),
      employmentType: new FormControl<number | null>(null, Validators.required),

    })
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.employeeId = params['id'];
      if (this.employeeId) {
        this.loadEmployeeData();
      }
    });
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
  loadEmployeeData() {
    this.employeeService.getEmployeeProfile(this.employeeId).subscribe({
      next: (response: any) => {
        if (response) {
          this.employeeData = response;
        }
      },
      error: (error) => {
        alert("An error occurred while  Displaying Profile");
      }
    })
  }
  loadForm(employeeData: Employee) {
    this.userForm.patchValue({
      designationId:this.employeeData.designationId,
      departmanetId: this.employeeData.departmentId,
      managerId: this.employeeData.managerId,
      isActive: this.employeeData.isActive,
      joiningDate: this.employeeData.joiningDate,
      employmentType: this.employeeData.employmentType,
      
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      const formData = this.userForm.value;
      const params = {
      id: this.employeeId,
      designationId:formData.designationId,
      departmanetId: formData.departmanetId,
      managerId: formData.managerId,
      isActive: formData.isActive,
      joiningDate: formData.joiningDate,
      employmentType: formData.employmentType,
      }
      this.employeeService.employeeUpdateEmployee(params).subscribe({
        next: (response: any) => {
          if (response == true) {
            alert("Profile Updated Successfully")
            this.loadEmployeeData();
          }
          else {
            alert("Profile Update Failed")
          }
        }
      })
    } else {
    }

  }

}




