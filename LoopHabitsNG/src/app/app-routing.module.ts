import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HabitsComponent } from './habits/habits.component';
import { HabitCreateComponent } from './habit-create/habit-create.component';


const routes: Routes = [
  { path: '', component: HabitsComponent },
  { path: 'habits', component: HabitsComponent },
  { path: 'create/:type', component: HabitCreateComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
