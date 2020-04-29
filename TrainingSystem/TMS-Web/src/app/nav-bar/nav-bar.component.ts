import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  Role: string;
  changeDetected:Boolean;
  successMessage=false;
  failMessage = false;
  message="";
  constructor(public _auth: AuthService) { }

  ngOnInit(): void {
    this.Role=localStorage.getItem('role')
    console.log(localStorage.getItem('role'))
  }

  ngDoCheck(): void
  {
   if (localStorage.getItem('role') !== this.Role) {
     this.changeDetected = true;
     this.Role=localStorage.getItem('role')
     console.log(localStorage.getItem('role'))
   }
  }

  ngOnChanges(changeDetected)
  {
   this.Role=localStorage.getItem('role')
   console.log(localStorage.getItem('role'))
  }


  async TurnOnSuccesMessage(message)
  {
    console.log(message);
    this.failMessage = false;
    this.successMessage = true;
    this.message = message;
    await this.delay(3000);
    this.successMessage = false;
    this.message="";
  }

  async TurnOnFailMessage(message)
  {
    console.log("atieji");
    this.successMessage = false;
    this.failMessage = true;
    this.message = message;
    await this.delay(3000);
    this.failMessage = false;
    this.message="";
  }

  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
}
}
