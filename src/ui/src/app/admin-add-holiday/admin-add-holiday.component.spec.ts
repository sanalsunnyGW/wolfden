import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddHolidayComponent } from './admin-add-holiday.component';

describe('AdminAddHolidayComponent', () => {
  let component: AdminAddHolidayComponent;
  let fixture: ComponentFixture<AdminAddHolidayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminAddHolidayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminAddHolidayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
