import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { Habit } from './habit';
import { HabitService } from './habit.service';

@Component({
  selector: 'app-habits',
  templateUrl: './habits.component.html',
  styleUrls: ['./habits.component.scss']
})
export class HabitsComponent implements OnInit {
  public displayedColumns: string[] = ['name'];
  public habits!: MatTableDataSource<Habit>;

  constructor(private habitService: HabitService) {
  }

  ngOnInit() {
    this.habitService.getData().subscribe(result => {
      this.habits = new MatTableDataSource<Habit>(result);
    }, error => console.error(error));
  }
}
