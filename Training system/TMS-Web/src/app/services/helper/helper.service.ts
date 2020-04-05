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
    var Role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']; 
    localStorage.setItem('token', token )
    localStorage.setItem('role', Role )    
  }
}
