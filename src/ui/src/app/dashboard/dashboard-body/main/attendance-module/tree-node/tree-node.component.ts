import { CommonModule } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { ModalDetailsComponent } from '../modal-details/modal-details.component';  
import { MatDialog } from '@angular/material/dialog';
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tree-node',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tree-node.component.html',
  styleUrl: './tree-node.component.scss',
})

export class TreeNodeComponent {
  node! : SubordinatesDetails;
  dialogRef = inject(MatDialog);
  constructor(public dialog: MatDialog,private router:Router) {}
  @Input()
  set item(node2: SubordinatesDetails) {
    this.node = node2;
  }
  expanded = false;
  toggle(): void {
    this.expanded = !this.expanded; 
  }
  getAttendance(id:number)
  {
    this.router.navigate(['portal/attendance/history',id])
  }
  openModal(node:SubordinatesDetails)
  {
    this.dialogRef.open(ModalDetailsComponent, {
      data: {
        name: node.name,
        employeeCode: node.employeeCode,
        email: node.email,
        department: node.department,
        designation: node.designation,
        manager:node.manager
      }
    })
  }
}

