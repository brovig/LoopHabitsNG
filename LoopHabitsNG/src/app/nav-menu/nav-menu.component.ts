import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { HabitTypeChooseComponent } from '../habits/habit-type-choose.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  constructor(private dialog: MatDialog,
    private router: Router) { }

  addHabit(): void {
    const dialogRef = this.dialog.open(HabitTypeChooseComponent);

    dialogRef.afterClosed().subscribe(habitType => {
      if (typeof habitType === "number") {
        this.router.navigate(['/habit', habitType]);
      }
    }, error => console.error(error));
  }

  settings() {
    console.log("settings button");
  }
}
