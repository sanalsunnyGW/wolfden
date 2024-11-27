import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormControlName, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {  ILeaveApplicationFormControl } from '../../../../../interface/leave-application-interface';
import { CommonModule } from '@angular/common';
import { NgSelectComponent } from '@ng-select/ng-select';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';

@Component({
  selector: 'app-leave-application',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,NgSelectComponent],
  templateUrl: './leave-application.component.html',
  styleUrl: './leave-application.component.scss'
})
export class LeaveApplicationComponent {

  fb = inject(FormBuilder);
  applyLeave : FormGroup
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []

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
