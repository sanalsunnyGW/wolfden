import { CommonModule } from '@angular/common';
import { Component,Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';  
import { MatButtonModule } from '@angular/material/button';  
import { Router } from '@angular/router';

@Component({
  selector: 'app-no-subordinate-modal',
  standalone: true,
  imports: [MatDialogModule,MatButtonModule],
  templateUrl: './no-subordinate-modal.component.html',
  styleUrl: './no-subordinate-modal.component.scss'
})
export class NoSubordinateModalComponent {
  constructor(
        private router:Router,private dialogRef: MatDialogRef<NoSubordinateModalComponent>  
      ) {}
      close(): void {
        this.dialogRef.close();
         this.router.navigate(['portal/dashboard'])
      }
}
