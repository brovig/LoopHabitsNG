import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';

import { HabitTypeChooseComponent } from '../habits/habit-type-choose.component';
import { Habit } from '../habits/habit';
import { HabitDeleteDialogComponent } from './habit-delete-dialog.component';
import { AuthService } from '../auth/auth.service';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  private destroySubject = new Subject();
  public isLoggedIn?: boolean = false;
  @Input() habit?: Habit;
  @Input() isAuthenticated?: boolean | null;
  public activeComponent: string | undefined;

  constructor(
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute) {
    
      this.authService.authStatus.pipe(takeUntil(this.destroySubject))
        .subscribe(result => {
          console.log('Check authStatus in navmenu constructor: ' + result);
          this.isLoggedIn = result;
        });

      const activeComp = this.route.snapshot.routeConfig?.component?.name;
      if (activeComp) {
        this.activeComponent = activeComp;
      } else {
        this.activeComponent = "HabitsComponent";
      }
  }

  ngOnInit() {
    this.isLoggedIn = this.authService.isAuthenticated();
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

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/']).then(() => window.location.reload());
  }

  ngOnDestroy() {
    this.destroySubject.next(true);
    this.destroySubject.complete();
  }

  settings() {
    console.log("settings button");
  }
}
