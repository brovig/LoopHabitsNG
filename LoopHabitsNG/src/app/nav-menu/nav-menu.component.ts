import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { HabitTypeChooseComponent } from '../habits/habit-type-choose.component';
import { Habit } from '../habits/habit';
import { HabitDeleteDialogComponent } from './habit-delete-dialog.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent{
  @Input() habit?: Habit;
  public activeComponent: string | undefined;

  constructor(private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute) {
    const activeComp = this.route.snapshot.routeConfig?.component?.name;
    if (activeComp) {
      this.activeComponent = activeComp;
    } else {
      this.activeComponent = "HabitsComponent";
    }
  }

  addHabit() {
    const dialogRef = this.dialog.open(HabitTypeChooseComponent);

    dialogRef.afterClosed().subscribe(habitType => {
      if (typeof habitType === "number") {
        this.router.navigate(['/habit', habitType]);
      }
    }, error => console.error(error));
  }

  deleteHabit() {
    console.log(this.habit);
    this.dialog.open(HabitDeleteDialogComponent, { data: { habitId: this.habit!.id } });
  }

  settings() {
    console.log("settings button");
  }
}
