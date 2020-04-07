import { Component, OnInit } from '@angular/core';
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';

interface IInviter{
  Id:string;
}

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.scss']
})
export class InvitationsComponent implements OnInit {
  invitations: Object;
  error:string;
  success:string;
  inviter:IInviter = <any>{};
  constructor(private _http: ProcessService,public _router:Router) { }

  ngOnInit(): void {
    this._http.GetInvitations().subscribe(data=>{
      this.invitations = data
    console.log(data);
    })
  }
 
  AcceptInvite(invaiterId:string)
  {
    this.inviter.Id =invaiterId;
    console.log(invaiterId);
    this._http.AcceptInvite(this.inviter).subscribe(data=>{      
      this.invitations = data;
        console.log(this.invitations);    
    })   
    if(localStorage.getItem("error")==null)
    {
      this.inviter.Id="";    
    }
  }

  DeclineInvite(invaiterId:string)
  {
    this.inviter.Id =invaiterId;
    console.log(invaiterId);
    this._http.DeclineInvitation(this.inviter).subscribe(data=>{      
      this.invitations = data;
        console.log(this.invitations); 
        if(localStorage.getItem("error")==null)
      {    
        invaiterId=null;  
        this.inviter.Id="";       
      }   
    })   
    
  }

  SendInvite()
  {
    console.log( this.inviter);
    this._http.SendInvite(this.inviter).subscribe(data=>{    
      if(localStorage.getItem("error")==null)
      {  
        this.success= "Invitation sent succesfully"
        this.inviter.Id="";    
      }
      else
      {
        this.error = localStorage.getItem("error");
      }
    })
  
  }

}
