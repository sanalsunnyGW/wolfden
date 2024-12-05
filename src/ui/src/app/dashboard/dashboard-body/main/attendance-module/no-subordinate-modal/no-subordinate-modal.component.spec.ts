import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoSubordinateModalComponent } from './no-subordinate-modal.component';

describe('NoSubordinateModalComponent', () => {
  let component: NoSubordinateModalComponent;
  let fixture: ComponentFixture<NoSubordinateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoSubordinateModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NoSubordinateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
