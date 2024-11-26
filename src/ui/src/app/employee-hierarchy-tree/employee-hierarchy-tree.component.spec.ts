import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeHierarchyTreeComponent } from './employee-hierarchy-tree.component';

describe('EmployeeHierarchyTreeComponent', () => {
  let component: EmployeeHierarchyTreeComponent;
  let fixture: ComponentFixture<EmployeeHierarchyTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployeeHierarchyTreeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeHierarchyTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
