import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenParams } from '../../user/login/TokenParams';
import * as jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { HttpService } from '../http/http.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  //private _loginUrl = "https://localhost:44391/api/Auth/Login";
  private _loginUrl = "Auth/Login";
  private _registerUrl = "Auth/Register";


  constructor(public _router:Router, private httpserv: HttpService) { }

  loginUser(data):Observable<TokenParams>
  {
    console.log(data);
   return this.httpserv.requestCall(this._loginUrl,"Post",data,null).pipe(catchError(this.HandleError));   
  }

  registerUser(data)
  {
    console.log(data);
    return this.httpserv.requestCall(this._registerUrl,"Post",data,null).pipe(catchError(this.HandleError)); 
   // let header = new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' });
    //return this.http.post<any>(this._registerUtl, data)
          
  }

  logoutUser()
  {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem('user');
    localStorage.removeItem('expire')         
    this._router.navigate([decodeURI("login")]);       
  }
    
  private HandleError(errorResponse: HttpErrorResponse){
    if(errorResponse.status!=200){
      return "error";
    }  
  }
}
