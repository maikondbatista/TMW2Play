import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HumiliateMyLibraryComponent } from './humiliate-my-library.component';

describe('HumiliateMyLibraryComponent', () => {
  let component: HumiliateMyLibraryComponent;
  let fixture: ComponentFixture<HumiliateMyLibraryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HumiliateMyLibraryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HumiliateMyLibraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
