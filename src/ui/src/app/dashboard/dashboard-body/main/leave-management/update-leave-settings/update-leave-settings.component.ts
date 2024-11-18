import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../../../../../interface/update-leave-setting';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-leave-settings',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './update-leave-settings.component.html',
  styleUrl: './update-leave-settings.component.scss'
})
export class UpdateLeaveSettingsComponent {
  fb = inject(FormBuilder)
  leaveManagement = inject(LeaveManagementService)

  updateLeaveSetting : FormGroup<IUpdateLeaveSetting>
  leaveSettings : ILeaveUpdate = {} as ILeaveUpdate;

  constructor(){
    this.updateLeaveSetting = this.fb.group<IUpdateLeaveSetting>({
      minDaysForLeaveCreditJoining : new FormControl(null,Validators.required),
      maxNegativeBalanceLimit : new FormControl(null,Validators.required)
    });
  }

    
  ngOnInit(){
    this.leaveManagement.getLeaveSetting().subscribe({
      next:(response : ILeaveUpdate)=>{
        this.leaveSettings = response
        if(response)
        {
          this.updateLeaveSetting.patchValue({
            minDaysForLeaveCreditJoining : this.leaveSettings.minDaysForLeaveCreditJoining,
            maxNegativeBalanceLimit : this.leaveSettings.maxNegativeBalanceLimit
          });
        }
        console.log(response)
      },
        error:(error) =>{
          console.log(error)
          alert(error)
          }
     });
  }

  onSubmit(){
    if(this.updateLeaveSetting.valid)
      {
        this.leaveManagement.updateLeaveSettings(this.updateLeaveSetting).subscribe({
          next:(response : boolean)=>{
            if(response)
            {
              alert("Leave Settings Updated")
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
