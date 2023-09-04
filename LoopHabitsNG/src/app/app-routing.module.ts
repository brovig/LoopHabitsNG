import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HabitsComponent } from './habits/habits.component';
import { HabitCreateComponent } from './habit-create/habit-create.component';
import { HabitDetailsComponent } from './habit-details/habit-details.component';
import { LoginComponent } from './auth/login.component';


const routes: Routes = [
  { path: '', component: HabitsComponent },
  { path: 'habits', component: HabitsComponent },
  { path: 'habit/:id', component: HabitCreateComponent },
  { path: 'habits/:id', component: HabitDetailsComponent },
  { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
