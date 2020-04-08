import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-personal-stuff',
  templateUrl: './personal-stuff.component.html',
  styleUrls: ['./personal-stuff.component.scss']
})
export class PersonalStuffComponent implements OnInit {
  Chosen="Records"
  RecordsActive = true;
  CompetitionsActive = false;
  TrainingsActive = false;
  constructor() { }

  ngOnInit(): void {
  }
  TurnRecords(){
    this.Chosen="Records"
    this.RecordsActive = true;
    this.CompetitionsActive = false;
    this.TrainingsActive = false;
  }
  TurnCompetitions()
  {
    this.Chosen="Competitions"
    this.RecordsActive = false;
    this.CompetitionsActive = true;
    this.TrainingsActive = false;
  }
  TurnTrainings()
  {
    this.Chosen="Competitions"
    this.RecordsActive = false;
    this.CompetitionsActive = false;
    this.TrainingsActive = true;
  }

}
