import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HabitsComponent } from './habits/habits.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { AppRoutingModule } from './app-routing.module';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RepEditComponent } from './habits/rep-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HabitTypeChooseComponent } from './habits/habit-type-choose.component';
import { HabitCreateComponent } from './habit-create/habit-create.component';
import { FrequencyDialogComponent } from './habit-create/frequency-dialog.component';
import { HabitDetailsComponent } from './habit-details/habit-details.component';
import { HabitDeleteDialogComponent } from './nav-menu/habit-delete-dialog.component';
import { LoginComponent } from './auth/login.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { RegisterComponent } from './auth/register.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { ConnectionServiceModule, ConnectionServiceOptions, ConnectionServiceOptionsToken } from 'ng-connection-service';
import { environment } from '../environments/environment';


export function DECLARATIONS() {
  return [
    AppComponent,
    HabitsComponent,
    NavMenuComponent,
    RepEditComponent,
    HabitTypeChooseComponent,
    HabitCreateComponent,
    FrequencyDialogComponent,
    HabitDetailsComponent,
    HabitDeleteDialogComponent,
    LoginComponent,
    RegisterComponent
  ]
}

@NgModule({
  declarations: DECLARATIONS(),
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    ConnectionServiceModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: ConnectionServiceOptionsToken,
      useValue: <ConnectionServiceOptions>{
        enableHeartbeat: true,
        heartbeatUrl: environment.baseUrl + 'api/heartbeat',
        heartbeatInterval: 5000
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  static declarations = DECLARATIONS;
}
