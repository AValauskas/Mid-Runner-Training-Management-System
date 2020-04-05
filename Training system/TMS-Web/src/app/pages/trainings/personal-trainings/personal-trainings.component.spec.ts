import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalTrainingsComponent } from './personal-trainings.component';

describe('PersonalTrainingsComponent', () => {
  let component: PersonalTrainingsComponent;
  let fixture: ComponentFixture<PersonalTrainingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonalTrainingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalTrainingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
