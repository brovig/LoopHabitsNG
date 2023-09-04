import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

import { Habit } from './habit';
import { HabitService } from './habit.service';
import { RepetitionService } from './repetition.service';
import { Repetition } from './repetition';
import { MatDialog } from '@angular/material/dialog';
import { RepEditComponent } from './rep-edit.component';
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-habits',
  templateUrl: './habits.component.html',
  styleUrls: ['./habits.component.scss']
})
export class HabitsComponent implements OnInit {
  public displayedColumns: string[] = ['name'];
  public habits: Habit[] = [];
  public dataSource!: MatTableDataSource<Habit>;
  public dates: Date[] = [];
  public displayedDates: string[] = [];
  public numberOfDaysToDisplay!: number;
  public screenWidth!: number;
  public authStatus$: Observable<boolean>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private habitService: HabitService,
    private repetitionService: RepetitionService,
    private dialog: MatDialog) {
    this.authStatus$ = this.authService.authStatus;
  }

  ngOnInit() {
    this.getData();    
  }

  getData() {
    this.habitService.getData().subscribe(habitsResult => {
      this.habits = habitsResult;
      this.getDatesToDisplay();
      this.populateRepetitionsForHabit(this.dates, this.habits);
      this.populateDisplayedColumns(this.dates);
      this.displayedDates = this.displayedColumns.slice(1);
      this.dataSource = new MatTableDataSource(this.habits);
    }, error => console.error(error));
  }

  getDatesToDisplay() {
    // Calculate the amount of repetitions to display on screen
    this.screenWidth = window.innerWidth;
    this.numberOfDaysToDisplay = Math.floor((this.screenWidth - 100) / 64);

    // Populate dates
    const d = new Date();
    for (let i = 0; i < this.numberOfDaysToDisplay; i++) {
      const dateToAdd = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate() - i, 0));
      this.dates.push(dateToAdd);
    }
  }

  populateRepetitionsForHabit(dates: Date[], habits: Habit[]) {
    habits.forEach((h) => {
      this.repetitionService.getData(h.id, dates[dates.length - 1], dates[0]).subscribe(repetitionsResult => {
        const repetitionsToDisplay: Repetition[] = [];
        dates.forEach((d) => {
          const dateString = d.toISOString().split('.')[0] + 'Z';
          const repetitionFound = repetitionsResult.find(r => r.timeStamp === dateString);
          if (repetitionFound) {
            repetitionsToDisplay.push(repetitionFound);
          } else {
            let valueToDisplay: number = -1;
            if (h.type === 1) {
              valueToDisplay = 0;
            } 
            repetitionsToDisplay.push({
              id: -1,
              timeStamp: dateString,
              value: valueToDisplay
            });
          }
        });
        h.repetitions = repetitionsToDisplay;
      }, error => console.error(error));
    });
  }

  populateDisplayedColumns(dates: Date[]) {
    dates.forEach((d) => {
      const dateColumnName = d.toLocaleString('en-us', { weekday: 'short', day: "numeric" }).toUpperCase().split(" ").join("\n");
      this.displayedColumns.push(dateColumnName);
    })
  }

  handleRepButtonClick(habit: Habit, repetition: Repetition) {
    switch (habit.type) {
      // Yes/no habit
      case 0: {
        switch (repetition.value) {

          // Rep doesn't exist -> create one
          case -1: {
            repetition.value = 2;
            this.repetitionService.post(habit.id, repetition).subscribe((result) => {
              repetition.id = result.id;
              //this.router.navigate(['/habits']);
            });
            break;
          }

          // Rep exists but with zero value (was created earlier and canceled) -> change value to "2"
          case 0: {
            repetition.value = 2;
            this.repetitionService.put(habit.id, repetition).subscribe(() => { })
            break;
          }

          // Rep exists -> change value to "0"
          case 2: {
            repetition.value = 0;
            this.repetitionService.put(habit.id, repetition).subscribe(() => { })
            break;
          }
        }
        break;
      }
      // Measurable habit
      case 1: {
        this.editMeasurableRep(habit.id, repetition);
        break;
      }
    }
  }

  editMeasurableRep(habitId: string, rep: Repetition) {
    const dialogRef = this.dialog.open(RepEditComponent, { data: rep.value/1000 });   

    dialogRef.afterClosed().subscribe(result => {
      if (typeof result === "number" && result >= 0) {
        // Rep doesn't exist -> create one
        if (rep.id === -1 && result !== 0) {
          rep.value = result * 1000;
          this.repetitionService.post(habitId, rep).subscribe((result) => {
            rep.id = result.id;
          });
        // Rep exists -> change if new value inserted
        } else {
          if (result === 0) {
            this.repetitionService.delete(habitId, rep.id).subscribe(() => { });
            rep.id = -1;
            rep.value = 0;
          } else if (result !== rep.value / 1000) {
            rep.value = result * 1000;
            this.repetitionService.put(habitId, rep).subscribe(() => { });
          }
        }
      }
    });

    
  }

  getColor(key: number) {
    switch (key) {
      case 1: { return '#ed9999'; }
      case 2: { return '#feaa90'; }
      case 3: { return '#fecb7f'; }
      case 4: { return '#feebb1'; }
      case 5: { return '#69efad'; }
      case 6: { return '#c4e0a5'; }
      case 7: { return '#e5ed9a'; }
      case 8: { return '#fef49b'; }
      case 9: { return '#7fcac3'; }
      case 10: { return '#7fdde9'; }
      case 11: { return '#80d4f9'; }
      case 12: { return '#64b4f5'; }
      case 13: { return '#f38eb0'; }
      case 14: { return '#cf91da'; }
      case 15: { return '#b29cda'; }
      case 16: { return '#9da7da'; }
      case 17: { return '#bbaaa3'; }
      case 18: { return '#f4f4f4'; }
      case 19: { return '#dfdfdf'; }
      case 20: { return '#9d9d9d'; }
      default: { return '#ed9999'; }
    }
  }
}
