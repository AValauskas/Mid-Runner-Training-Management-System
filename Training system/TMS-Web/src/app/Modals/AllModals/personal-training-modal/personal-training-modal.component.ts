import { Component, OnInit, Input } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { IPersonalTraining } from 'src/app/pages/home/IPersonalTraining';

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


  @Input() data: String;
  @Input() trainings: IPersonalTraining[];

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    /* this._http.GetTrainingTemplates().subscribe(data=>{
      this.trainings = data
      console.log(this.trainings)
      
    })*/
    console.log(this.data);
    console.log(this.trainings);
  }

  ngOnChanges() {
    console.log(this.data);
  }

  
  
}
