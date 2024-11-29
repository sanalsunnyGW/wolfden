import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubordinateLeaveRequestComponent } from './subordinate-leave-request.component';

describe('SubordinateLeaveRequestComponent', () => {
  let component: SubordinateLeaveRequestComponent;
  let fixture: ComponentFixture<SubordinateLeaveRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubordinateLeaveRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubordinateLeaveRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
