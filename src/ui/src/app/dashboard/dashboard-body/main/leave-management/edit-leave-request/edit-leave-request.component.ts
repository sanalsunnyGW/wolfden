import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { IEditleave } from '../../../../../interface/edit-leave-application-interface';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-edit-leave-request',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './edit-leave-request.component.html',
  styleUrl: './edit-leave-request.component.scss'
})
export class EditLeaveRequestComponent {

  fb = inject(FormBuilder);
  editLeave : FormGroup
  leaveManagement = inject(LeaveManagementService)
  leaveType : Array<IGetLeaveTypeIdAndname> = []

  constructor(){
    this.editLeave = this.fb.group<IEditleave>({
      leaveRequestId : new FormControl(2),
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

    if(this.editLeave.valid){
      this.leaveManagement.editLeaveRequest(this.editLeave.value).subscribe({
        next:(response : boolean)=>{
          if(response)
          {
            alert("Leave Edited")
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
