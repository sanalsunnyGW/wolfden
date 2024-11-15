import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewLeaveTypeComponent } from './add-new-leave-type.component';

describe('AddNewLeaveTypeComponent', () => {
  let component: AddNewLeaveTypeComponent;
  let fixture: ComponentFixture<AddNewLeaveTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddNewLeaveTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewLeaveTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
