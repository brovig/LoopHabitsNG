import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HabitsComponent } from './habits/habits.component';
import { HabitCreateComponent } from './habit-create/habit-create.component';
import { HabitDetailsComponent } from './habit-details/habit-details.component';
import { LoginComponent } from './auth/login.component';
import { AuthGuard } from './auth/auth.guard';
import { RegisterComponent } from './auth/register.component';


const routes: Routes = [
  { path: '', component: HabitsComponent },
  { path: 'habits', component: HabitsComponent },
  { path: 'habits/:id', component: HabitDetailsComponent, canActivate: [AuthGuard] },
  { path: 'habit/:id', component: HabitCreateComponent, canActivate: [AuthGuard] },  
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
