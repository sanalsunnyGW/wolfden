import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormControlName, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ILeaveApplication } from '../../../../../interface/Leave-Application-Interface';
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
  applyLeave : FormGroup<ILeaveApplication>
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []

  constructor(){
    this.applyLeave = this.fb.group<ILeaveApplication>({
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

  }



}
