import { Component, OnInit } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-personal-best',
  templateUrl: './personal-best.component.html',
  styleUrls: ['./personal-best.component.scss']
})
export class PersonalBestComponent implements OnInit {

  competitions: Object;

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    
  }

  ngAfterViewInit(): void
  {    
    this._http.GetRecords().subscribe(data=>{
      this.competitions = data
    
    })
  }
}
