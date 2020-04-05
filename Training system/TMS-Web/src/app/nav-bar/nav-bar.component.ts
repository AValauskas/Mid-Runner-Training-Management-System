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

}
