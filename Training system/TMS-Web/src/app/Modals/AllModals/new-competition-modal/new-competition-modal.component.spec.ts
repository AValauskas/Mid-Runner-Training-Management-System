import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewCompetitionModalComponent } from './new-competition-modal.component';

describe('NewCompetitionModalComponent', () => {
  let component: NewCompetitionModalComponent;
  let fixture: ComponentFixture<NewCompetitionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCompetitionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCompetitionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
