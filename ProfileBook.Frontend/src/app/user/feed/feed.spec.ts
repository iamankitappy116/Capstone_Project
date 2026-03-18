import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Feed } from './feed';

describe('Feed', () => {
  let component: Feed;
  let fixture: ComponentFixture<Feed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Feed],
    }).compileComponents();

    fixture = TestBed.createComponent(Feed);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
