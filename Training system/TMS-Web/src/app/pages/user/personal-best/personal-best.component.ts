import { Component, OnInit } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import { IRecords, IRecordsByPlace } from 'src/app/Interfaces/IRecords';

@Component({
  selector: 'app-personal-best',
  templateUrl: './personal-best.component.html',
  styleUrls: ['./personal-best.component.scss']
})
export class PersonalBestComponent implements OnInit {

  
  Records: IRecords[];
  Inside:IRecordsByPlace[];
  Outside:IRecordsByPlace[];

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this._http.GetRecords().subscribe(data=>{
      this.Records = data
      this.Outside =this.Records[0].records;
      this.Inside =this.Records[1].records;
      this.FixTimeToNormal();
      console.log(this.Records);
      console.log(this.Outside);
      console.log(this.Inside);
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

  ngAfterViewInit(): void
  {    
    
  }
}
