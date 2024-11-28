import { Component } from '@angular/core';
import { Idepartment } from '../Interface/idepartment';
import { Idesignation } from '../Interface/idesignation';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IdepartmentForm } from '../Interface/idepartment-form';
import { IdesignationForm } from '../Interface/idesignation-form';
import { EmployeeService } from '../Service/employee.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {

  depatmentData: Idepartment = {
    id: 0,
    departmentName: ''
  }
  designationData: Idesignation = {
    id: 0,
    designationName: ''
  }
  departmentForm!: FormGroup<IdepartmentForm>
  designationForm!: FormGroup<IdesignationForm>

  constructor(private employeeService: EmployeeService, private fb: FormBuilder,private toastr: ToastrService) {
    this.buildForm();
  }
  employeeId: number = 0;
  private buildForm() {
    this.departmentForm = this.fb.group({
      departmentName: new FormControl('', Validators.required)
    }),
      this.designationForm = this.fb.group({
        designationName: new FormControl('', Validators.required)
      })

  }

  ngOnInit() {

    // this.loadEmployeeData();
  }


  // loadEmployeeData() {
  //   this.employeeService.getAllDepartment().subscribe({
  //     next: (response: any) => {
  //       if (response) {
  //         this.depatmentData=response;

  //       }
  //     },
  //     error: (error) => {
        //this.toastr.error('An error occurred while Displaying Departments')

  //     }
  //   }),
  //   this.employeeService.getAllDesignation().subscribe({
  //     next: (response: any) => {
  //       if (response) {
  //         this.designationData=response;
  //       }
  //     },
  //     error: (error) => {
    //this.toastr.error('An error occurred while Displaying Designations')

  //     }
  //   })
  // }

  onSubmit() {
    if (this.departmentForm.valid) {
      const formData = this.departmentForm.value;
      const params = {
        departmentName: formData.departmentName
      }
      this.employeeService.addDepartment(params).subscribe({
        next: (response: any) => {
          if (response > 0) {
            this.toastr.success('Department added Successfully')
           // this.loadEmployeeData();
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Adding Departments')


        }
      })
    }

    if (this.designationForm.valid) {
      const formData = this.designationForm.value;
      const params = {
        designationName: formData.designationName
      }
      this.employeeService.addDesignation(params).subscribe({
        next: (response: any) => {
          if (response > 0) {
            this.toastr.success('Designation added Successfully')
            // this.loadEmployeeData();
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Adding Designation')

        }
      })
    }

  }
}