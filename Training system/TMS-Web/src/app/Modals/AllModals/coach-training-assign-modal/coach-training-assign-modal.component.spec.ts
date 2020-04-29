import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoachTrainingAssignModalComponent } from './coach-training-assign-modal.component';

describe('CoachTrainingAssignModalComponent', () => {
  let component: CoachTrainingAssignModalComponent;
  let fixture: ComponentFixture<CoachTrainingAssignModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoachTrainingAssignModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoachTrainingAssignModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
