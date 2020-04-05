import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  LoginActive = true;
  RegisterActive = false;

  constructor(private router: Router) { }

  ChangeActive() {
    if(this.router.url =="/login")
    {
    this.LoginActive=true;
    this.RegisterActive=false;
    }
    if(this.router.url =="/register")
    {
    this.LoginActive=false;
    this.RegisterActive=true;   
    }
    return console.log(this.router.url)
}

    
  ngOnInit(): void {
    if(this.router.url =="/login")
    {
    this.LoginActive=true;
    this.RegisterActive=false;
    }
    if(this.router.url =="/register")
    {
    this.LoginActive=false;
    this.RegisterActive=true;   
    }
  }

}
