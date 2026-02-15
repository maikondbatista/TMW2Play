import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TellMeWhatIsUpcomingComponent } from './tell-me-what-is-upcoming.component';

describe('TellMeWhatIsUpcomingComponent', () => {
  let component: TellMeWhatIsUpcomingComponent;
  let fixture: ComponentFixture<TellMeWhatIsUpcomingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TellMeWhatIsUpcomingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TellMeWhatIsUpcomingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
