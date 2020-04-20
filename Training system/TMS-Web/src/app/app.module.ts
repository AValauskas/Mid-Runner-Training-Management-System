import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { FormsModule } from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { UserComponent } from './user/user.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { TrainingTemplatesComponent } from './pages/trainings/training-templates/training-templates.component';
import { MainModalComponent } from './Modals/main-modal/main-modal.component';
import { PersonalBestComponent } from './pages/user/personal-best/personal-best.component';
import { PersonalInfoComponent } from './pages/user/personal-info/personal-info.component';
import { InvitationsComponent } from './nav-bar/invitations/invitations.component';
import { PersonalStuffComponent } from './pages/user/personal-stuff/personal-stuff.component';
import { CompetitionsComponent } from './pages/user/competitions/competitions.component';
import { TrainingsComponent } from './pages/user/trainings/trainings.component';
import {FullCalendarModule} from '@fullcalendar/angular';
import { PersonalTrainingModalComponent } from './Modals/AllModals/personal-training-modal/personal-training-modal.component';
import { CommonModule } from '@angular/common';
import { FlatpickrModule } from 'angularx-flatpickr';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { CoachTrainingAssignModalComponent } from './Modals/AllModals/coach-training-assign-modal/coach-training-assign-modal.component';
import { AthleteListComponent } from './Modals/AllModals/athlete-list/athlete-list.component';
import { NewCompetitionModalComponent } from './Modals/AllModals/new-competition-modal/new-competition-modal.component';



//import { NavbarComponent } from './navbar/navbar.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    UserComponent,
    NavBarComponent,
    TrainingTemplatesComponent,
    MainModalComponent,
    PersonalBestComponent,
    PersonalInfoComponent,
    InvitationsComponent,
    PersonalStuffComponent,
    CompetitionsComponent,
    TrainingsComponent,
    PersonalTrainingModalComponent,
    CoachTrainingAssignModalComponent,
    AthleteListComponent,
    NewCompetitionModalComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
     HttpClientModule,
     FullCalendarModule,
     NgbModalModule,
     FlatpickrModule.forRoot(),
     CalendarModule.forRoot({
       provide: DateAdapter,
       useFactory: adapterFactory,
     }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
