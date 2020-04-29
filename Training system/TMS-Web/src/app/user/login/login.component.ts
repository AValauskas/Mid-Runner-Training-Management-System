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
    
        if (!this.helper.CheckIfTokenIsExpired())
        {this._router.navigate([decodeURI("home")]);}
        else
        {this.helper.UnsetStorage();}    
   {     
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
      console.log(localStorage.getItem('error' ).substring(18,23));
      if(localStorage.getItem('error' ).substring(18,23)=="email")
      {
        this.error= "you should confirm your email first";
        localStorage.removeItem('error');
      }
      else{
       
        if( localStorage.getItem('error' ) == "[object ProgressEvent]"  )
        {
          this.error= "Server is not working right now";
          localStorage.removeItem('error');
        }
        else if(localStorage.getItem('error' ).length>100 || localStorage.getItem('error' ) == "[object ProgressEvent]"  )
        {
          localStorage.removeItem('error');
        }
        else{
      this.error= localStorage.getItem('error' );
      localStorage.removeItem('error');
        }
      }
    }
    else{
      this.error= null;      
    }
  }
  
}
