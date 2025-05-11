import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TellMeWhatToPlayComponent } from './tell-me-what-to-play.component';

describe('TellMeWhatToPlayComponent', () => {
  let component: TellMeWhatToPlayComponent;
  let fixture: ComponentFixture<TellMeWhatToPlayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TellMeWhatToPlayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TellMeWhatToPlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
