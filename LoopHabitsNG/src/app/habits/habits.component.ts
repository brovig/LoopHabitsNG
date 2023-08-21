import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { Habit } from './habit';
import { HabitService } from './habit.service';
import { RepetitionService } from './repetition.service';
import { Repetition } from './repetition';

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

  constructor(private habitService: HabitService,
    private repetitionService: RepetitionService) {
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
    this.numberOfDaysToDisplay = Math.floor((this.screenWidth - 100) / 65);

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
}
