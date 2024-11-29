import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddLeaveByAdminForEmployeesComponent } from './add-leave-by-admin-for-employees.component';

describe('AddLeaveByAdminForEmployeesComponent', () => {
  let component: AddLeaveByAdminForEmployeesComponent;
  let fixture: ComponentFixture<AddLeaveByAdminForEmployeesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddLeaveByAdminForEmployeesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddLeaveByAdminForEmployeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
