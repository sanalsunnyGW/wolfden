import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { IAddLeaveByAdminForEmployee, IAddLeaveByAdminForEmployeeFormControl } from '../../../../../interface/add-leave-by-admin-for-employee';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-add-leave-by-admin-for-employees',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './add-leave-by-admin-for-employees.component.html',
  styleUrl: './add-leave-by-admin-for-employees.component.scss'
})
export class AddLeaveByAdminForEmployeesComponent {


  fb = inject(FormBuilder);
  applyLeave : FormGroup
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []

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
    this.leaveManagement.getLeaveTypeIdAndName().subscribe({
      next:(response : Array<IGetLeaveTypeIdAndname>) =>{
              this.leaveType = response
      },
      error:(error) => {
        alert(error)
      }
    })
  }

  onSubmit(){
    if(this.applyLeave.valid){

      const leaveData: IAddLeaveByAdminForEmployee = {
        adminId: this.applyLeave.get('adminId')?.value ?? null,
        employeeCode: this.applyLeave.get('employeeCode')?.value,
        typeId: this.applyLeave.get('typeId')?.value,
        halfDay: this.applyLeave.get('halfDay')?.value,
        fromDate: this.applyLeave.get('fromDate')?.value,
        toDate: this.applyLeave.get('toDate')?.value,
        description: this.applyLeave.get('description')?.value,
      };
      this.leaveManagement.applyLeaveByAdminforEmployee(this.applyLeave.value).subscribe({
        next:(response : boolean)=>{
          if(response)
          {
            alert("Leave Added")
          }
        },
          error:(error) =>{
            alert(error)
            }
       }
       
       );

    }

  }

}
