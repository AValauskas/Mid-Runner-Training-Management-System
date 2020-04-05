import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './pages/home/home.component';
import {TrainingTemplatesComponent} from './pages/trainings/training-templates/training-templates.component';
import {PersonalTrainingsComponent} from './pages/trainings/personal-trainings/personal-trainings.component';
import {LoginComponent} from './user/login/login.component';
import {RegisterComponent} from './user/register/register.component';
import {UserComponent} from './user/user.component';

const routes: Routes = [
{path: '', component: UserComponent,
children: [{path: '', component:LoginComponent}]},
{path: 'home', component: HomeComponent},
{path: 'trainingTemplates', component: TrainingTemplatesComponent},
{path: 'personalTrainings', component: PersonalTrainingsComponent},
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
