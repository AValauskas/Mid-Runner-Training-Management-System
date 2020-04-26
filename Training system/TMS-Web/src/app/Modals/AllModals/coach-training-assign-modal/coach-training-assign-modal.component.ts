declare var $ :any;
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { ITrainingTemplate } from 'src/app/Interfaces/ITrainingTemplate';
import { IAthleteForm } from 'src/app/Interfaces/IAthleteForm';
import { IAthlete } from 'src/app/Interfaces/IAthlete';
import { IPersonalTraining } from 'src/app/Interfaces/IPersonalTraining';
import { IPersonalTrainingFew } from 'src/app/Interfaces/IPersonalTrainingFew';

@Component({
  selector: 'app-coach-training-assign-modal',
  templateUrl: './coach-training-assign-modal.component.html',
  styleUrls: ['./coach-training-assign-modal.component.scss']
})
export class CoachTrainingAssignModalComponent implements OnInit {

  @Input() dateClicked :string;  
  @Output("renewTrainings") parentFun: EventEmitter<any> = new EventEmitter();
  canFillForm:boolean;

  PersonalTrainin:IPersonalTrainingFew= new IPersonalTrainingFew();
  Trainings:ITrainingTemplate[];
  Athletes:IAthlete[];


   IsAnyAthleteFree =false;


  oldDate:string;
  Description="";
  selectedType = "";
  selectedTraining:ITrainingTemplate;
  Place="";
  trainingsCount=0;
  selectAthletes: IAthlete[];

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
   
  }

  ngOnChanges() {
     
    var dateTraining=new Date(this.dateClicked);
    console.log( this.canFillForm);
    console.log (dateTraining.getTime());
    console.log (Date.now());
    if(dateTraining.getTime() >= Date.now() )
    {
      this.canFillForm = true;
      this.GetData();
      if(this.oldDate!=this.dateClicked)
      {
        this.Place="";
        this.trainingsCount=0;
        this.Description="";
        this.selectedType = "";
        this.selectedType="";
        this.Athletes=[];
        this.Trainings=[];
        this.oldDate=this.dateClicked;
      }      
    }
    else{
      this.canFillForm = false;
    }  
  }


  GetData(){
    this._http.GetAthletesWhichStillFree(this.dateClicked).subscribe(data=>{   
      this.Athletes=data
       if(this.Athletes.length!=0)
       {
         this.IsAnyAthleteFree= true;
       } 
      });  
  }

  onChangeType(newValue) {
    console.log(newValue);
    this.selectedType = newValue;
    this._http.GetTrainingsByType(newValue).subscribe(data=>{   
      this.Trainings=data;
       this.trainingsCount=this.Trainings.length;
      });    
  }

  onChangeTraining(newValue)
  {
    this.selectedTraining=newValue;
    console.log( this.selectedTraining);
  }

  OnChooseAthletes(athletes){


  }
  OnSubmit(){
    this.PersonalTrainin.coachId=localStorage.getItem('user');;
    this.PersonalTrainin.athleteIds=this.selectAthletes.map(({ idPerson }) => idPerson );
    this.PersonalTrainin.description = this.Description;
    this.PersonalTrainin.place = this.Place;
    this.PersonalTrainin.day=new Date(this.dateClicked);
    this.PersonalTrainin.trainTemplateId = this.selectedTraining.id;
    console.log(this.PersonalTrainin);

    this._http.InsertPersonalTraining( this.PersonalTrainin).subscribe(data=>{   
      $('#myModal').modal("hide");
      this.parentFun.emit("You have succesfully assigned trainings");
      });    
  }
}
