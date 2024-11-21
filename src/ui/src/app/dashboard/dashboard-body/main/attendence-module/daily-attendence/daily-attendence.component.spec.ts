import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAttendenceComponent } from './daily-attendence.component';

describe('DailyAttendenceComponent', () => {
  let component: DailyAttendenceComponent;
  let fixture: ComponentFixture<DailyAttendenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DailyAttendenceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DailyAttendenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
