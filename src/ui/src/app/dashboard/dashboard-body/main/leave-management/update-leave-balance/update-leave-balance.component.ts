import { Component, DestroyRef,  OnInit, inject } from '@angular/core';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-update-leave-balance',
  standalone: true,
  imports: [],
  templateUrl: './update-leave-balance.component.html',
  styleUrl: './update-leave-balance.component.scss'
})
export class UpdateLeaveBalanceComponent implements OnInit {
  destroyRef= inject(DestroyRef);

  constructor(private leaveManagementService: LeaveManagementService) { }

  ngOnInit() {                     
    this.leaveManagementService.updateLeaveBalance()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe((data: boolean) => {
        if (data) {
          alert('Leave Balance of All employees Updated !!');
        }
        else {
          alert('Sorry ! Encountered some issues while Updating employees leave balance !')
        }
      });
  }
}



