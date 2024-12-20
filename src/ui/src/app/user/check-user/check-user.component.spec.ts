import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckUserComponent } from './check-user.component';

describe('CheckUserComponent', () => {
  let component: CheckUserComponent;
  let fixture: ComponentFixture<CheckUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
