import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { Router } from '@angular/router';
import mermaid from 'mermaid';
import { EmployeeServiceService } from '../Service/employee-service.service';

@Component({
  selector: 'app-my-team',
  standalone: true,
  imports: [],
  templateUrl: './my-team.component.html',
  styleUrl: './my-team.component.scss'
})
export class MyTeamComponent {

  @ViewChild('mermaidDiv', { static: false }) mermaidDiv!: ElementRef;
  router = inject(Router)
  employeeData: any[] = [];
  employeeDataModal: any[] = [];
  isDataLoaded: boolean = false;
  service = inject(EmployeeServiceService)

  ngOnInit(): void {
    this.loadEmployeeHierarchy();
    (window as any).onA = (nodeName: string) => {
      console.log('Node clicked:', nodeName);
      this.router.navigate(['/employee-display']);
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
    this.service.getMyTeamHierarchy(true).subscribe({
      next: (response: any) => {
        if (response) {
          this.employeeDataModal = response;
          this.isDataLoaded = true;
          this.renderMermaidChart();
        } else {
          alert('No Employee found');
        }
      },
      error: (error) => {
        console.error('Error Displaying Hierarchy:', error);
        alert('An error occurred while displaying hierarchy');
      },
    });
  }

  loadEmployeeHierarchy() {
    this.service.getMyTeamHierarchy(false).subscribe({
      next: (response: any) => {
        if (response) {
          this.employeeData = response;
          this.isDataLoaded = true;
          this.renderMermaidChart();
          console.log('Employee data loaded:', this.employeeData);
        } else {
          alert('No Employee found');
        }
      },
      error: (error) => {
        console.error('Error Displaying Hierarchy:', error);
        alert('An error occurred while displaying hierarchy');
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
      const nodeId = `Node${emp.id}`;
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


