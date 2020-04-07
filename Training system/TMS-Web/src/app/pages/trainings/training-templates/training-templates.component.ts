import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ProcessService } from 'src/app/services/process/process.service';

interface ITrainingTemplate{
  description:string;
  trainingtype:string;
  destinition:number;    
  repeats:number;  
  personal:boolean;
  sets:any;  
}


@Component({
  selector: 'app-training-templates',
  templateUrl: './training-templates.component.html',
  styleUrls: ['./training-templates.component.scss']
})
export class TrainingTemplatesComponent implements OnInit {
  error:string;
  Role: string;
  destinition:string;
  trainings: Object;
  trainingsToAdd: { distance: number, pace: number, rest: number }[] = [];
  train: { distance: number, pace: number, rest: number };
  training:ITrainingTemplate = <any>{};
  
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this.Role=localStorage.getItem('role')
  }

  ngAfterViewInit(): void
  {
    this.Role=localStorage.getItem('role')
    this._http.GetTrainingTemplates().subscribe(data=>{
      this.trainings = data
      console.log(this.trainings)
    })
  }
  OnSubmit()
  {
    this.FillData();

    this._http.PostTrainingTemplates(this.training).subscribe(  )  
    if(localStorage.getItem("error")==null)
    {
    console.log("good")
      
    }
    else{
      this.error=localStorage.getItem("error");
      localStorage.removeItem("error");
    }
  }
 

  FillData(){
    this.training.sets=this.trainingsToAdd;
    if(localStorage.getItem("role")=='admin') 
    {
      this.training.personal=false;
    }
    this.training.destinition=+this.destinition;
    console.log(this.training);

  }


  OnClick()
  { var nullObject = {distance:null,pace:null, rest:null};
    console.log( this.trainingsToAdd);
    this.trainingsToAdd[this.trainingsToAdd.length]=nullObject;
    //this.trainingsToAdd.push(nullObject);    
  }

  OnDelete(index:number)
  {    
    this.trainingsToAdd.splice(index, 1);
  }
}
