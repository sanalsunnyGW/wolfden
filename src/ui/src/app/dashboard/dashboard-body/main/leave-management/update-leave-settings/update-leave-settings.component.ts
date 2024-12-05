import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ILeaveUpdate, IUpdateLeaveSetting, IUpdateLeaveSettingFormControl } from '../../../../../interface/update-leave-setting';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-update-leave-settings',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './update-leave-settings.component.html',
  styleUrl: './update-leave-settings.component.scss'
})
export class UpdateLeaveSettingsComponent {
  fb = inject(FormBuilder)
  leaveManagement = inject(LeaveManagementService)
  destroyRef= inject(DestroyRef);
  toastr = inject(ToastrService);
  updateLeaveSetting : FormGroup<IUpdateLeaveSettingFormControl>
  leaveSettings : ILeaveUpdate = {} as ILeaveUpdate;

  constructor(){
    this.updateLeaveSetting = this.fb.group<IUpdateLeaveSettingFormControl>({
      adminId : new FormControl(null),
      minDaysForLeaveCreditJoining : new FormControl(null,Validators.required),
      maxNegativeBalanceLimit : new FormControl(null,Validators.required)
    });
  }

    
ngOnInit(){
  this.leaveManagement.getLeaveSetting()
  .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
    next:(response : ILeaveUpdate) =>{
      this.leaveSettings = response;
      this.updateLeaveSetting.patchValue({
        minDaysForLeaveCreditJoining : this.leaveSettings.minDaysForLeaveCreditJoining,
        maxNegativeBalanceLimit : this.leaveSettings.maxNegativeBalanceLimit   
               });
    },
    error:(error) =>{
      this.toastr.error  (error)
    }
  })
}

  onSubmit(){
    if(this.updateLeaveSetting.valid)
      {
        this.leaveManagement.updateLeaveSettings(this.updateLeaveSetting.value as IUpdateLeaveSetting)
        .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
          next:(response : boolean)=>{
            if(response)
            {
              this.toastr.success("Leave Settings Updated")
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

