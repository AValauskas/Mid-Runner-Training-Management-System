import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpService } from '../http/http.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {

  private trainingTemplatesUrl = "training";
  private personalManagement = "personalmanagement";
  
  constructor(public _router:Router, private httpserv: HttpService, private http: HttpClient) { }

  GetTrainingTemplates()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  PostTrainingTemplates(data)
  {   
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl,"Post",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetRecords()
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetInvitations()
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/invitations","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  AcceptInvite(data)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/AcceptInvite","Patch",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  DeclineInvitation(data)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/DeclineInvite","Patch",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  SendInvite(data)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/invite","Patch",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  private HandleError(errorResponse: HttpErrorResponse){
    if(errorResponse.status!=200){
      return "error";
    }  
  }
}
