import { Component, OnInit } from '@angular/core';
import { Router, RoutesRecognized, NavigationStart } from '@angular/router';
import { filter, pairwise } from 'rxjs/operators';

@Component({
  selector: 'app-personal-stuff',
  templateUrl: './personal-stuff.component.html',
  styleUrls: ['./personal-stuff.component.scss']
})
export class PersonalStuffComponent implements OnInit {
  Chosen="Records";
  RecordsActive = true;
  CompetitionsActive = false;
  TrainingsActive = false;
  isFriend = false;
  friendName:string;

  constructor(public _router:Router) { 
    
  }

  ngOnInit(): void {
    if(localStorage.getItem('role')==null)
    {
      this._router.navigateByUrl('/login');
    }   

    if(localStorage.getItem("friend")!=null)
    {
      if(localStorage.getItem("visit")!=null )  
      {
        localStorage.removeItem("friend");
        localStorage.removeItem("friendId");   
        localStorage.removeItem("visit");   
      }
      else{
        this.friendName=localStorage.getItem("friend")   
        this.isFriend = true;      
        localStorage.setItem("visit","true")
      }
     
    }
    else{
      this.isFriend= false;
    }
  }
  TurnRecords(){
    this.Chosen="Records"
    this.RecordsActive = true;
    this.CompetitionsActive = false;
    this.TrainingsActive = false;
    console.log( this.Chosen);
  }
  TurnCompetitions()
  {
    this.Chosen="Competitions"
    this.RecordsActive = false;
    this.CompetitionsActive = true;
    this.TrainingsActive = false;
    console.log( this.Chosen);
  }
  TurnTrainings()
  {
    this.Chosen="Trainings"
    this.RecordsActive = false;
    this.CompetitionsActive = false;
    this.TrainingsActive = true;
    console.log( this.Chosen);
  }

}
