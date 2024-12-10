import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { NgSelectModule } from '@ng-select/ng-select';
import { AttendanceService } from '../../../../../service/attendance.service';
import { AttendanceHistory } from '../../../../../interface/attendance-history';
import { WolfDenService } from '../../../../../service/wolf-den.service';
import { ActivatedRoute } from '@angular/router';
import { DurationFormatPipe } from "../../../../../pipe/duration-format.pipe";
import { SplitCommaPipe } from "../../../../../pipe/split-comma.pipe";
import { SubordinatesDetails } from '../../../../../interface/subordinates-details';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { TreeNodeComponent } from "../tree-node/tree-node.component";
import { SubordinatesComponent } from "../subordinates/subordinates.component";
import { TreeSelectModule } from 'primeng/treeselect';
import { TreeNode } from '../../../../../interface/tree-node';

@Component({
  selector: 'app-attendance-history',
  standalone: true,
  imports: [ReactiveFormsModule, NgSelectModule, CommonModule, FormsModule, MatPaginatorModule, DurationFormatPipe, SplitCommaPipe, MatMenuModule,
    MatButtonModule, TreeSelectModule],
  templateUrl: './attendance-history.component.html',
  styleUrl: './attendance-history.component.scss'
})
export class AttendanceHistoryComponent implements OnInit {

  service = inject(AttendanceService);
  baseService = inject(WolfDenService);
  selectedYear: number;
  selectedMonth!: number;
  selectedStatus: number = 0;
  selectedSubordinate: TreeNode|null = null;  
  selectedPageSize: number = 5;
  selectedPageNumber: number = 0;
  attendanceData: any[] = [];


  employeeId = this.baseService.userId;
  years: number[] = [];
  pageSizes = [5, 10, 20, 30, 40];
  totalPages!: number;
  noRecordFound!: boolean;
  currentSubordinates: TreeNode[]=[];

  constructor(private route: ActivatedRoute) {
    this.selectedYear = new Date().getFullYear();
    for (let year = this.selectedYear; year >= 2020; year--) {
      this.years.push(year);
    }
  }

  id!: number;

  ngOnInit(): void {
    this.fetchSubordinates();
  }

  months = [
    { id: 1, name: 'January' },
    { id: 2, name: 'February' },
    { id: 3, name: "March" },
    { id: 4, name: 'April' },
    { id: 5, name: 'May' },
    { id: 6, name: "June" },
    { id: 7, name: 'July' },
    { id: 8, name: 'August' },
    { id: 9, name: "September" },
    { id: 10, name: 'October' },
    { id: 11, name: 'November' },
    { id: 12, name: "December" },
  ];

  status = [
    { id: 1, name: 'Present' },
    { id: 2, name: 'Absent' },
    { id: 3, name: 'Incomplete Shift' },
    { id: 4, name: 'Restricted Holiday' },
    { id: 5, name: 'Normal Holiday' },
    { id: 6, name: "WFH" },
    { id: 7, name: 'Leave' },
    { id: 9, name: 'Half Day' },
    { id: 10, name: 'Weekend' },
    { id: 11, name: 'All' }
  ];

  fetchSubordinates() {
    this.service.getSubOrdinates(this.employeeId).subscribe((response: SubordinatesDetails) => {
      this.currentSubordinates = this.formatSubordinates(response.subOrdinates!) || [];
      console.log(this.currentSubordinates);
    });
  }

  formatSubordinates(subordinates: SubordinatesDetails[]): TreeNode[] {
    return subordinates.map(sub => {
      return {
        label: sub.name,  
        value: sub.id,    
        children: sub.subOrdinates ? this.formatSubordinates(sub.subOrdinates) : []  
      };
    });
  }

  fetchHistory() {
    const user = this.selectedSubordinate ? this.selectedSubordinate.value : this.employeeId;
    this.service.getMonthlyHistoryByStatus(
      user,
      this.selectedYear,
      this.selectedMonth,
      this.selectedStatus,
      this.selectedPageNumber,
      this.selectedPageSize
    ).subscribe(
      (response: AttendanceHistory) => {
        if (response.attendanceHistory && response.attendanceHistory.length === 0) {
          this.attendanceData = [];
          this.noRecordFound = true;
        } else {
          this.attendanceData = response.attendanceHistory;
          this.totalPages = response.totalPages;
          this.noRecordFound = false;
        }
      }
    );
  }

  getStatusName(statusId: number): string {
    const statusObj = this.status.find(s => s.id === statusId);
    return statusObj ? statusObj.name : 'Unknown';
  }

  getStatusClass(statusId: number): string {
    switch (statusId) {
      case 1: return 'status-present';
      case 2: return 'status-absent';
      case 3: return 'status-incomplete';
      case 4: return 'status-restricted';
      case 5: return 'status-normal-holiday';
      case 6: return 'status-wfh';
      case 7: return 'status-leave';
      case 10: return 'status-weekend';
      default: return 'status-unknown';
    }
  }

  onPageChange(event: PageEvent): void {
    this.selectedPageNumber = event.pageIndex;
    this.selectedPageSize = event.pageSize;
    this.fetchHistory();
  }

  onSubmit(): void {
    this.selectedPageNumber = 0;
    this.fetchHistory();
  }
}
