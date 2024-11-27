import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmloyeeHierarchyDisplayComponent } from './emloyee-hierarchy-display.component';

describe('EmloyeeHierarchyDisplayComponent', () => {
  let component: EmloyeeHierarchyDisplayComponent;
  let fixture: ComponentFixture<EmloyeeHierarchyDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmloyeeHierarchyDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmloyeeHierarchyDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
