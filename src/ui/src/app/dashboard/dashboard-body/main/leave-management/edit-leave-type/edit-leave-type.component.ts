import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { IEditLeaveType, IEditLeaveTypeFormControl } from '../../../../../interface/edit-leave-type';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { IGetLeaveTypeIdAndname } from '../../../../../interface/get-leave-type-interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-leave-type',
  standalone: true,
  imports: [ReactiveFormsModule, NgSelectComponent],
  templateUrl: './edit-leave-type.component.html',
  styleUrl: './edit-leave-type.component.scss'
})
export class EditLeaveTypeComponent implements OnInit {

  fb = inject(FormBuilder);
  leaveManagement = inject(LeaveManagementService);
  editLeaveTypeForm: FormGroup<IEditLeaveTypeFormControl>
  leaveType: Array<IGetLeaveTypeIdAndname> = [];
  selectedType: number | null = null;
  destroyRef = inject(DestroyRef);
  toastr=inject(ToastrService);

  constructor() {

    this.editLeaveTypeForm = this.fb.group<IEditLeaveTypeFormControl>({
      id: new FormControl(null, Validators.required),
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
      sandwich: new FormControl(null)
    });
  }

  increments = [
    { type: 1, viewValue: 'Monthly Increment' },
    { type: 2, viewValue: 'Quarterly Increment' },
    { type: 3, viewValue: 'Half-Yearly Increment' },
  ];

  ngOnInit(): void {
    this.leaveManagement.getLeaveTypeIdAndName()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((response: Array<IGetLeaveTypeIdAndname>) => {
        this.leaveType = response;
      });
  }

  onLeaveTypeChange(event:number) {
    this.leaveManagement.getLeaveDetails(event)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe((response:IEditLeaveType) => {
      if (response) {
        this.editLeaveTypeForm.patchValue({
          maxDays: response.maxDays,
          isHalfDayAllowed: response.isHalfDayAllowed,
          incrementCount: response.incrementCount,
          incrementGapId: response.incrementGapId,
          carryForward: response.carryForward,
          carryForwardLimit: response.carryForwardLimit,
          daysCheck: response.daysCheck,
          daysCheckMore: response.daysCheckMore,
          daysCheckEqualOrLess: response.daysCheckEqualOrLess,
          dutyDaysRequired: response.dutyDaysRequired,
          sandwich: response.sandwich
        })
      }
    });  
  }

  onSubmit() {
    if (this.editLeaveTypeForm.valid) {
      this.leaveManagement.editLeaveType(this.editLeaveTypeForm.value as IEditLeaveType)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe((response) => {
          if (response) {
            this.toastr.success("Leave Type Updated");
            this.editLeaveTypeForm.reset();
          }
          else
          {
            this.toastr.error("Leave Type couldn't be Updated !");
          }
        });
    }
  }
}
