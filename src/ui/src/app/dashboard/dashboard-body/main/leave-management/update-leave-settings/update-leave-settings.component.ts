import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ILeaveUpdate, IUpdateLeaveSettingFormControl } from '../../../../../interface/update-leave-setting';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';

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

  updateLeaveSetting : FormGroup
  leaveSettings : ILeaveUpdate = {} as ILeaveUpdate;

  constructor(){
    this.updateLeaveSetting = this.fb.group<IUpdateLeaveSettingFormControl>({
      adminId : new FormControl(null),
      minDaysForLeaveCreditJoining : new FormControl(null,Validators.required),
      maxNegativeBalanceLimit : new FormControl(null,Validators.required)
    });
  }

    


  onSubmit(){
    if(this.updateLeaveSetting.valid)
      {
        this.leaveManagement.updateLeaveSettings(this.updateLeaveSetting.value).subscribe({
          next:(response : boolean)=>{
            if(response)
            {
              alert("Leave Settings Updated")
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
