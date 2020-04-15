declare var $ :any;
import { Component, OnInit, Input } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { ISet } from '../../../Interfaces/ISet';
import { IPersonalTraining } from '../../../Interfaces/IPersonalTraining';
import { ITrainingTemplate } from '../../../Interfaces/ITrainingTemplate';
import { IAthleteForm } from '../../../Interfaces/IAthleteForm';
import { concat } from 'rxjs';

@Component({
  selector: 'app-personal-training-modal',
  templateUrl: './personal-training-modal.component.html',
  styleUrls: ['./personal-training-modal.component.scss']
})
export class PersonalTrainingModalComponent implements OnInit {

  @Input() personalTraining: IPersonalTraining;
  @Input() toDo :ITrainingTemplate;
  @Input() exist :boolean;
  athleteForm:IAthleteForm = new IAthleteForm()
  trainingsToAdd: ISet[] = [];
  canFillForm = false;
  max =0;


  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
     
  }


  ngOnChanges() {
    if(this.exist){      
      var dateTraining=new Date(this.personalTraining.day)
    if(dateTraining.getTime() <= Date.now() )
    {
      this.canFillForm = true;
      if(this.toDo!=null)
      {
        this.max = this.toDo.repeats*this.toDo.sets.length
      }      
    }
    else{
      this.canFillForm = false;
    }
  }
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
    this._http.UpdatePersonalTrainingByAthlete(this.athleteForm, this.personalTraining.id).subscribe(data=>{   
      console.log(data);      
    })
    $('#myModal').modal("hide");
  }
  
}
