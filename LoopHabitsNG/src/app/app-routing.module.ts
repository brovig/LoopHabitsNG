import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HabitsComponent } from './habits/habits.component';


const routes: Routes = [
  { path: '', component: HabitsComponent },
  { path: 'habits', component: HabitsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
