import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingTemplatesComponent } from './training-templates.component';

describe('TrainingTemplatesComponent', () => {
  let component: TrainingTemplatesComponent;
  let fixture: ComponentFixture<TrainingTemplatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingTemplatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
