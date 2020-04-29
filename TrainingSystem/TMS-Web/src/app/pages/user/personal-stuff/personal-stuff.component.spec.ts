import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalStuffComponent } from './personal-stuff.component';

describe('PersonalStuffComponent', () => {
  let component: PersonalStuffComponent;
  let fixture: ComponentFixture<PersonalStuffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonalStuffComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalStuffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
