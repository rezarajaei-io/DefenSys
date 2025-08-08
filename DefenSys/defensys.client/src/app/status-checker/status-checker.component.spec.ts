import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusCheckerComponent } from './status-checker.component';

describe('StatusCheckerComponent', () => {
  let component: StatusCheckerComponent;
  let fixture: ComponentFixture<StatusCheckerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StatusCheckerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatusCheckerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
