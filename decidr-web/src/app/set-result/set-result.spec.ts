import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetResult } from './set-result';

describe('SetResult', () => {
  let component: SetResult;
  let fixture: ComponentFixture<SetResult>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetResult]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetResult);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
