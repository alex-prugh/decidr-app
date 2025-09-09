import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetDetailComponent } from './set-detail';

describe('SetDetail', () => {
  let component: SetDetailComponent;
  let fixture: ComponentFixture<SetDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
