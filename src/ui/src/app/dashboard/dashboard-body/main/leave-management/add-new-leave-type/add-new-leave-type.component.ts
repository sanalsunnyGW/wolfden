import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, NgSelectOption, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IAddNewLeaveType, IAddNewLeaveTypeFormcontrol } from '../../../../../interface/Add-New-Leave-Type-Interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-new-leave-type',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectComponent],
  templateUrl: './add-new-leave-type.component.html',
  styleUrl: './add-new-leave-type.component.scss'
})
export class AddNewLeaveTypeComponent{
  fb = inject(FormBuilder)
  leaveManagement = inject(LeaveManagementService)
  destroyRef= inject(DestroyRef);
  toastr = inject(ToastrService)

    addNewLeaveType : FormGroup<IAddNewLeaveTypeFormcontrol>
    constructor() {
      this.addNewLeaveType = this.fb.group<IAddNewLeaveTypeFormcontrol>({
        typeName: new FormControl(null,Validators.required),
        maxDays: new FormControl(null),
        isHalfDayAllowed: new FormControl(null),
        incrementCount: new FormControl(null),
        incrementGapId: new FormControl(null),
        carryForward: new FormControl(null),
        carryForwardLimit: new FormControl(null),
        daysCheck: new FormControl(null),
        daysCheckMore: new FormControl(null),
        daysCheckEqualOrLess: new FormControl(null),
        dutyDaysRequired: new FormControl(null),
        sandwich: new FormControl(null),
      
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
          this.leaveManagement.addNewLeaveType(this.addNewLeaveType.value as IAddNewLeaveType )
          .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
            next:(response : boolean)=>{
              if(response)
              {
                this.toastr.success("New Leave Type Added")
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


