import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProcessService } from 'src/app/services/process/process.service';

@Component({
  selector: 'app-training-templates',
  templateUrl: './training-templates.component.html',
  styleUrls: ['./training-templates.component.scss']
})
export class TrainingTemplatesComponent implements OnInit {


  trainings: Object;
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void
  {
    this._http.getTrainingTemplates().subscribe(data=>{
      this.trainings = data
      console.log(this.trainings)
    })
  }
}
