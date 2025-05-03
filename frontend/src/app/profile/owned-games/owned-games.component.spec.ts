import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnedGamesComponent } from './owned-games.component';

describe('OwnedGamesComponent', () => {
  let component: OwnedGamesComponent;
  let fixture: ComponentFixture<OwnedGamesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OwnedGamesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OwnedGamesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
