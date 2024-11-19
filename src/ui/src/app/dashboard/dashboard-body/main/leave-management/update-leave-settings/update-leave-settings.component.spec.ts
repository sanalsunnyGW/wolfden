import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateLeaveSettingsComponent } from './update-leave-settings.component';

describe('UpdateLeaveSettingsComponent', () => {
  let component: UpdateLeaveSettingsComponent;
  let fixture: ComponentFixture<UpdateLeaveSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateLeaveSettingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateLeaveSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
