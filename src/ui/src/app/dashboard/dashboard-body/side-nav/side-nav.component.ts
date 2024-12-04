
import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WolfDenService } from '../../../service/wolf-den.service';
import { EmployeeService } from '../../../service/employee.service';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss',
  animations: [
    trigger('expandCollapse', [
      state('collapsed', style({
        height: '0',
        overflow: 'hidden'
      })),
      state('expanded', style({
        height: '*'
      })),
      transition('collapsed <=> expanded', [
        animate('200ms ease-in-out')
      ])
    ])
  ]
})
export class SideNavComponent {
  userService = inject(WolfDenService);
  employeeService = inject(EmployeeService);

  expandedSections: { [key: string]: boolean } = {
    leave: true,
    attendance: true
  };

  toggleSection(section: string): void {
    this.expandedSections[section] = !this.expandedSections[section];
  }

  isSectionExpanded(section: string): boolean {
    return this.expandedSections[section];
  }
}
