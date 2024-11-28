import { CommonModule } from '@angular/common';
import { Component,Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';  
import { MatButtonModule } from '@angular/material/button';  

@Component({
  selector: 'app-modal-details',
  standalone: true,
  imports: [CommonModule,MatDialogModule,MatButtonModule],
  templateUrl: './modal-details.component.html',
  styleUrl: './modal-details.component.scss'
})
export class ModalDetailsComponent {
  constructor(
@Inject(MAT_DIALOG_DATA) public data: {name: string,department:string,designation:string,employeeCode:number,email:string,manager:string},  
    private dialogRef: MatDialogRef<ModalDetailsComponent>  
  ) {}
  close(): void {
    this.dialogRef.close(); 
  }
}

