import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateLeaveBalanceComponent } from './update-leave-balance.component';

describe('UpdateLeaveBalanceComponent', () => {
  let component: UpdateLeaveBalanceComponent;
  let fixture: ComponentFixture<UpdateLeaveBalanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateLeaveBalanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateLeaveBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
