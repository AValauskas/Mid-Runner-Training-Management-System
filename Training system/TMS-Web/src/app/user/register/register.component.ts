import { Component, OnInit } from '@angular/core';
import { RegisterForm } from './RegisterForm';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Router } from '@angular/router';
import { HelperService } from 'src/app/services/helper/helper.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  user= <any>{};
  error:String;
  constructor(private _auth: AuthService, private helper:HelperService, public _router:Router) { }

  

  OnSubmit()
  {
    console.log(this.user);
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

  ngOnInit(): void {
  }

  HandleError()
  {
    if(localStorage.getItem('error') !=null)
    {
      this.error= localStorage.getItem('error' );
      localStorage.removeItem('error');
    }
    else{
      this.error= null;      
    }
  }
}
