import { Component, Inject, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../service/employee.service';
import { RouterLink } from '@angular/router';
import { IDepartment } from '../interface/idepartment';
import { IDesignation } from '../interface/idesignation';
import { IDepartmentData, IDepartmentForm } from '../interface/idepartment-form';
import { IDesignationData, IDesignationForm } from '../interface/idesignation-form';
import { IadminForm } from '../interface/iadmin-form';
import { ImanagerForm } from '../interface/imanager-form';
import { ImanagerData } from '../interface/imanager-data';
import { IaddEmployeeForm } from '../interface/iadd-employee-form';
import { IaddEmployee } from '../interface/iadd-employee';
import { IadminUpdate } from '../interface/iadmin-update';
import { WolfDenService } from '../service/wolf-den.service';

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
  params: IaddEmployee = {} as IaddEmployee;
  departmentForm!: FormGroup<IDepartmentForm>;
  designationForm!: FormGroup<IDesignationForm>;
  wolfdenService = inject(WolfDenService)
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
    this.wolfdenService;


  }

  loadEmployeeData() {
    this.employeeService.getAllDepartment().subscribe({
      next: (response: IDepartment[]) => {
        if (response) {
          this.departmentData = response
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while Displaying Departments')

      }
    }),
      this.employeeService.getAllDesignation().subscribe({
        next: (response: IDesignation[]) => {
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
      const params: IDepartmentData = {
        departmentName: this.departmentForm.value.departmentName ?? ''
      }
      this.employeeService.addDepartment(params).subscribe({
        next: (response: number) => {
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
      const designationData: IDesignationData = {
        designationName: this.designationForm.value.designationName ?? ''
      }
      this.employeeService.addDesignation(designationData).subscribe({
        next: (response: number) => {
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
        firstName: formData.firstName ?? '',
        lastName: formData.lastName ?? ''
      }
      this.employeeService.getEmployeeByName(params.firstName, params.lastName).subscribe({
        next: (response: ImanagerData[]) => {
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
  sync() {
    this.employeeService.syncEmployee().subscribe({
      next: (response: boolean) => {
        if (response) {
          this.toastr.success('Employee Sync Success');
        }
        else {
          this.toastr.info('Employees are already up to date')
        }

      },
      error: (err) => {
        this.toastr.error('Error fetching managers');
      }
    });


  }

  onSubmit() {
    if (this.userForm.valid) {
      const formData = this.userForm.value;
      const params: IadminUpdate = {
        id: this.newEmployeeId,
        designationId: formData.designationId ?? 0,
        departmentId: formData.departmentId ?? 0,
        managerId: formData.managerId ?? 0,
        isActive: formData.isActive ?? false,
        joiningDate: formData.joiningDate ?? this.inDate,
        employmentType: Number(formData.employmentType)
      }
      this.employeeService.adminUpdateEmployee(params).subscribe({
        next: (response: boolean) => {
          if (response) {
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

      this.params.employeeCode = formData.employeeCode ?? 0;
      this.params.rfId = formData.rfId ?? '';
      this.employeeService.addEmployee(this.params).subscribe({
        next: (response: number) => {
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