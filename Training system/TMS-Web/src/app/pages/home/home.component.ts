declare var $ :any;
import { Component, ViewChild, ViewEncapsulation, OnInit } from '@angular/core';  
import { FullCalendarComponent } from '@fullcalendar/angular';  
import { EventInput } from '@fullcalendar/core';  
import dayGridPlugin from '@fullcalendar/daygrid';  
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import interactionPlugin from '@fullcalendar/interaction';
import bootstrapPlugin from '@fullcalendar/bootstrap';
import listPlugin from '@fullcalendar/list';
import { PersonalTrainingModalComponent } from 'src/app/Modals/AllModals/personal-training-modal/personal-training-modal.component';
import { IPersonalTraining } from '../../Interfaces/IPersonalTraining';
import { ITrainingTemplate } from '../../Interfaces/ITrainingTemplate';
import { ThrowStmt } from '@angular/compiler';
import { ITrainingDefinition } from 'src/app/Interfaces/ITrainingDefinition';
import { ICoachAssignedTraining } from 'src/app/Interfaces/ICoachAssignedTraining';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  //changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  //Calendar stuff
  @ViewChild('calendar') calendarComponent: FullCalendarComponent; // the #calendar in the template  
  exist = false;
  calendarPlugins = [dayGridPlugin, interactionPlugin, bootstrapPlugin, listPlugin];
  Role: string;
  calendarEvents: EventInput[] = [];  
  ListClicked= false;
  TrainingFormActive= false;
  PersonalTrainActive= false;
  listSizeBig= false;
  successMessage=false;
  failMessage = false;
  deleteModalActive=false;

  message:string;
  personalTrainingIdToDelete:string

  //----------Athlete
  trainings: IPersonalTraining[]; 
  ChosenTraining:IPersonalTraining;
  ToDoInTraining:ITrainingTemplate;
    //----------Coach
 
  CoachAssignedTrainings: IPersonalTraining[];
  dateClicked:string;
  CoachBusy: ITrainingDefinition[];
  AssignedTrainingsByDate:ICoachAssignedTraining[];
  TrainingToSendForModal:ICoachAssignedTraining=<ICoachAssignedTraining>{};


  constructor(private _http: ProcessService,public _router:Router) { }

  //-------------------------------Data to display-------------------------------------------
  ngOnInit() {  

    if(localStorage.getItem('role')==null)
    {
      this._router.navigateByUrl('/login');
    }   
    localStorage.removeItem("friend");
    localStorage.removeItem("friendId");   
    localStorage.removeItem("visit");
    this.Role=localStorage.getItem('role')
    if(this.Role=="Athlete")
    this.HttpCallAthlete();  
    if(this.Role=="Coach")
    this.HttpCallCoach(); 
  }  
  
    HttpCallAthlete(){ 
        this._http.GetPersonalTrainings().subscribe(data=>{   
          
        this.trainings = data;
        this.trainings.forEach(element => {
          this.calendarEvents.push({ title: element.description, date: new Date(element.day)})
          });          
        });      
    }

    HttpCallCoach(){ 
      this._http.GetPersonalTrainingsBusy().subscribe(data=>{   
        this.CoachBusy = data;    
        this.CoachBusy.forEach(element => {
          var splitted = element.description.split("/", 3); 
          var eventColor="blue";
          var firstNum = Number(splitted[0]);
          var secondNum = Number(splitted[1]);
          if((firstNum/secondNum)>=0.75)
          {eventColor="green"}
          else if ((firstNum/secondNum)>=0.5)
          {eventColor="rgb(240,230,140)"}
          else if ((firstNum/secondNum)>=0.25)
          {eventColor="grey"}
          else
          {eventColor="rgb(139,0,0)"}
          this.calendarEvents.push({ title: element.description, date: new Date(element.day), color:eventColor})
          });   
      });    
     
  }
//---------------------------------Clicks on dates-------------------------------------
    eventClick(model) {  
      console.log(model);
    
    }  
  
    dateClick(model) {  
      this.dateClicked = model.dateStr;
      if(this.Role=="Athlete"){
        this.AthleteDateClick(model)     
        }
        else if(this.Role=="Coach"){
      this.CoachDateClick(model);
        }
    }  


    AthleteDateClick(model)
    {
      this._http.GetPersonalTrainingByDate(model.dateStr).subscribe(data=>{   
        this.ChosenTraining= data;
        if(data!=null)
        {
          this.exist = true;
          this._http.GetTrainingTemplateById(this.ChosenTraining.trainTemplateId).subscribe(data2=>{   
            this.ToDoInTraining=data2;
            
            console.log(this.ToDoInTraining);
            $('#myModal').modal("show");
            });
        }else
        {
          this.exist = false;
          $('#myModal').modal("show");
        }         
    });
    }

    CoachDateClick(model){
      
      this._http.GetAllCoachAssignedTrainingsByDate(model.dateStr).subscribe(data=>{   
        this.AssignedTrainingsByDate= data;   
        this.ListClicked = true;
        console.log(data)  
         }); 
    }
    


    //-----------------------other stuff
    renewTrainings(message)
    {
        this.calendarEvents = []
        this.HttpCallCoach();

    this.SuccesfullyMadeByCoachMessage(message);
        
    }


   FindByDate(train, date) { 
      return train.day === new Date(date);
    }
    

    //------------------deleting-----------------------
    DeletePersonal(id)
    {
      this.personalTrainingIdToDelete=id;
      this.deleteModalActive=true;
      $('#myModalDelete').modal("show");
    }
    DeletePersonalTraining()
    {
      this._http.DeletePersonalTraining(this.personalTrainingIdToDelete).subscribe(data=>{   
      this._http.GetAllCoachAssignedTrainingsByDate(this.dateClicked).subscribe(data2=>{   
       this.AssignedTrainingsByDate= data2;   
       this.calendarEvents = [];
       $('#myModalDelete').modal("hide");  
       this.SuccesfullyMadeByCoachMessage("Training was succesfully deleted")  
       this.HttpCallCoach();
        }); 
        this.deleteModalActive = false;
      });  
    
    
    }
    //-------------------------------Clicked to open new training modal---------------------------------
    OpenNewTrainingModal()
    {
      console.log("paspaude");
      this.TrainingFormActive= true;
      this.PersonalTrainActive= false;
      $('#myModal').modal("show");

    }
//---------------------Open personal training modal----------------
    TurnOnTrain(trainingToSend)
    {
      console.log(trainingToSend);
      this.PersonalTrainActive= true;
      this.TrainingFormActive=false;
      this.TrainingToSendForModal = trainingToSend;
        console.log(this.TrainingToSendForModal)

        $('#myModal').modal("show");
    }


    ChangeAthleteListSize()
    {
      if(this.listSizeBig)
      {this.listSizeBig= false;}
      else{
        this.listSizeBig= true;
      }
    }

///----------------- messages------------
    async TurnOnSuccesMessageAthlete()
    {
      this.successMessage = true;
      this.message = "You have succesfully updated your training"
      await this.delay(3000);
      this.successMessage = false;
      this.message="";
    }

    async SuccesfullyMadeByCoachMessage(message)
    {
      console.log(message);
      this.successMessage = true;
      this.message = message;
      await this.delay(3000);
      this.successMessage = false;
      this.message="";
    }

   delay(ms: number) {
      return new Promise( resolve => setTimeout(resolve, ms) );
  }
}
