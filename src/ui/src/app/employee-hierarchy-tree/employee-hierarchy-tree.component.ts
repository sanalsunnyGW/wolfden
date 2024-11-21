import { AfterViewChecked, Component, ElementRef, Input, OnInit, ViewChild, inject } from '@angular/core';
import { Router } from '@angular/router';
import mermaid from 'mermaid';
import { EmployeeServiceService } from '../services/employee-service.service';


@Component({
  selector: 'app-employee-hierarchy-tree',
  standalone: true,
  imports: [],
  templateUrl: './employee-hierarchy-tree.component.html',
  styleUrl: './employee-hierarchy-tree.component.scss',
})
export class EmployeeHierarchyTreeComponent implements OnInit {
  @ViewChild('mermaidDiv', { static: true }) mermaidDiv!: ElementRef;
  router = inject(Router)
  service = inject(EmployeeServiceService)
  employeeData: any[] = [];
  isDataLoaded: boolean = false; 

  ngOnInit(): void {
    this.loadEmployeeHierarchy();
    (window as any).onA = (nodeName: string) => {
      console.log('Node clicked:', nodeName);
      this.router.navigate(['/emp-dis']);
    };

    mermaid.initialize({
      securityLevel: 'loose',
      theme: 'base',
      flowchart: {
        nodeSpacing: 100,
        rankSpacing: 80,
        padding: 20,
      },
      themeVariables: {
        primaryColor: '#0369a1',
        primaryTextColor: '#ffffff',
        lineColor: '#7f8b9b',
        nodeBorder: '#075985',
      },
      themeCSS: `
        .node rect {
          rx: 8px;
          height: 60px !important;
          transition: all 0.3s ease;
        }
        .node:hover rect {
          fill: #113c60 !important;
        }
        .node:hover text {
          fill: #ffffff !important;
        }
      `,
    });
  }

  loadEmployeeHierarchy() {
    this.service.getHierarchy().subscribe({
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
    if (!this.isDataLoaded || !this.employeeData) return; 
    const graphDefinition = this.generateMermaidGraph(this.employeeData);
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

  private generateMermaidGraph(employee: any): string {
    let graph = 'graph TB;\n';

    const traverse = (emp: any) => {
      const nodeId = `Node${emp.id}`;
      if (emp.lastName != null) {
        const nodeLabel = `${emp.firstName} ${emp.lastName}`;
        graph += `${nodeId}[${nodeLabel}]\n`;

      }
      else {
        const nodeLabel = `${emp.firstName}`;
        graph += `${nodeId}[${nodeLabel}]\n`;

      }
      if (emp.subordinates && emp.subordinates.length > 0) {
        emp.subordinates.forEach((subordinate: any) => {
          const subNodeId = `Node${subordinate.id}`;
          graph += `${nodeId}-->${subNodeId}\n`;
          traverse(subordinate);
        });
      }
      graph += `click ${nodeId} onA\n`;
    };

    traverse(employee);
    return graph;
  }
}