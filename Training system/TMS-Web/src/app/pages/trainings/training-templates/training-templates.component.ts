import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ProcessService } from 'src/app/services/process/process.service';
import { ITrainingTemplate } from '../../../Interfaces/ITrainingTemplate';
declare var $ :any;


@Component({
  selector: 'app-training-templates',
  templateUrl: './training-templates.component.html',
  styleUrls: ['./training-templates.component.scss']
})
export class TrainingTemplatesComponent implements OnInit {
  error:string;
  Role: string;
  trainings: ITrainingTemplate[]=[];
  trainingsToAdd: { distance: number, pace: number, rest: number }[] = [];
  Choosen: { distance: number, pace: number, rest: number }[] = [];
  train: { distance: number, pace: number, rest: number };
  training:ITrainingTemplate;
  belongsToUser= false;
  deleteModalActive =false;
  updateModalActive =false;
  insertModalActive =false;
  descriptionClicked = false;
  setsClicked = false;
  manageClicked = false;



  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this.Role=localStorage.getItem('role')
  }

  ngAfterViewInit(): void
  {
    this.Role=localStorage.getItem('role')
    if( this.Role=="Athlete")
    {
      this._http.GetTrainingTemplates().subscribe(data=>{
        this.trainings = data
        console.log(this.trainings)
      });
    }else{
    this._http.GetTrainingTemplatesIncludedPersonal().subscribe(data=>{
      this.trainings = data
      console.log(this.trainings)
    })
  }
  }
  OnSubmit()
  {
    this.FillData();
     if(this.insertModalActive){
      this.InsertAction();
     } else{
       this.UpdateAction();
     }
    
    $('#myModal').modal("hide");
  }

  UpdateAction()
  {
    this._http.UpdateTrainingTemplate(this.training).subscribe(data=>{
      this._http.GetTrainingTemplatesIncludedPersonal().subscribe(data=>{
        this.trainings = data
        console.log(this.trainings)
      })
    });
  }

  InsertAction()
  {
    this._http.PostTrainingTemplates(this.training).subscribe(data=>{
      if(localStorage.getItem("error")==null)
      {
      console.log("good")
        
      }
      else{
        this.error=localStorage.getItem("error");
        localStorage.removeItem("error");
      }
      this._http.GetTrainingTemplatesIncludedPersonal().subscribe(data=>{
        this.trainings = data
        console.log(this.trainings)
      })
    });
  }
 

  FillData(){
    this.training.sets=this.trainingsToAdd;
   this.training.destinition=Number(this.training.destinition);
    console.log(this.training);

  }


  OnClick()
  { 
    var nullObject = {distance:null,pace:null, rest:null};
    this.trainingsToAdd[this.trainingsToAdd.length]=nullObject;
    console.log(this.training)
    //this.trainingsToAdd.push(nullObject);    
  }

  OnDelete(index:number)
  {    
    this.trainingsToAdd.splice(index, 1);
  }


  ChooseTraining(trainingTemplateId)
  {
    this.trainingsToAdd=[];
      this.insertModalActive= false;
      this.training = this.trainings.filter(x=>x.id==trainingTemplateId)[0];
      if(this.training.owner == localStorage.getItem('user'))
      {this.belongsToUser= true;}
      else{this.belongsToUser= false;}
      console.log(this.belongsToUser);
  }

  UpdateTemplate()
  {
    
  }

  OpenModal(modal)
  {
    console.log(modal);
    
    if(modal=="insert")
    {
    this.training = <any>{};
    this.insertModalActive= true;
    this.updateModalActive= false;
    this.deleteModalActive = false;
    }
    else if(modal=="update")
    {    
      console.log(this.training);
    this.insertModalActive= false;
    this.updateModalActive= true;
    this.deleteModalActive = false;
    this.training.sets.forEach(val =>  this.trainingsToAdd.push(Object.assign({}, val)));
    }
    else{
      this.insertModalActive= false;
      this.updateModalActive= false;
      this.deleteModalActive = true;
    }
    $('#myModal').modal("show");

  }
  DeleteTemplate()
  {
    this._http.DeleteTrainingTemplate(this.training.id).subscribe(data=>{
      console.log(data)
      this._http.GetTrainingTemplatesIncludedPersonal().subscribe(data=>{
        this.trainings = data
        console.log(this.trainings)
      })
    })
   
    $('#myModal').modal("hide");

  }

  ChangeDescriptionSize()
  {
    console.log("clicked");
    if( this.descriptionClicked)
    {
      this.descriptionClicked= false;
    }
    else{
      this.descriptionClicked= true;
    }


  }

  changeTimes()
  {
    if( this.setsClicked)
    {
      this.setsClicked= false;
    }
    else{
      this.setsClicked= true;
    }
  }

  
  changeManage()
  {
    if( this.manageClicked)
    {
      this.manageClicked= false;
    }
    else{
      this.manageClicked= true;
    }
  }
}

