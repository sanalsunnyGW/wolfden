import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, inject, Input, signal } from '@angular/core';
import { ModalDetailsComponent } from '../modal-details/modal-details.component';  
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { AttendanceService } from '../../../../../service/attendance.service';
import { SubordinatesDetails } from '../../../../../Interface/subordinates-details';


@Component({
  selector: 'app-tree-node',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tree-node.component.html',
  styleUrl: './tree-node.component.scss',
  // changeDetection : ChangeDetectionStrategy.OnPush
})
export class TreeNodeComponent {
  selected!:SubordinatesDetails
  status=false;
  name:string=""
  newName = signal<string>('');
  testName="";
  node! : SubordinatesDetails;
  isModalOpened = signal<boolean>(false);
  imageUrl: string = 'https://picsum.photos/300/200';
  dialogRef = inject(MatDialog);
  constructor(public dialog: MatDialog,private cd : ChangeDetectorRef,
   
  ) {   


  }
  @Input()
  set item(node2: SubordinatesDetails) {
    this.node = node2;
    console.log(this.node)
    this.newName.set(node2.name);
    this.testName =node2.name;
    this.cd.markForCheck();
  }
  expanded = false;
  toggle(): void {
    this.expanded = !this.expanded; 
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

