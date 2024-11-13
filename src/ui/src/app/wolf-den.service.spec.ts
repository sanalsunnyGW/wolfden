import { TestBed } from '@angular/core/testing';

import { WolfDenService } from './wolf-den.service';

describe('WolfDenService', () => {
  let service: WolfDenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WolfDenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
