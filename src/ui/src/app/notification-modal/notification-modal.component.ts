import { Component, Input, inject } from '@angular/core';
import {  DatePipe } from '@angular/common';
import { INotificationForm } from '../interface/i-notification-form';
import { WolfDenService } from '../service/wolf-den.service';

@Component({
  selector: 'app-notification-modal',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './notification-modal.component.html',
  styleUrl: './notification-modal.component.scss'
})
export class NotificationModalComponent {
  @Input() notifications: INotificationForm[] = [];
  @Input() onClose: () => void = () => {};
  userService=inject(WolfDenService);


  markNotificationAsRead(notificationId: number): void {
    this.userService.markAsRead(notificationId).subscribe({
      next: (response) => {
        this.notifications = this.notifications.filter(notification => notification.notificationId !== notificationId);
        
      },
    });
  }
  markAllAsRead(employeeId: number): void{
    this.userService.markAllAsRead(employeeId).subscribe({
      next: (response) => {
        this.notifications = this.notifications.filter(notification => notification !== notification);
      }
    })
  }
}

