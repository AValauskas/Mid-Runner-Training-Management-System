import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { EventInput, Calendar } from '@fullcalendar/core';  
import dayGridPlugin from '@fullcalendar/daygrid';  
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import interactionPlugin from '@fullcalendar/interaction'
import bootstrapPlugin from '@fullcalendar/bootstrap';
import * as bootstrap from "bootstrap";
import { PersonalTrainingModalComponent } from 'src/app/Modals/AllModals/personal-training-modal/personal-training-modal.component';
import listPlugin from '@fullcalendar/list';

interface IPersonalTraining{
  day:Date;
  trainTemplateId:string;
  athleteId:string;   
  coachId:string;    
  athleteReport:string;    
  description:string;     
  place:string;  
  id:string;     
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {

  @ViewChild(PersonalTrainingModalComponent)
  private child: PersonalTrainingModalComponent;

  trainings: IPersonalTraining[];
  calendarEvents:EventInput[] = [];
  trainingdate:String;

  constructor(private _http: ProcessService,public _router:Router) { }
  SetId(date){
    console.log(date);


  }

  ngOnInit(): void {
  this.HttpCall();  
  }

  HttpCall(){ 
      this._http.GetPersonalTrainings().subscribe(data=>{   
        
      this.trainings = data;
      this.trainings.forEach(element => {
        this.calendarEvents.push({ title: element.description, date: new Date(element.day), trainingId:element.id })
        });
        this.CalendarCreation();
      });
  }

  CalendarCreation()
  {

      var calendarDiv:HTMLElement = document.getElementById('calendar');
      var calendar = new Calendar(calendarDiv,{
          editable: true,
          plugins: [dayGridPlugin, interactionPlugin, bootstrapPlugin, listPlugin], // important!
          events: this.calendarEvents,
          themeSystem:'boostrap',
          
          dateClick: function(info){    
          //  this.SetId(info.date) ;
         ///   console.log(info);    
           // console.log(this.trainingdate);   
           this.trainingdate = info.dateStr;        
          //   console.log(this.trainingdate); 
          
           
          
          

           $('#myModal').modal("show");
           },           
                             
      });
     
      calendar.render();

  }

 
  





















  /*
  showModalBox: boolean = false;
 
  calendarPlugins = [dayGridPlugin, interaactionPlugin, bootstrapPlugin]; // important!
  themeSystem: 'bootstrap'
  trainings: IPersonalTraining[];
  calendarEvents:EventInput[] = [   { title: 'event 1', date: new Date(2020, 3, 9) }, ];
  isUpdate = false;
  constructor(private _http: ProcessService,public _router:Router) { }
  changeDetected= false;


  dayRender(args){
      var cell:HTMLElement = args.el;
      cell.ondblclick=(ev:MouseEvent)=>
      {
        this.open();

      }
   
  }
  addEvent(date: any) {
    var title = prompt('Enter event title');
    this.calendarEvents=this.calendarEvents.concat({
      title:title,
    start:date});
   alert(date);
  }


  ngOnInit(): void {
   
   this._http.GetPersonalTrainings().subscribe(data=>{   
        
      this.trainings = data;
      console.log(this.trainings);
      this.trainings.forEach(element => {
        this.calendarEvents.push({ title: element.description, date: new Date(element.day) })
          console.log(this.calendarEvents);      
        });
    }) 
   
  }



  public open() {
   
    console.log("ateja");
       this.showModalBox = true;
       console.log(this.showModalBox);

  }
 /* ngDoCheck()
  {
   // console.log("tikrina changes");
   if (this.isUpdate==true) {
    this.trainings.forEach(element => {
      this.calendarEvents.push({ title: element.description, date: new Date(2020, 3, 12) })
        console.log(this.calendarEvents);     
      });
      this.isUpdate = false;      
   }
  }

  ngOnChanges(changeDetected)
  {
    console.log("padaro changes");
    this.trainings.forEach(element => {
      this.calendarEvents.push({ title: element.description, date: new Date(2020, 3, 12) })
        console.log(this.calendarEvents);
        this.isUpdate = false;      
      });
  }
*/

}
