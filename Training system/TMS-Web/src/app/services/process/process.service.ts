import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {

  private trainingTemplates = "https://localhost:44391/api/training";
  
  constructor(private http: HttpClient, public _router:Router) { }

  getTrainingTemplates()
  {
    const HeadersForProductAPI = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })
    console.log(localStorage.getItem('token')); 
      return this.http.get(this.trainingTemplates, {
        headers: HeadersForProductAPI
      })
  }
}
