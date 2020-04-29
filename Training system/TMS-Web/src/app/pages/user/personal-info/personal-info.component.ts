import { Component, OnInit } from '@angular/core';
import { Iuser } from 'src/app/Interfaces/IUser';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.scss']
})
export class PersonalInfoComponent implements OnInit {

  user:Iuser = new Iuser();
  role:string;
  firstPass: string;
  secondPass: string;
  coach:Iuser = new Iuser();
  friends:Iuser[]=[];
  athletes:Iuser[]=[];
  CoachClicked= false;
  friendsClicked= false;
  athletesClicked= false;
  successMessage= false;
  failMessage= false;
  message:string;

  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {                
    if(localStorage.getItem('role')==null)
    {
      this._router.navigateByUrl('/login');
    }   
    localStorage.removeItem("friend");
    localStorage.removeItem("friendId");   
    localStorage.removeItem("visit");   

    this.role= localStorage.getItem('role')
    this._http.GetPersonalInfo().subscribe(data=>{
      this.user= data;
      console.log( this.user);
    })
    if (this.role=="Athlete"){   
      this._http.GetPersonalCoach().subscribe(data=>{
        this.coach= data;
        console.log( this.coach);
      });
      this._http.GetUserFriends().subscribe(data=>{
        this.friends= data;
        console.log( this.friends);
      });
    }
    if (this.role=="Coach"){   
      this._http.GetAthletesByCoach().subscribe(data=>{
        this.athletes= data;
        console.log( this.athletes);
      });
    }

  }

  ChangePassword()
  {
    if(this.firstPass==this.secondPass)
    {
      this.user.password=this.firstPass;
      this._http.ChangePassword(this.user).subscribe(data=>{        
        this.firstPass="";
        this.secondPass="";
        console.log( this.user);
        this.SuccesfullyMessage("Password succesfully changed")
      })
    }
    else{
      this.FailMessage("Password missmatch")
    }
  }

  ChangeCoachSize()
  {
    console.log("clicked");
    if( this.CoachClicked)
    {
      this.CoachClicked= false;
    }
    else{
      this.CoachClicked= true;
    }
  }

  ChangeFriendsSize()
  {
    console.log("clicked");
    if( this.friendsClicked)
    {
      this.friendsClicked= false;
    }
    else{
      this.friendsClicked= true;
    }
  }

  ChangeAthleteSize()
  {
    console.log("clicked");
    if( this.athletesClicked)
    {
      this.athletesClicked= false;
    }
    else{
      this.athletesClicked= true;
    }
  }

  CheckFriend(other:Iuser)
  {
    console.log(other);
   localStorage.setItem("friendId", other.idPerson);
   localStorage.setItem("friend", other.name+ " " + other.surname);
   this._router.navigateByUrl('/personal');
  }


  async SuccesfullyMessage(message)
  {
    console.log(message);
    this.successMessage = true;
    this.message = message;
    await this.delay(3000);
    this.successMessage = false;
    this.message="";
  }

  async FailMessage(message)
  {
    console.log(message);
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
