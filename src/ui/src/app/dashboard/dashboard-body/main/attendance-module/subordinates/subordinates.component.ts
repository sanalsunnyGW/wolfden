import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';  
import { TreeNodeComponent } from '../tree-node/tree-node.component';
import { Component, inject } from '@angular/core';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { WolfDenService } from '../../../../../service/wolf-den.service';
import { tick } from '@angular/core/testing';
import { NoSubordinateModalComponent } from '../no-subordinate-modal/no-subordinate-modal.component';

@Component({
  selector: 'app-subordinates',
  standalone: true,
  imports: [TreeNodeComponent,CommonModule,MatDialogModule],
  templateUrl: './subordinates.component.html',
  styleUrl: './subordinates.component.scss'
})
export class SubordinatesComponent {
  service=inject(AttendanceService)
  baseService=inject(WolfDenService)
  employeeData: SubordinatesDetails[]=[] ;
  noSubordinate=false;
  dialogRef = inject(MatDialog);
  constructor(public dialog: MatDialog) { 
  }
  ngOnInit() {
    const employeeId=this.baseService.userId
    this.service.getSubOrdinates(employeeId).subscribe(
      (response: SubordinatesDetails) =>{
        if(response){
          this.employeeData=response.subOrdinates || [];  
      }
        else {alert("no subordinate found") };
   });  

   if(this.employeeData.length==0)
    {
      this.dialogRef.open(NoSubordinateModalComponent, { 
      })
    } 
  } 
  
}
