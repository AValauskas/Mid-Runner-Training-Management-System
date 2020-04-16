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
  private child: PersonalTrainingModalComponent;
  calendarVisible = true;  
  calendarWeekends = true;  
  exist = false;
  calendarPlugins = [dayGridPlugin, interactionPlugin, bootstrapPlugin, listPlugin];
  Role: string;
  calendarEvents: EventInput[] = [];  


  //----------Athlete
  trainings: IPersonalTraining[]; 
  ChosenTraining:IPersonalTraining;
  ToDoInTraining:ITrainingTemplate;
    //----------Coach
 
  CoachAssignedTrainings: IPersonalTraining[];
  dateClicked:string;
  CoachBusy: ITrainingDefinition[];
 



  constructor(private _http: ProcessService,public _router:Router) { }

  //-------------------------------Data to display-------------------------------------------
  ngOnInit() {  
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
          this.calendarEvents.push({ title: element.description, date: new Date(element.day) })
          });   
      });
     
     
      this._http.GetAllCoachAssignedTrainings().subscribe(data=>{   
        
      this.CoachAssignedTrainings = data;     
            
      });      
  }
//---------------------------------Clicks-------------------------------------
    eventClick(model) {  
      console.log(model);
    
    }  
  
    dateClick(model) {  
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
      this.dateClicked =model.dateStr;
      console.log(this.dateClicked);
    //  this._http.GetAthletesWhichStillFree(model.dateStr).subscribe(data=>{   
     // console.log(data);

       
    //  });
      $('#myModal').modal("show");

    }
    renewTrainings()
    {
      this._http.GetAllCoachAssignedTrainings().subscribe(data=>{   
        this.calendarEvents = []
        this.CoachAssignedTrainings = data;
        
        this.CoachAssignedTrainings.forEach(element => {
          this.calendarEvents.push({ title: element.description, date: new Date(element.day), trainingId:element.id })
          });          
        }); 
      console.log("atejo");
    }


















   FindByDate(train, date) { 
      return train.day === new Date(date);
    }
    










    toggleVisible() {  
      this.calendarVisible = !this.calendarVisible;  
    }  

  eventDragStop(model) {  
    console.log(model);

  }      
  toggleWeekends() {  
    this.calendarWeekends = !this.calendarWeekends;  
  }  
 
}
