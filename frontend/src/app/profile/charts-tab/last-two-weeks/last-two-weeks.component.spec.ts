import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LastTwoWeeksComponent } from './last-two-weeks.component';

describe('LastTwoWeeksComponent', () => {
  let component: LastTwoWeeksComponent;
  let fixture: ComponentFixture<LastTwoWeeksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LastTwoWeeksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LastTwoWeeksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
