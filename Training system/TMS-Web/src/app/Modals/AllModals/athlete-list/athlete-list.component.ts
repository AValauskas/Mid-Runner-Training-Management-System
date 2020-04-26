import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPersonalTraining } from 'src/app/Interfaces/IPersonalTraining';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { ITrainingTemplate } from 'src/app/Interfaces/ITrainingTemplate';
import { ICoachAssignedTraining } from 'src/app/Interfaces/ICoachAssignedTraining';
import { ISet } from 'src/app/Interfaces/ISet';
import { IAthleteForm } from 'src/app/Interfaces/IAthleteForm';
import { ISetToDisplay } from 'src/app/Interfaces/ISetToDisplay';
declare var $ :any;

@Component({
  selector: 'app-athlete-list',
  templateUrl: './athlete-list.component.html',
  styleUrls: ['./athlete-list.component.scss']
})
export class AthleteListComponent implements OnInit {
  @Input() dateClicked :string;
  @Input() AssignedTraining :ICoachAssignedTraining;
  @Output("SuccesfullyMadeByCoachMessage") parentFun: EventEmitter<any> = new EventEmitter();

  alreadyPast = false;
  max=0;
  athlete:string;

  toDo:ITrainingTemplate;
  personalTraining: IPersonalTraining;
  trainingsToAdd: ISet[] = [];
  athleteForm:IAthleteForm = new IAthleteForm();
  setsToDisplay:ISetToDisplay[]=[];
  
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    console.log(this.AssignedTraining)
    console.log(this.AssignedTraining.personalTrainingId)
    console.log(this.AssignedTraining.trainingTemplateId)
    console.log(this.AssignedTraining.athlete)
    
  }


  ngOnChanges() {
    this.toDo=<any>{};
    this.personalTraining=<any>{};
    this.trainingsToAdd=[];
    this.athleteForm=<any>{};
    this.setsToDisplay=[];
    var dateTraining=new Date(this.dateClicked);

    if(dateTraining.getTime() <= Date.now() )    
    {
      this.alreadyPast=true;
    }
    else{
      this.alreadyPast=false;
    }
    this.trainingsToAdd = [];
    this.DisplayTrain()
  }

  FormSetsToDisplay()
  {
      this.toDo.sets.forEach(element => {
        
        let set =  {} as ISetToDisplay;
        set.distane =element.distance;

        var paceMinNumb =Math.trunc(element.pace/60) ;
        var restMinNumb =Math.trunc(element.rest/60);

        if(paceMinNumb>=1)
        {
         var paceMin = paceMinNumb.toString()+"min "
        }
        else
        {
          paceMin="";
        }
        if(restMinNumb>=1)
        {
         var restMin = restMinNumb.toString()+"min "
        }
        else
        {
          restMin="";
        }

        set.rest=restMin+(element.rest - restMinNumb*60).toString()+"s"
        set.runTime=paceMin+(element.pace - paceMinNumb*60).toString()+"s"
        
        this.setsToDisplay.push(set);
      });
console.log(this.setsToDisplay);
  }


  DisplayTrain()
  {
    this.athlete = this.AssignedTraining.athlete;
    console.log( this.dateClicked);
    console.log( this.AssignedTraining.personalTrainingId);
    this._http.GetTrainingTemplateById( this.AssignedTraining.trainingTemplateId).subscribe(data=>{   
      this.toDo=data;
      console.log( this.toDo);
      if(typeof this.toDo !== 'undefined'){
        if(this.toDo.sets.length==0)
        {
          this.max =1;
        }
        else{
        this.max = this.toDo.repeats*this.toDo.sets.length
        this.setsToDisplay=[];
        this.FormSetsToDisplay();
        }
      }
    });
    this._http.GetPersonalTrainingById( this.AssignedTraining.personalTrainingId).subscribe(data=>{   
      this.personalTraining=data;
      this.trainingsToAdd =this.personalTraining.results
      console.log(this.trainingsToAdd)
      console.log( this.personalTraining);
    }); 
  }

  //----------------------------athleto redagavimo formos metodai-------------------

  OnSubmit()
  {
    this.athleteForm.results = this.trainingsToAdd;

    console.log(this.athleteForm);
    this._http.UpdatePersonalTrainingResults(this.athleteForm, this.personalTraining.id).subscribe(data=>{   
      console.log(data);           
    })
    $('#myModal').modal("hide");
      this.parentFun.emit("You have succesfully updated your athlete training");
  }


  OnClick()
  { 
    console.log("pasuapude")
    var nullObject = {distance:null,pace:null, rest:null};
      
    if(this.trainingsToAdd.length<this.max)
    {
      this.trainingsToAdd.push(nullObject);    
    }
    console.log( this.trainingsToAdd);
  }

  OnDelete(index:number)
  {        
     this.trainingsToAdd.splice(index, 1);
  }
}
