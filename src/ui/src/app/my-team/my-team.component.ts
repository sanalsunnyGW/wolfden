import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import mermaid from 'mermaid';
import { IEmployeeData } from '../Interface/employee-data';
import { EmployeeService } from '../Service/employee.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-my-team',
  standalone: true,
  imports: [],
  templateUrl: './my-team.component.html',
  styleUrl: './my-team.component.scss'
})
export class MyTeamComponent {

  @ViewChild('mermaidDiv', { static: false }) mermaidDiv!: ElementRef;
  constructor(private router: Router, private employeeService: EmployeeService, private toastr: ToastrService
  ) { }
  inDate = new Date();
  isDataLoaded: boolean = false;
  employeeData: IEmployeeData[] = [{
    id: 0,
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: this.inDate,
    designationId: 0,
    designationName: '',
    departmentId: 0,
    departmentName: '',
    managerId: 0,
    managerName: '',
    isActive: true,
    address: '',
    country: '',
    state: '',
    employmentType: 0,
    photo: '',
    subordinates: []
  }];
  employeeDataModal: IEmployeeData[] = [{
    id: 0,
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: this.inDate,
    designationId: 0,
    designationName: '',
    departmentId: 0,
    departmentName: '',
    managerId: 0,
    managerName: '',
    isActive: true,
    address: '',
    country: '',
    state: '',
    employmentType: 0,
    photo: '',
    subordinates: []
  }];
  ngOnInit(): void {
    this.loadEmployeeHierarchy();
    (window as any).onA = (nodeName: string) => {
      console.log(nodeName)
      this.router.navigate(['/dashboard/employee-display'], { queryParams: { id: nodeName } });
    };

    mermaid.initialize({
      securityLevel: 'loose',
      theme: 'base',
      flowchart: {
        nodeSpacing: 100,
        rankSpacing: 80,
        padding: 20
      },

      themeVariables: {
        primaryColor: '#0369a1',
        primaryTextColor: '#ffffff',
        lineColor: '#7f8b9b',
        nodeBorder: '#075985',

      },
      themeCSS: `
        .node rect {
          rx: 8px;                    /* Rounded corners */
          height: 60px !important;    /* Fixed height for nodes */
          transition: all 0.3s ease;
        }
        .node:hover rect {
          fill: #113c60 !important;  
        }
        .node:hover text {
          fill: #ffffff !important;
        }
      `
    });

  }
  viewTeamHierarchy() {
    this.employeeService.getMyTeamHierarchy(true).subscribe({
      next: (response: any) => {
        if (response) {
          this.employeeDataModal = response;
          this.isDataLoaded = true;
          this.renderMermaidChart();
        } else {
          this.toastr.error('No Employee found')

        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while displaying hierarchy')

      },
    });
  }

  loadEmployeeHierarchy() {
    this.employeeService.getMyTeamHierarchy(false).subscribe({
      next: (response: any) => {
        if (response) {
          console.log(response)
          this.employeeData = response;
          this.isDataLoaded = true;
          this.renderMermaidChart();
        } else {
          this.toastr.error('No Employee found')
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while displaying hierarchy')
      },
    });

  }
  private renderMermaidChart() {
    if (!this.isDataLoaded || !this.employeeDataModal) return;
    const graphDefinition = this.generateMermaidGraph(this.employeeDataModal);
    mermaid
      .render('mermaidDiv', graphDefinition)
      .then(({ svg, bindFunctions }) => {
        const element = this.mermaidDiv.nativeElement;
        element.innerHTML = svg;
        bindFunctions?.(element);
        setTimeout(() => {
          const svgElement = element.querySelector('svg');
          if (svgElement) {
            svgElement.style.display = 'block'; // Ensure block-level display
            svgElement.style.margin = '0 auto'; // Centering the SVG
          }
        }, 0);
      })
      .catch((error) => {
        console.error('Mermaid render error:', error);
      });
  }

  private generateMermaidGraph(employees: any[]): string {
    let graph = 'graph TB;\n';
    const traverse = (emp: any) => {
      const nodeId = `${emp.id}`;
      const nodeLabel = `${emp.firstName || 'No Name'} ${emp.lastName || ''}`.trim();
      graph += `${nodeId}[${nodeLabel}]\n`;
      if (emp.subordinates && Array.isArray(emp.subordinates) && emp.subordinates.length > 0) {
        emp.subordinates.forEach((subordinate: any) => {
          const subNodeId = `Node${subordinate.id}`;
          graph += `${nodeId}-->${subNodeId}\n`;
          traverse(subordinate);
        });
      }
      graph += `click ${nodeId} onA\n`;
    };
    employees.forEach(employee => traverse(employee));
    return graph;
  }
}


