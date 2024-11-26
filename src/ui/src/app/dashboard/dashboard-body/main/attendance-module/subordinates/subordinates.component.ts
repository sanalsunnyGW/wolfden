
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';  // Import MatDialogModule
import { TreeNodeComponent } from '../tree-node/tree-node.component';
import { Component, inject } from '@angular/core';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { AttendanceService } from '../../../../../service/attendance.service';

interface EmployeeNode {
  id: number;
  name: string;
  designation: string;
  children?: EmployeeNode[];
}
@Component({
  selector: 'app-subordinates',
  standalone: true,
  imports: [TreeNodeComponent,CommonModule,MatDialogModule],
  templateUrl: './subordinates.component.html',
  styleUrl: './subordinates.component.scss'
})
export class SubordinatesComponent {

  service=inject(AttendanceService)
  employeeData: SubordinatesDetails[]=[] ;
  ngOnInit() {
    const employeeId=1;
    this.service.getSubOrdinates(employeeId).subscribe(
      (response: SubordinatesDetails) =>{
        if(response){
          this.employeeData=response.subOrdinates || [];  
      }
        else {alert("no subordinate found") };
   });   
  }
 
   
}
