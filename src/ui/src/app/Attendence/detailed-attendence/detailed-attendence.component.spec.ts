import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailedAttendenceComponent } from './detailed-attendence.component';

describe('DetailedAttendenceComponent', () => {
  let component: DetailedAttendenceComponent;
  let fixture: ComponentFixture<DetailedAttendenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailedAttendenceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailedAttendenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
