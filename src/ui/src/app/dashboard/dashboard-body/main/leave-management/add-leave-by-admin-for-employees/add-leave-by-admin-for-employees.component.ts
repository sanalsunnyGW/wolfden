import { Component, DestroyRef, Inject, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { IAddLeaveByAdminForEmployee, IAddLeaveByAdminForEmployeeFormControl } from '../../../../../interface/add-leave-by-admin-for-employee';
import { NgSelectComponent } from '@ng-select/ng-select';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-leave-by-admin-for-employees',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './add-leave-by-admin-for-employees.component.html',
  styleUrl: './add-leave-by-admin-for-employees.component.scss'
})
export class AddLeaveByAdminForEmployeesComponent implements OnInit{

  destroyRef= inject(DestroyRef);
  fb = inject(FormBuilder);
  applyLeave : FormGroup<IAddLeaveByAdminForEmployeeFormControl>
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []
  toastr = inject(ToastrService)

  constructor(){
    this.applyLeave = this.fb.group<IAddLeaveByAdminForEmployeeFormControl>({
      adminId : new FormControl(null),
      employeeCode : new FormControl(null,Validators.required),
      typeId : new FormControl(null,Validators.required),
      halfDay : new FormControl(null),
      fromDate : new FormControl(null,Validators.required),
      toDate : new FormControl(null,Validators.required),
      description :  new FormControl(null,Validators.required)
    })
  }

  ngOnInit(){
    this.leaveManagement.getLeaveTypeIdAndName()
    .pipe(takeUntilDestroyed(this.destroyRef)) .subscribe({
      next:(response : Array<IGetLeaveTypeIdAndname>) =>{
              this.leaveType = response
      },
      error:(error) => {
        this.toastr.error(error)
      }
    })
  }

  onSubmit(){
    if(this.applyLeave.valid){

      this.leaveManagement.applyLeaveByAdminforEmployee(this.applyLeave.value as IAddLeaveByAdminForEmployee)
      .pipe(takeUntilDestroyed(this.destroyRef)) .subscribe({
        next:(response : boolean)=>{
          if(response)
          {
            this.toastr.success("Leave Added")
            this.applyLeave.reset();
          }
        },
          error:(error) =>{
            this.toastr.error(error)
            }
       }
       
       );

    }

  }

}





