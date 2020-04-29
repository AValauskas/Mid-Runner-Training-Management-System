import { Component, OnInit } from '@angular/core';
import { RegisterForm } from './RegisterForm';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Router } from '@angular/router';
import { HelperService } from 'src/app/services/helper/helper.service';
import { Iuser } from 'src/app/Interfaces/IUser';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  user= <Iuser>{};
  checkedAthlete=true;
  checkedCoach = false;
  role="Athlete";
  pw1:string;
  pw2:string;
  error:String;
  constructor(private _auth: AuthService, private helper:HelperService, public _router:Router) { }

  

  OnSubmit()
  {
    console.log(this.user);
    if(this.pw1!=this.pw2)
    {
      console.log(this.pw1);
      console.log(this.pw2);
      this.error="Passwords are not the same";

    }
    else{
      this.user.password = this.pw1;
    this._auth.registerUser(this.user).subscribe(
      data=>{  
        this.HandleError();
        if(this.error == null) 
        {  
        this._router.navigate([decodeURI("login")]);  
        }            
      }    
    )  
  }
  }

  ngOnInit(): void {
    this.user.role="Athlete";
  }



  HandleError()
  {
    console.log("aaten");
    if(localStorage.getItem('error') !=null)
    {
      console.log("a y");
      this.error= localStorage.getItem('error' );
      localStorage.removeItem('error');
    }
    else{
      this.error= null;      
    }
  }
}
