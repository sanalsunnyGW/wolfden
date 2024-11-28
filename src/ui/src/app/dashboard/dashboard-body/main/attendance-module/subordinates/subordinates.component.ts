import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';  
import { TreeNodeComponent } from '../tree-node/tree-node.component';
import { Component, inject } from '@angular/core';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { WolfDenService } from '../../../../../service/wolf-den.service';

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
  ngOnInit() {
    const employeeId=this.baseService.userId
    this.service.getSubOrdinates(employeeId).subscribe(
      (response: SubordinatesDetails) =>{
        if(response){
          this.employeeData=response.subOrdinates || [];  
      }
        else {alert("no subordinate found") };
   });   
  } 
}
