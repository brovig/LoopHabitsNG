import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Habit } from './habits/habit';
import { NavigationStart, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ShareService {

  private _activeComp = new Subject<string>();
  public activeComp = this._activeComp.asObservable();

  private _habit = new Subject<Habit>();
  public habit = this._habit.asObservable();

  constructor(private router: Router) { }

  init() {
    this.router.events.subscribe((val) => {
      if (val instanceof NavigationStart) {
        const currentUrl = val.url;
        const urlParts = currentUrl.split('/');

        let currentComp: string = "HabitsComponent";

        switch (urlParts.length) {
          case 2:
            if (!urlParts[1] || urlParts[1] === 'habits') {
              currentComp = "HabitsComponent";
            } else if (urlParts[1] === 'login') {
              currentComp = "LoginComponent";
            } else if (urlParts[1] === 'register') {
              currentComp = "RegisterComponent";
            }
            break;
          case 3:
            if (urlParts[2] === '0' || urlParts[2] === '1' || urlParts[1] === 'habit') {
              currentComp = "HabitCreateComponent";
            } else {
              currentComp = "HabitDetailsComponent";
            }
            break;
          default:
            currentComp = "HabitsComponent";
        }

        this.setActiveComp(currentComp);
      }
    })
  }

  public setActiveComp(comp: string) {
    this._activeComp.next(comp);
  }

  public setHabit(habitToShare: Habit) {
    this._habit.next(habitToShare);
  }
}
