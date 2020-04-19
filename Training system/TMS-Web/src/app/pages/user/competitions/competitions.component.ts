import { Component, OnInit } from '@angular/core';
import { IRecords, IRecordsByPlace } from 'src/app/Interfaces/IRecords';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-competitions',
  templateUrl: './competitions.component.html',
  styleUrls: ['./competitions.component.scss']
})
export class CompetitionsComponent implements OnInit {

  Competitions: IRecords[];
  Inside:IRecordsByPlace[];
  Outside:IRecordsByPlace[];

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this._http.GetCompetitions().subscribe(data=>{
      this.Competitions = data
      this.Outside =this.Competitions[0].records;
      this.Inside =this.Competitions[1].records;
      this.FixTimeToNormal();
    })

  }

  FixTimeToNormal()
  {
    this.Outside.forEach(element => {
      var num = Number(element.time);
      var fullTime = num/60;
      if(fullTime>1)
      {
        var mins = Math.trunc(fullTime)
        var sconds = (num-mins*60).toFixed(2);
        element.time = mins+ ":"+sconds
      }   
        });   
    this.Inside.forEach(element => {
      var num = Number(element.time);
      var fullTime = num/60;
      if(fullTime>1)
      {
        var mins = Math.trunc(fullTime)
        var sconds = (num-mins*60).toFixed(2);
        element.time = mins+ ":"+sconds
      }   
        });  

  }

}
