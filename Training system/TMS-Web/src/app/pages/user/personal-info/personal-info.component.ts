import { Component, OnInit } from '@angular/core';
import { Iuser } from 'src/app/Interfaces/IUser';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.scss']
})
export class PersonalInfoComponent implements OnInit {

  user:Iuser = new Iuser();
  role:string;
  firstPass: string;
  secondPass: string;
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this.role= localStorage.getItem('role')
    this._http.GetPersonalInfo().subscribe(data=>{
      this.user= data;
      console.log( this.user);
    })

  }

  ChangePassword()
  {
    if(this.firstPass==this.secondPass)
    {
      this.user.password=this.firstPass;
      this._http.ChangePassword(this.user).subscribe(data=>{        
        this.firstPass="";
        this.secondPass="";
        console.log( this.user);
      })
    }
    else{
      console.log("nepavyko!")
    }
  }

}
