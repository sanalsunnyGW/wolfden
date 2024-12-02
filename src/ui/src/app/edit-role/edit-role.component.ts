import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ImanagerForm } from '../interface/imanager-form';
import { ImanagerData } from '../interface/imanager-data';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../service/employee.service';
import { IroleForm } from '../interface/irole-form';

@Component({
  selector: 'app-edit-role',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-role.component.html',
  styleUrl: './edit-role.component.scss'
})
export class EditRoleComponent {
  managerForm!: FormGroup<ImanagerForm>;
  roleForm!: FormGroup<IroleForm>;
  isDataLoaded: boolean = false;
  isDataClicked: boolean = false;
  employeeClicked: ImanagerData = {
    id: 0,
    firstName: '',
    lastName: '',
    role: ''
  };
  managerData: ImanagerData[] = [{
    id: 0,
    firstName: '',
    lastName: '',
    role: ''
  }];


  constructor(private employeeService: EmployeeService, private fb: FormBuilder, private toastr: ToastrService
  ) {
    this.buildForm();
  }
  employeeId: number = 0;
  private buildForm() {
    this.managerForm = this.fb.group({
      firstName: new FormControl('',),
      lastName: new FormControl(''),

    }),
      this.roleForm = this.fb.group({
        id: new FormControl<number | null>(null, Validators.required),
        role: new FormControl('', Validators.required),
      })
  }

  onSubmit() {
    if (this.managerForm.valid) {
      const formData = this.managerForm.value;
      const params = {
        firstName: formData.firstName,
        lastName: formData.lastName
      };
      this.employeeService.getEmployeeByName(params.firstName, params.lastName).subscribe({
        next: (response: any) => {
          this.managerData = response;
          this.isDataLoaded = true;
          if (this.managerData.length === 0) {
            this.toastr.info('No employees found for the given search.');
          }
          this.managerForm.get('firstName')?.setValue('');
          this.managerForm.get('lastName')?.setValue('');
        },
        error: (err) => {
          this.toastr.error('Error fetching managers');
        }
      });

    }
  }
  roleChange() {
    if (this.roleForm.valid) {
      const formData = this.roleForm.value;
      const params = {
        id: formData.id,
        role: formData.role
      };
      this.employeeService.roleChange(params).subscribe({
        next: (response: any) => {
          this.toastr.success('Role Changed Successfully');
          this.isDataLoaded=false;
          this.isDataClicked = false;

        },
        error: (err) => {
          this.toastr.error('Role Change Failed');
        }
      });
    }
  }

  selectEmployee(employee: any): void {
    this.employeeClicked = employee;
    this.isDataClicked = true;
    this.roleForm.patchValue({
      id: employee.id,
      role: employee.role
    });
  }

}
