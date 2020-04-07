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
import { PersonalTrainingsComponent } from './pages/trainings/personal-trainings/personal-trainings.component';
import { MainModalComponent } from './Modals/main-modal/main-modal.component';
import { PersonalBestComponent } from './pages/user/personal-best/personal-best.component';
import { PersonalInfoComponent } from './pages/user/personal-info/personal-info.component';
import { InvitationsComponent } from './nav-bar/invitations/invitations.component';

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
    PersonalTrainingsComponent,
    MainModalComponent,
    PersonalBestComponent,
    PersonalInfoComponent,
    InvitationsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
     HttpClientModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }