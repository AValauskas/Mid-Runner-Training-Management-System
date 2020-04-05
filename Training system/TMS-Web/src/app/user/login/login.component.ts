import { Component, OnInit } from '@angular/core';
import { TokenParams } from './TokenParams';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Router } from '@angular/router';
import { LoginForm } from './LoginForm';
import { HelperService } from 'src/app/services/helper/helper.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  error:String;
  loginUserData = <any>{};
  tokenParam: TokenParams;
  constructor(private _auth: AuthService,private helper:HelperService, public _router:Router) { }

  ngOnInit(): void {    
    if(localStorage.getItem('role')!=null)
    {
      this._router.navigate([decodeURI("home")]);     
    }
  }

  OnSubmit()
  {
    this._auth.loginUser(this.loginUserData).subscribe(
      data=>{  
        this.HandleError();
        if(this.error == null) 
        {
        console.log(data);
        this.helper.ProcessToken(data.Token)        
        this._router.navigate([decodeURI("home")]);  
        }            
      }     
    )  
     
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
