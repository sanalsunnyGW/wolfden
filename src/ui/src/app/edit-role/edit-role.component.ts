import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ImanagerForm } from '../interface/imanager-form';
import { ImanagerData } from '../interface/imanager-data';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../service/employee.service';
import { IroleForm } from '../interface/irole-form';
import { ImanagerDataWithPage } from '../interface/imanager-data-with-page';
import { WolfDenService } from '../service/wolf-den.service';
import {MatPaginator, MatPaginatorModule, PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-edit-role',
  standalone: true,
  imports: [ReactiveFormsModule, MatPaginatorModule],
  templateUrl: './edit-role.component.html',
  styleUrl: './edit-role.component.scss'
})
export class EditRoleComponent {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  managerForm!: FormGroup<ImanagerForm>;
  roleForm!: FormGroup<IroleForm>;
  isDataLoaded: boolean = false;
  isDataClicked: boolean = false;
  employeeClicked: ImanagerData = {
    id: 0,
    firstName: '',
    lastName: '',
    role: '',
    employeeCode: 0,
  };
  managerDataWithPage! : ImanagerDataWithPage;
  managerData: ImanagerData[] = [{
    id: 0,
    firstName: '',
    lastName: '',
    role: '',
    employeeCode: 0,
  }];
  pageNumber: number = 0;
  pageSize: number = 5; 
  totalRecords: number = 0;
  pageSizeOptions: number[] = [1, 5, 10, 20]; 




  constructor(private employeeService: EmployeeService, 
              private fb: FormBuilder,  
              private toastr: ToastrService,
              private userService: WolfDenService,
  ) {
    this.buildForm();
  }
  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;
    console.log(this.pageNumber);
    this.pageSize = event.pageSize;
    this.onSubmit();
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

      
      this.userService.getAllEmployeesByName(
        this.pageNumber,
        this.pageSize,
        params.firstName || undefined,
        params.lastName || undefined
      ).subscribe({
        next: (data) => {
          this.managerDataWithPage=data;
          this.managerDataWithPage.employeeNames=data.employeeNames;
          this.managerDataWithPage.totalRecords=data.totalRecords;
          this.totalRecords = data.totalRecords;
          this.managerData = data.employeeNames;
          this.isDataLoaded = true;
          if (this.managerData.length === 0) {
            this.toastr.info('No employees found for the given search.');
            }

        },
        error: (err) => {
              this.toastr.error('Error fetching managers');
            }
      })
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
    this.roleForm.patchValue({
      id: employee.id,
      role: employee.role
    });
  }

}
