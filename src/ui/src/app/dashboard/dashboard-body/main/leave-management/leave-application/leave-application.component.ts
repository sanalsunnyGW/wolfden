import { Component, DestroyRef, Inject, inject } from '@angular/core';
import { FormBuilder, FormControl, FormControlName, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {  ILeaveApplicationFormControl } from '../../../../../interface/leave-application-interface';
import { NgSelectComponent } from '@ng-select/ng-select';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';


@Component({
  selector: 'app-leave-application',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './leave-application.component.html',
  styleUrl: './leave-application.component.scss'
})
export class LeaveApplicationComponent {

  fb = inject(FormBuilder);
  applyLeave : FormGroup
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []
  destroyRef= Inject(DestroyRef);

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
      console.log(this.applyLeave);
      this.leaveManagement.applyLeaveRequest(this.applyLeave.value).subscribe({
        next:(response : boolean)=>{
          if(response)
          {
            alert("Leave Request Added")
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
