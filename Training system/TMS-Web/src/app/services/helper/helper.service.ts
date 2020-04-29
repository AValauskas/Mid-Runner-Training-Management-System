import { Injectable } from '@angular/core';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  constructor() { }
  ProcessToken(token)
  {
    console.log(token);
    var decoded = jwt_decode(token); 
    var expire = decoded.exp;
    console.log(expire);
    var Role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']; 
    var Id = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']; 
    localStorage.setItem('token', token )
    localStorage.setItem('role', Role ) 
    localStorage.setItem('user', Id )   
    localStorage.setItem('user', Id )   
    localStorage.setItem('expire', expire )    
    
  }

  UnsetStorage()
  {
    localStorage.removeItem('expire' ) 
    localStorage.removeItem('token')
    localStorage.removeItem('role') 
    localStorage.removeItem('user' )      
  }

  CheckIfTokenIsExpired()
  {
    var current_time = new Date().getTime() / 1000;
    if(localStorage.hasOwnProperty('expire')){
    var expire = Number(localStorage.getItem('expire'));
    if (current_time >  expire ) { 
      return true;
     }
     else {false;}
    }   
    return true;
  }
  
}
