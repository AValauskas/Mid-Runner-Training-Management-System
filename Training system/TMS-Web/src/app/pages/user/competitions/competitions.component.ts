import { Component, OnInit } from '@angular/core';
import { IRecords, IRecordsByPlace } from 'src/app/Interfaces/IRecords';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
declare var $ :any;


@Component({
  selector: 'app-competitions',
  templateUrl: './competitions.component.html',
  styleUrls: ['./competitions.component.scss']
})
export class CompetitionsComponent implements OnInit {

  Competitions: IRecords[];
  Inside:IRecordsByPlace[];
  Outside:IRecordsByPlace[];
  Role:string;
  isFriend = false;
  friend:string

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    if(localStorage.getItem("friend")!=null)
    {
        this.isFriend = true;
        this.friend=localStorage.getItem("friendId");
        console.log(this.friend);
        this.FillFormsOther();  
    }
    else{
      this.isFriend= false;
      this.FillFormsOwn();   
    }
    this.Role=localStorage.getItem('role')
   
  }


  FillFormsOwn()
  {
    this._http.GetCompetitions().subscribe(data=>{
      this.Competitions = data
      console.log(this.Competitions)
      if( this.Competitions.length>0)
      {
        if(this.Competitions.length==1)
        {
            if(this.Competitions[0].records[0].place=="Outside")
            {
              this.Outside =this.Competitions[0].records;
            }
            else{
              this.Inside =this.Competitions[0].records;
            }
        }
       else {
        this.Outside =this.Competitions[0].records;
        this.Inside =this.Competitions[1].records;
       
        }
        this.FixTimeToNormal();
      }
    })

  }


  
  FillFormsOther()
  {
    this._http.GetOtherCompetitions(this.friend).subscribe(data=>{
      this.Competitions = data
      console.log(this.Competitions)
      if( this.Competitions.length>0)
      {
        if(this.Competitions.length==1)
        {
            if(this.Competitions[0].records[0].place=="Outside")
            {
              this.Outside =this.Competitions[0].records;
            }
            else{
              this.Inside =this.Competitions[0].records;
            }
        }
       else {
        this.Outside =this.Competitions[0].records;
        this.Inside =this.Competitions[1].records;
       
        }
        this.FixTimeToNormal();
      }
    })

  }

  FixTimeToNormal()
  {
    if(this.Outside!=null){
    if( this.Outside.length>0){
      this.Outside.forEach(element => {
        var num = Number(element.time);
        var fullTime = num/60;
        if(fullTime>1)
        {
          var minZero="";
          var secZero="";
          var mins = Math.trunc(fullTime)
          var seconds = (num-mins*60)
          var sconds = (num-mins*60).toFixed(2);
          if(mins<10) 
          {
            minZero="0";
          } 
          if(seconds<10) 
          {
            secZero="0";
          } 
          element.time = minZero+mins+ ":"+secZero+sconds;
        }   
          });   
    }
    }
    if(this.Inside!=null){
   if(this.Inside.length>0)
   {
    this.Inside.forEach(element => {
      var num = Number(element.time);
      var fullTime = num/60;
      if(fullTime>1)
      {
        var minZero="";
        var secZero="";
        var mins = Math.trunc(fullTime)
        var seconds = (num-mins*60)
        var sconds = (num-mins*60).toFixed(2);
        if(mins<10) 
        {
          minZero="0";
        } 
        if(seconds<10) 
        {
          secZero="0";
        } 
        element.time = minZero+mins+ ":"+secZero+sconds;
      }   
        });  
      }
    }

  }
  OpenModal()
  {

    $('#myModal').modal("show");
  }
}
