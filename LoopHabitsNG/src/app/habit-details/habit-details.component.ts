import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Habit } from '../habits/habit';
import { HabitService } from '../habits/habit.service';

@Component({
  selector: 'app-habit-details',
  templateUrl: './habit-details.component.html',
  styleUrls: ['./habit-details.component.scss']
})
export class HabitDetailsComponent {
  public currentHabit?: Habit;

  constructor(
  private route: ActivatedRoute,
  private router: Router,
  private habitService: HabitService) {
    const id = this.route.snapshot.params['id'];
    this.habitService.get(id).subscribe(data => {
      this.currentHabit = data;
    }, error => console.log(error));
  }
}
