import { Component, OnInit, Input } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';


/*interface IPersonalTraining{
  description:string;
  trainingtype:string;
  destinition:number;    
  repeats:number;  
  personal:boolean;
  sets:any;  
}*/

@Component({
  selector: 'app-personal-training-modal',
  templateUrl: './personal-training-modal.component.html',
  styleUrls: ['./personal-training-modal.component.scss']
})
export class PersonalTrainingModalComponent implements OnInit {



  @Input() personalTraining: IPersonalTraining;
  @Input() toDo :ITrainingTemplate
  trainingsToAdd: ISet[] = [];
  newToAdd:ISet;


  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
 

    
  }

  ngOnChanges() {
    console.log(this.personalTraining);
    console.log(this.toDo);
  }

  OnClick()
  { var nullObject = {distance:null,pace:null, rest:null};
    
   this.trainingsToAdd[this.trainingsToAdd.length]=nullObject;
   // this.trainingsToAdd.push(nullObject);    
    console.log( this.trainingsToAdd);
  }

  OnDelete(index:number)
  {        
     this.trainingsToAdd.splice(index, 1);
     console.log(this.trainingsToAdd);
  }

  trackBy(index) { return index }
}
