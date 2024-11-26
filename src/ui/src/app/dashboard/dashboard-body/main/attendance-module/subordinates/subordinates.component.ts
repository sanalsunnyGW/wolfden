
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';  
import { TreeNodeComponent } from '../tree-node/tree-node.component';
import { Component, inject } from '@angular/core';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../Interface/subordinates-details';


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
    const employeeId=2;
    this.service.getSubOrdinates(employeeId).subscribe(
      (response: SubordinatesDetails) =>{
        if(response){
          this.employeeData=response.subOrdinates || [];  
          console.log(response);
      }
        else {alert("no subordinate found") };
   });   
  }
 
   
}
