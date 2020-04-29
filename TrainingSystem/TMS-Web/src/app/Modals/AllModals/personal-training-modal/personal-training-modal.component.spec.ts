import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalTrainingModalComponent } from './personal-training-modal.component';

describe('PersonalTrainingModalComponent', () => {
  let component: PersonalTrainingModalComponent;
  let fixture: ComponentFixture<PersonalTrainingModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonalTrainingModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalTrainingModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
