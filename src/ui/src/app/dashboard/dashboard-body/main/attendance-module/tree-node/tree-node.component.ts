import { CommonModule } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { ModalDetailsComponent } from '../modal-details/modal-details.component';  
import { MatDialog } from '@angular/material/dialog';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';


@Component({
  selector: 'app-tree-node',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tree-node.component.html',
  styleUrl: './tree-node.component.scss'
})
export class TreeNodeComponent {
  data!:SubordinatesDetails
  imageUrl: string = 'https://picsum.photos/300/200';
  constructor(public dialog: MatDialog) {   
  }
  @Input() node!: SubordinatesDetails;
  expanded = false;
  toggle(): void {
    this.expanded = !this.expanded; 
  }
  openDetails(node: SubordinatesDetails): void {
    // this.dialog.open(ModalDetailsComponent, {
    //   data: { node },  
    // });
    this.data=node;

  }
}

