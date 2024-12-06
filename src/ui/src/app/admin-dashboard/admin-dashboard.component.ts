import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../service/employee.service';
import { RouterLink } from '@angular/router';
import { IDepartment } from '../interface/idepartment';
import { IDesignation } from '../interface/idesignation';
import { IDepartmentForm } from '../interface/idepartment-form';
import { IDesignationForm } from '../interface/idesignation-form';
import { IadminForm } from '../interface/iadmin-form';
import { ImanagerForm } from '../interface/imanager-form';
import { ImanagerData } from '../interface/imanager-data';
import { IaddEmployeeForm } from '../interface/iadd-employee-form';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {
  inDate = new Date();
  userForm!: FormGroup<IadminForm>;
  managerForm!: FormGroup<ImanagerForm>;
  employeeForm!: FormGroup<IaddEmployeeForm>
  isDataLoaded: boolean = false;
  isDataClicked: boolean = false;
  employeeIdClicked: number = 0;
  newEmployeeId: number = 0;
  managerData: ImanagerData[] = [{
    id: 0,
    firstName: '',
    lastName: '',
    role: ''
  }]

  departmentData: IDepartment[] = [{
    id: 0,
    departmentName: ''
  }];
  designationData: IDesignation[] = [{
    id: 0,
    designationName: ''
  }];
  departmentForm!: FormGroup<IDepartmentForm>;
  designationForm!: FormGroup<IDesignationForm>;

  constructor(private employeeService: EmployeeService, private fb: FormBuilder, private toastr: ToastrService) {
    this.buildForm();
  }
  employeeId: number = 0;
  private buildForm() {
    this.departmentForm = this.fb.group({
      departmentName: new FormControl('', Validators.required)
    }),
      this.designationForm = this.fb.group({
        designationName: new FormControl('', Validators.required)
      }), this.userForm = this.fb.group({
        designationId: new FormControl<number | null>(null, Validators.required),
        departmentId: new FormControl<number | null>(null, Validators.required),
        managerId: new FormControl<number | null>(null),
        isActive: new FormControl<boolean | null>(true),
        joiningDate: new FormControl(this.inDate, Validators.required),
        employmentType: new FormControl<number | null>(null, Validators.required),

      }),
      this.managerForm = this.fb.group({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl(''),

      }),
      this.employeeForm = this.fb.group({
        employeeCode: new FormControl<number | null>(null, Validators.required),
        rfId: new FormControl('', Validators.required)
      })
  }

  selectEmployee(employeeId: number): void {
    this.employeeIdClicked = employeeId;
    this.userForm.patchValue({
      managerId: employeeId
    });
    this.isDataClicked = true;
  }

  ngOnInit() {
    this.loadEmployeeData();
  
  }

  loadEmployeeData() {
    this.employeeService.getAllDepartment().subscribe({
      next: (response: any) => {
        if (response) {
          this.departmentData = response
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while Displaying Departments')

      }
    }),
      this.employeeService.getAllDesignation().subscribe({
        next: (response: any) => {
          if (response) {
            this.designationData = response;
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Displaying Designations')
        }
      })
  }

  onSubmitDepartment() {
    if (this.departmentForm.valid) {
      const formData = this.departmentForm.value;
      const params = {
        departmentName: formData.departmentName
      }
      this.employeeService.addDepartment(params).subscribe({
        next: (response: any) => {
          if (response > 0) {
            this.toastr.success('Department added Successfully')
            this.loadEmployeeData();
            this.departmentForm.reset();
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Adding Departments')
        }
      })
    }
  }
  onSubmitDesignation() {
    if (this.designationForm.valid) {
      const formData = this.designationForm.value;
      const params = {
        designationName: formData.designationName
      }
      this.employeeService.addDesignation(params).subscribe({
        next: (response: any) => {
          if (response > 0) {
            this.toastr.success('Designation added Successfully')
            this.loadEmployeeData();
            this.designationForm.reset()
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Adding Designation')

        }
      })
    }

  }
  onSubmitManager() {
    if (this.managerForm.valid) {
      const formData = this.managerForm.value;
      const params = {
        firstName: formData.firstName,
        lastName: formData.lastName
      }
      this.employeeService.getEmployeeByName(params.firstName, params.lastName).subscribe({
        next: (response: any) => {
          this.managerData = response;
          this.isDataLoaded = true;
          this.managerForm.get('firstName')?.setValue('');
          this.managerForm.get('lastName')?.setValue('');
        },
        error: (err) => {
          this.toastr.error('Error fetching managers');
        }
      });
    }
  }
  
  onSubmit() {
    if (this.userForm.valid) {
      const formData = this.userForm.value;
      const params = {
        id: this.newEmployeeId,
        designationId: formData.designationId,
        departmentId: formData.departmentId,
        managerId: formData.managerId,
        isActive: formData.isActive,
        joiningDate: formData.joiningDate,
        employmentType: Number(formData.employmentType),
      }
      this.employeeService.adminUpdateEmployee(params).subscribe({
        next: (response: any) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
            this.restEmployeeId();
            this.userForm.reset();
          }
          else {
            this.toastr.error('Profile Update Failed')
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while  Updating Profile')
        }
      })
    }
  }

  onSubmitAddEmployee() {
    console.log(this.employeeForm)
    if (this.employeeForm.valid) {
      const formData = this.employeeForm.value;
      const params = {
        employeeCode: formData.employeeCode,
        rfId: formData.rfId

      }
      this.employeeService.addEmployee(params).subscribe({
        next: (response: any) => {
          if (response > 0) {
            this.newEmployeeId = response;
            this.toastr.success('Employee Added Successfully')
            this.employeeForm.reset()
          }
          else {
            this.toastr.error('Employee Adding Failed')
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Adding Employee')
        }
      })
    }
  }
  restEmployeeId() {
    this.newEmployeeId = 0;
  }
}