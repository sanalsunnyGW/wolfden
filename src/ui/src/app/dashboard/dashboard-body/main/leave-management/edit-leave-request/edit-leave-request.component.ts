import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import {  IEditleave, IEditleaveFormControl } from '../../../../../interface/edit-leave-application-interface';
import { NgSelectComponent } from '@ng-select/ng-select';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-leave-request',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './edit-leave-request.component.html',
  styleUrl: './edit-leave-request.component.scss'
})
export class EditLeaveRequestComponent implements OnInit {

  fb = inject(FormBuilder);
  editLeave : FormGroup<IEditleaveFormControl>
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []
  destroyRef= inject(DestroyRef);
  router = inject(ActivatedRoute)
  toastr = inject(ToastrService)
  id =  this.router.snapshot.paramMap.get('leaveRequestId')
  leaveRequestId = this.id ? Number(this.id) : null;

  constructor(){
    this.editLeave = this.fb.group<IEditleaveFormControl>({
      leaveRequestId : new FormControl(this.leaveRequestId),
      typeId : new FormControl(null,Validators.required),
      halfDay : new FormControl(null),
      fromDate : new FormControl(null,Validators.required),
      toDate : new FormControl(null,Validators.required),
      description : new FormControl(null,Validators.required)
    })
  }

  ngOnInit(){
    this.leaveManagement.getLeaveTypeIdAndName()
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next:(response : Array<IGetLeaveTypeIdAndname>) =>{
              this.leaveType = response
      },
      error:(error) => {
        this.toastr.error(error)
      }
    })
  }

  onSubmit(){

    if(this.editLeave.valid){
      this.leaveManagement.editLeaveRequest(this.editLeave.value as IEditleave)
      .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next:(response : boolean)=>{
          if(response)
          {
            this.toastr.success("Leave Edited")
            this.editLeave.reset();
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
