import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, NgSelectOption, ReactiveFormsModule, Validators } from '@angular/forms';
import { IAddNewLeaveType } from '../../../../../interface/add-new-leave-type-interface';
import { CommonModule } from '@angular/common';
import { NgSelectComponent } from '@ng-select/ng-select';
import { LeaveManagementService } from '../../../../../service/leave-management.service';

@Component({
  selector: 'app-add-new-leave-type',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,NgSelectComponent],
  templateUrl: './add-new-leave-type.component.html',
  styleUrl: './add-new-leave-type.component.scss'
})
export class AddNewLeaveTypeComponent {
  fb = inject(FormBuilder)
  leaveManagement = inject(LeaveManagementService)

    addNewLeaveType : FormGroup<IAddNewLeaveType>
    constructor() {
      this.addNewLeaveType = this.fb.group<IAddNewLeaveType>({
        typeName: new FormControl(null,Validators.required),
        maxDays: new FormControl(null),
        isHalfDayAllowed: new FormControl(null),
        incrementCount: new FormControl(null),
        incrementGap: new FormControl(null),
        carryForward: new FormControl(null),
        carryForwardLimit: new FormControl(null),
        daysCheck: new FormControl(null),
        daysCheckMore: new FormControl(null),
        daysCheckEqualOrLess: new FormControl(null),
        dutyDaysRequired: new FormControl(null),
        type: new FormControl(null),
        sandwich : new FormControl(null)
      });
  }
  selectedType : number|null =null
  increments = [
    { type: 1, viewValue: 'Monthly Increment' },
    { type: 2, viewValue: 'Quarterly Increment' },
    { type: 3, viewValue: 'Half-Yearly Increment' },
    ];



      

      onSubmit()
      {
        if(this.addNewLeaveType.valid)
        {
          this.leaveManagement.addNewLeaveType(this.addNewLeaveType).subscribe({
            next:(response : boolean)=>{
              if(response)
              {
                alert("New Leave Type Added")
              }
            },
              error:(error) =>{
                console.log(error)
                alert(error)
                }
           }
           
           );
        }
      }
}
