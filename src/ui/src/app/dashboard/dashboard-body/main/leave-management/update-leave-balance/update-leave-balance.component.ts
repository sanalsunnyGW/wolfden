import { Component, DestroyRef,  OnInit, inject } from '@angular/core';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { WolfDenService } from '../../../../../service/wolf-den.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-leave-balance',
  standalone: true,
  imports: [],
  templateUrl: './update-leave-balance.component.html',
  styleUrl: './update-leave-balance.component.scss'
})
export class UpdateLeaveBalanceComponent implements OnInit {
  destroyRef= inject(DestroyRef);
  toastr=inject(ToastrService);
  
  constructor(private leaveManagementService: LeaveManagementService) { }

  ngOnInit() {                     
    this.leaveManagementService.updateLeaveBalance()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe((data: boolean) => {
        if (data) {
          this.toastr.success('Leave Balance of All employees Updated !!');
        }
        else {
          this.toastr.error(' Sorry ! We have Encountered some issues while Updating employees leave balance !')
        }
      });
  }
}




