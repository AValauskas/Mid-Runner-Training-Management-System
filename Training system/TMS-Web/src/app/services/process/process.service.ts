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
  private personalTraining = "personaltraining";
  private auth = "auth";
  
  constructor(public _router:Router, private httpserv: HttpService, private http: HttpClient) { }
//-------------------Training TEMPLATES------------------------------
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

  GetTrainingTemplateById(id)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl +"/"+id,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetTrainingTemplatesIncludedPersonal()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl+"/allTrainings","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetTrainingsByType(type)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl+"/TrainingsByType/"+ type,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  DeleteTrainingTemplate(id)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl+"/"+ id,"Delete",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  UpdateTrainingTemplate(data)
  {   
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.trainingTemplatesUrl,"Put",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


  //---------------------PERSONAL-----------------------------------
  GetPersonalInfo()
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/personal","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  ChangePassword(data)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/password","Patch",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


//---------------------------------Competitions-------------------------

   AddNewCompetition(data)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/newcompetition","Patch",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetRecords()
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/records","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  GetCompetitions()
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/competitions","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  GetAthletesWhichStillFree(date)
  { 
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/athleteList/"+date,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


  //----------------------------INVITATIONS----------------------------
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



  //-------------------------------------------PERSONALTRAININGS------------------------------------------------
  GetPersonalTrainings()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/athlete","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetPersonalTrainingByDate(data)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/date/"+data,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetPersonalTrainingById(id)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/"+id,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  GetPersonalTrainingsBusy()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/countByBusy","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  InsertPersonalTraining(data)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining,"Post",data,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  DeletePersonalTraining(id)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/"+ id,"Delete",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  
  UpdatePersonalTrainingResults(record, id)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/results/"+ id,"Patch",record,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

 

  GetAllCoachAssignedTrainings()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/coach","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }

  GetAllCoachAssignedTrainingsByDate(date)
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalTraining+"/coach/"+date,"Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


  GetPersonalCoach()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/personalcoach/","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


  GetUserFriends()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/friends/","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }


  GetAthletesByCoach()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })    
    return this.httpserv.requestCall(this.personalManagement+"/coachAthletes/","Get",null,HeadersForProductAPI).pipe(catchError(this.HandleError)); 
  }
  private HandleError(errorResponse: HttpErrorResponse){
    if(errorResponse.status!=200){
      return "error";
    }  
  }



}




