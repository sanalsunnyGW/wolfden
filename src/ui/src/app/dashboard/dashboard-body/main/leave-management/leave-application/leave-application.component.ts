import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { NgSelectComponent } from '@ng-select/ng-select';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILeaveApplication, ILeaveApplicationFormControl } from '../../../../../interface/Leave-Application-Interface';
import { ToastrService } from 'ngx-toastr';
import { ILeaveRequest } from '../../../../../interface/leave-request';

@Component({
  selector: 'app-leave-application',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './leave-application.component.html',
  styleUrl: './leave-application.component.scss'
})
export class LeaveApplicationComponent implements OnInit {

  fb = inject(FormBuilder);
  applyLeave : FormGroup<ILeaveApplicationFormControl>
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []
  destroyRef= inject(DestroyRef);
  toastr = inject(ToastrService)

  constructor(){
    this.applyLeave = this.fb.group<ILeaveApplicationFormControl>({
      empId : new FormControl(null),
      typeId : new FormControl(null,Validators.required),
      halfDay : new FormControl(null),
      fromDate : new FormControl(null,Validators.required),
      toDate : new FormControl(null,Validators.required),
      description : new FormControl(null,Validators.required)
    })
  }

  ngOnInit(){
    this.leaveManagement.getLeaveTypeIdAndName()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe((response : Array<IGetLeaveTypeIdAndname>) => {
              this.leaveType = response;
      });
      
  }
  
  onSubmit(){
    if(this.applyLeave.valid){
      this.leaveManagement.applyLeaveRequest(this.applyLeave.value as ILeaveApplication )
      .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next:(response : ILeaveRequest)=>{
          if(response.successStatus == true)
          {
            this.toastr.success("Leave Request Added")
            this.applyLeave.reset();
          }
          else{
            this.toastr.error(`${response.message}`)
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
