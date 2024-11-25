import { Component, inject } from '@angular/core';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { AttendanceService } from '../../../../../service/attendance.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-subordinates',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './subordinates.component.html',
  styleUrl: './subordinates.component.scss'
})
export class SubordinatesComponent {
  service=inject(AttendanceService)
  subOrdinateData:SubordinatesDetails[]=[]
  ngOnInit() {
    const employeeId=1;
    this.getSubOrdinates(employeeId)
  }
  getSubOrdinates(employeeId:number)
  {
  this.service.getSubOrdinates(employeeId).subscribe(
    (response:SubordinatesDetails[])=>{
      if(response)
      {
        this.subOrdinateData=response;
      }
      else
      {
        alert("no subOrdinates Found");
      }
    }
  )
  }
}
