import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import {  IEditleave, IEditleaveFormControl } from '../../../../../interface/edit-leave-application-interface';
import { NgSelectComponent } from '@ng-select/ng-select';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ILeaveRequest } from '../../../../../interface/leave-request';
import { ILeaveRequestHistory } from '../../../../../interface/leave-request-history';

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
  leaveRequestId : number | null = 0
  idParams: string |null =''
  // id =  this.router.params.subscribe(params => {
  //   const id = params['leaveRequest'];
  //   });
  // leaveRequestId = this.id ? Number(this.id) : null;

  constructor(){

    

    this.editLeave = this.fb.group<IEditleaveFormControl>({
      leaveRequestId : new FormControl(null),
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

    this.idParams =  this.router.snapshot.paramMap.get('leaveRequestId')
    this.leaveRequestId = this.idParams ? Number(this.idParams) : null;

    this.leaveManagement.getLeaveRequest(this.leaveRequestId as number)
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next:(response : IEditleave) =>{

        this.editLeave.patchValue({
          leaveRequestId : response.leaveRequestId,
          typeId : response.typeId,
          halfDay : response.halfDay,
          fromDate : response.fromDate,
          toDate : response.toDate,
          description : response.description,
    
        });
      }
    })
  



  }

  onSubmit(){

    if(this.editLeave.valid){
      this.leaveManagement.editLeaveRequest(this.editLeave.value as IEditleave)
      .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next:(response : ILeaveRequest)=>{
          if(response.successStatus== true)
          {
            this.toastr.success("Leave Edited")
            this.editLeave.reset();
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
