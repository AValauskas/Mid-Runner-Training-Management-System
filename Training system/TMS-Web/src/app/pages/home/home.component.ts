import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FullCalendarComponent } from '@fullcalendar/angular';  
import { EventInput } from '@fullcalendar/core';  
import dayGridPlugin from '@fullcalendar/daygrid';  
import { ProcessService } from 'src/app/services/process/process.service';
import { Router } from '@angular/router';
import interaactionPlugin from '@fullcalendar/interaction'
import bootstrapPlugin from '@fullcalendar/bootstrap';

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

  calendarPlugins = [dayGridPlugin, interaactionPlugin, bootstrapPlugin]; // important!
  themeSystem: 'bootstrap';
  trainings: IPersonalTraining[];
  calendarEvents:EventInput[] = [   { title: 'event 1', date: new Date(2020, 3, 9) }, ];
  isUpdate = false;
  constructor(private _http: ProcessService,public _router:Router) { }
  changeDetected= false;


  dayRender(args){
      var cell:HTMLElement = args.el;
      cell.ondblclick=(ev:MouseEvent)=>
      {
        this.addEvent(args.date);

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
