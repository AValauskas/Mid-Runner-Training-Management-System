import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPersonalTraining } from 'src/app/Interfaces/IPersonalTraining';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { ITrainingTemplate } from 'src/app/Interfaces/ITrainingTemplate';
import { ICoachAssignedTraining } from 'src/app/Interfaces/ICoachAssignedTraining';
import { ISet } from 'src/app/Interfaces/ISet';
import { IAthleteForm } from 'src/app/Interfaces/IAthleteForm';

@Component({
  selector: 'app-athlete-list',
  templateUrl: './athlete-list.component.html',
  styleUrls: ['./athlete-list.component.scss']
})
export class AthleteListComponent implements OnInit {
  @Input() dateClicked :string;
  @Output("renewTrainings") parentFun: EventEmitter<any> = new EventEmitter();

  AssignedTrainings:ICoachAssignedTraining[];
  AthleteTraininActive = false;
  ListActive = true;
  alreadyPast = false;
  max=0;
  exist = false;
  athlete:string;

  toDo:ITrainingTemplate;
  personalTraining: IPersonalTraining;
  trainingsToAdd: ISet[] = [];
  athleteForm:IAthleteForm = new IAthleteForm()
  
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
  }


  ngOnChanges() {
     
    var dateTraining=new Date(this.dateClicked);
    this.GetData();

    if(dateTraining.getTime() <= Date.now() )    
    {
      this.alreadyPast=true;
    }
    else{
      this.alreadyPast=false;
    }
    this.AthleteTraininActive = false;
    this.ListActive = true;
    this.exist = false;
    this.trainingsToAdd = [];
  }

  GetData(){
    this._http.GetAllCoachAssignedTrainingsByDate(this.dateClicked).subscribe(data=>{   
     this.AssignedTrainings= data;   
     console.log(data)  
      });  
  }


  TurnTrain(trainingTemplateid, personalTrainingId, athleteName)
  {
    this.athlete = athleteName;
    console.log( personalTrainingId);
    this.AthleteTraininActive = true;
    this.ListActive= false;
    this._http.GetTrainingTemplateById(trainingTemplateid).subscribe(data=>{   
      this.toDo=data;
      console.log( this.toDo);
      if(typeof this.toDo !== 'undefined'){
        if(this.toDo.sets.length==0)
        {
          this.max =1;
        }
        else{
        this.max = this.toDo.repeats*this.toDo.sets.length
        }
      }
    });
    this._http.GetPersonalTrainingById(personalTrainingId).subscribe(data=>{   
      this.personalTraining=data;
      this.exist = true;
      this.trainingsToAdd =this.personalTraining.results
      console.log(this.trainingsToAdd)
      console.log( this.personalTraining);
    }); 
  }


  DeletePersonalTraining(id)
  {
    this._http.DeletePersonalTraining(id).subscribe(data=>{   
       this._http.GetAllCoachAssignedTrainingsByDate(this.dateClicked).subscribe(data2=>{   
        this.AssignedTrainings= data2;   
        console.log(data2)  
         }); 
         this.parentFun.emit();
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
   this.ListActive=true;
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
