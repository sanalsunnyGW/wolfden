import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { Router } from '@angular/router';
import mermaid from 'mermaid';

@Component({
  selector: 'app-my-team',
  standalone: true,
  imports: [],
  templateUrl: './my-team.component.html',
  styleUrl: './my-team.component.scss'
})
export class MyTeamComponent {

  @ViewChild('mermaidDiv', { static: true }) mermaidDiv!: ElementRef;
  router = inject(Router)


  employeeData = {
    id: 1,
    employeeCode: 777,
    firstName: 'Abhi',
    lastName: 'Kumar',
    email: 'abhi.kumar@example.com',
    phoneNumber: '1234567890',
    dateofBirth: new Date('1990-05-15'),
    designationId: 2,
    departmentId: 1,
    managerId: null,
    isActive: true,
    subordinates: [
      {
        id: 2,
        employeeCode: 900,
        firstName: 'Nohan',
        lastName: 'Antony',
        email: 'nohanantony@gmail.com',
        phoneNumber: '892127666',
        dateofBirth: new Date('1995-11-20'),
        designationId: 1,
        departmentId: 1,
        managerId: 1,
        isActive: true,
        subordinates: [
          {
            id: 3,
            employeeCode: 901,
            firstName: 'Ravi',
            lastName: 'Sharma',
            email: 'ravi.sharma@example.com',
            phoneNumber: '9888776655',
            dateofBirth: new Date('1992-08-12'),
            designationId: 3,
            departmentId: 2,
            managerId: 2,
            isActive: true,
            subordinates: [{
              id: 81,
              employeeCode: 901,
              firstName: 'Ravi',
              lastName: 'Sharma',
              email: 'ravi.sharma@example.com',
              phoneNumber: '9888776655',
              dateofBirth: new Date('1992-08-12'),
              designationId: 3,
              departmentId: 2,
              managerId: 2,
              isActive: true,
              subordinates: []
            }],
          },
          {
            id: 4,
            employeeCode: 902,
            firstName: 'aaaa',
            lastName: 'Verma',
            email: 'saurabh.verma@example.com',
            phoneNumber: '9988774455',
            dateofBirth: new Date('1994-01-25'),
            designationId: 4,
            departmentId: 3,
            managerId: 2,
            isActive: true,
            subordinates: [
              {
                id: 7,
                employeeCode: 905,
                firstName: 'Kartik',
                lastName: 'Yadav',
                email: 'kartik.yadav@example.com',
                phoneNumber: '9998776655',
                dateofBirth: new Date('1999-05-20'),
                designationId: 7,
                departmentId: 5,
                managerId: 6,
                isActive: true,
                subordinates: [
                  {
                    id: 75,
                    employeeCode: 905,
                    firstName: 'Kartik',
                    lastName: 'Yadav',
                    email: 'kartik.yadav@example.com',
                    phoneNumber: '9998776655',
                    dateofBirth: new Date('1999-05-20'),
                    designationId: 7,
                    departmentId: 5,
                    managerId: 6,
                    isActive: true,
                    subordinates: [{}]
                  }
                ]              }
            ]
          },
          {
            id: 5,
            employeeCode: 903,
            firstName: 'Shubham',
            lastName: 'Singh',
            email: 'shubham.singh@example.com',
            phoneNumber: '9008779988',
            dateofBirth: new Date('1996-07-18'),
            designationId: 5,
            departmentId: 3,
            managerId: 2,
            isActive: true,
            subordinates: []
          }
        ]
      }
    ]
  };

  ngOnInit(): void {
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


  ngAfterViewInit(): void {
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
            svgElement.style.display = 'block';  // Ensure block-level display
            svgElement.style.margin = '0 auto';  // Centering the SVG
          }
        }, 0);
      })
      .catch(error => {
        console.error('Mermaid render error:', error);
      });
  }


  private generateMermaidGraph(employee: any): string {
    let graph = 'graph TB;\n';
    const traverse = (emp: any) => {
    const nodeId = `Node${emp.id}`;
    const nodeLabel = `${emp.firstName} ${emp.lastName}`;
    graph += `${nodeId}[${nodeLabel}]\n`;
    if (emp.subordinates && emp.subordinates.length > 0)
     {
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


