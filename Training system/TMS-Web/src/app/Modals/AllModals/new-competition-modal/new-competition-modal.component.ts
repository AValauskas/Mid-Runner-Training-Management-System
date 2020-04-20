import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ICompetition } from 'src/app/Interfaces/ICompetition';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { ITime } from 'src/app/Interfaces/ITime';
declare var $ :any;

@Component({
  selector: 'app-new-competition-modal',
  templateUrl: './new-competition-modal.component.html',
  styleUrls: ['./new-competition-modal.component.scss']
})
export class NewCompetitionModalComponent implements OnInit {

  constructor(private _http: ProcessService,public _router:Router) { }

  competition:ICompetition=<ICompetition>{};
  time:ITime=<ITime>{};
  @Output("FillForms") parentFun: EventEmitter<any> = new EventEmitter();
  
  ngOnInit(): void {

  }


  OnSubmit()
  {
    this.competition.time=this.time.min*60+this.time.sec;
  console.log(this.competition);
  this.competition.distance= Number(this.competition.distance);
  this._http.AddNewCompetition(this.competition).subscribe(data=>{
    this.parentFun.emit();
    $('#myModal').modal("hide");
    this.competition=<ICompetition>{};
    this.time=<ITime>{};
    });
  }



}
