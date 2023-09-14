import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';

import { HabitTypeChooseComponent } from '../habits/habit-type-choose.component';
import { Habit } from '../habits/habit';
import { HabitDeleteDialogComponent } from './habit-delete-dialog.component';
import { AuthService } from '../auth/auth.service';
import { ShareService } from '../share.service';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  private destroySubject = new Subject();
  public isLoggedIn?: boolean = false;
  public habit?: Habit;
  public activeComponent!: string;
  public activeRoute!: string;

  constructor(
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router,
    private shareService: ShareService) {    
      this.authService.authStatus.pipe(takeUntil(this.destroySubject))
        .subscribe(result => {
          this.isLoggedIn = result;
        });

    this.shareService.activeComp.subscribe(result => {
        this.activeComponent = result;
      });

    this.shareService.habit.subscribe(result => {
      this.habit = result;
      }); 
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
}
