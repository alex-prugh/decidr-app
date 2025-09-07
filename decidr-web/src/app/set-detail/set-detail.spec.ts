import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetDetail } from './set-detail';

describe('SetDetail', () => {
  let component: SetDetail;
  let fixture: ComponentFixture<SetDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
