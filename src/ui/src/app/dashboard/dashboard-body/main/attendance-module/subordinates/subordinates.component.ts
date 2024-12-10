import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';  
import { TreeNodeComponent } from '../tree-node/tree-node.component';
import { Component, inject, Input } from '@angular/core';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { WolfDenService } from '../../../../../service/wolf-den.service';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-subordinates',
  standalone: true,
  imports: [CommonModule,MatDialogModule,MatMenuModule,MatButtonModule],
  templateUrl: './subordinates.component.html',
  styleUrl: './subordinates.component.scss'
})
export class SubordinatesComponent {
  @Input() subordinates: any[] = [];
}
