import { Component, OnInit } from '@angular/core';
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

  getFreqMsg(freqDen: number, freqNum: number): string {
    let frequency: string = '';

    switch (freqNum) {

      case 1:
        switch (freqDen) {
          case 1:
            frequency = 'Every day';
            break;
          case 7:
            frequency = 'Every week';
            break;
          case 30:
            frequency = 'Every month';
            break;
          default:
            frequency = 'Every ' + freqDen + ' days';
        }
        break;

      case 7:
        switch (freqDen) {
          case 7:
            frequency = 'Every day';
            break;
          case 30:
            frequency = '7 times per month';
            break;
        }
        break;

      case 30:
        switch (freqDen) {
          case 30:
            frequency = 'Every day';
            break;
        }
        break;

      default:
        switch (freqDen) {
          case 7:
            frequency = freqNum + ' times per week';
            break;
          case 30:
            frequency = freqNum + ' times per month';
            break;
        }
    }
    
    return frequency;
  }
}
