declare var $ :any;
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { ISet } from '../../../Interfaces/ISet';
import { IPersonalTraining } from '../../../Interfaces/IPersonalTraining';
import { ITrainingTemplate } from '../../../Interfaces/ITrainingTemplate';
import { IAthleteForm } from '../../../Interfaces/IAthleteForm';
import { concat } from 'rxjs';
import { ISetToDisplay } from 'src/app/Interfaces/ISetToDisplay';
import { stringify } from 'querystring';

@Component({
  selector: 'app-personal-training-modal',
  templateUrl: './personal-training-modal.component.html',
  styleUrls: ['./personal-training-modal.component.scss']
})
export class PersonalTrainingModalComponent implements OnInit {

  @Input() personalTraining: IPersonalTraining;
  @Input() toDo :ITrainingTemplate;
  @Input() exist :boolean;
  @Output("TurnOnSuccesMessage") parentFun: EventEmitter<any> = new EventEmitter();
  
  athleteForm:IAthleteForm = new IAthleteForm()
  trainingsToAdd: ISet[] = [];
  canFillForm = false;
  max =0;
  setsToDisplay:ISetToDisplay[]=[]

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
     
  }


  ngOnChanges() {
  if(typeof this.personalTraining !== 'undefined'){
    if(this.personalTraining!=null)
    {
      this.trainingsToAdd =this.personalTraining.results
    }
    
  }
    if(this.exist){      
      var dateTraining=new Date(this.personalTraining.day)
    if(dateTraining.getTime() <= Date.now() )
    {
      this.canFillForm = true;
      if(this.toDo!=null)
      {
        this.max = this.toDo.repeats*this.toDo.sets.length;
        if(this.setsToDisplay.length>0) 
        this.setsToDisplay=[];
        this.FormSetsToDisplay();
      }      
    }
    else{
      this.canFillForm = false;
      this.setsToDisplay=[];
    }
  }
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

  OnClick()
  { var nullObject = {distance:null,pace:null, rest:null};
      
    if(this.trainingsToAdd.length<this.max)
    {
      this.trainingsToAdd.push(nullObject);    
    }
  }

  OnDelete(index:number)
  {        
     this.trainingsToAdd.splice(index, 1);
  }
  isEmptyObject(obj) {
    return (obj && (Object.keys(obj).length === 0));
  }

  OnSubmit()
  {
    this.athleteForm.report=this.personalTraining.athleteReport;
    this.athleteForm.results = this.trainingsToAdd;
    console.log(this.athleteForm);
    this._http.UpdatePersonalTrainingResults(this.athleteForm, this.personalTraining.id).subscribe(data=>{   
      console.log(data);      
    })
    this.parentFun.emit();
    $('#myModal').modal("hide");
  }

 

  
  
}
