import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './pages/home/home.component';
import {TrainingTemplatesComponent} from './pages/trainings/training-templates/training-templates.component';
import {LoginComponent} from './user/login/login.component';
import {RegisterComponent} from './user/register/register.component';
import {UserComponent} from './user/user.component';
import { PersonalBestComponent } from './pages/user/personal-best/personal-best.component';
import { PersonalInfoComponent } from './pages/user/personal-info/personal-info.component';
import { PersonalStuffComponent } from './pages/user/personal-stuff/personal-stuff.component';

const routes: Routes = [
{path: '', component: UserComponent,
children: [{path: '', component:LoginComponent}]},
{path: 'home', component: HomeComponent},
{path: 'trainingTemplates', component: TrainingTemplatesComponent},
{path: 'personalBest', component: PersonalBestComponent},
{path: 'personal', component: PersonalStuffComponent},
{path: 'personalInfo', component: PersonalInfoComponent},
{path: 'register', component: UserComponent,
children: [{path: '', component:RegisterComponent}]},
{path: 'login', component: UserComponent,
children: [{path: '', component:LoginComponent}]},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
