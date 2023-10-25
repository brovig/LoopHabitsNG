import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Habit } from '../habits/habit';
import { HabitService } from '../habits/habit.service';
import { ShareService } from '../share.service';
import { StatisticsService } from './statistics.service';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-habit-details',
  templateUrl: './habit-details.component.html',
  styleUrls: ['./habit-details.component.scss']
})
export class HabitDetailsComponent implements OnInit {
  public currentHabit!: Habit;
  public currentDateInUTC: Date;
  public dates!: string[];
  public scores!: number[];
  public scoresChart!: Chart;

  constructor(
    private route: ActivatedRoute,
    private habitService: HabitService,
    private statService: StatisticsService,
    private shareService: ShareService
  ) {
    Chart.register(...registerables);
    const id = this.route.snapshot.params['id'];
    this.habitService.get(id).subscribe(data => {
      this.currentHabit = data;
      this.shareService.setHabit(data);
      this.getScores();
    }, error => console.log(error));

    const d = new Date();
    this.currentDateInUTC = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), 0));
  }

  ngOnInit() {
    
  }

  getScores() {
    this.statService.getScores(this.currentHabit?.id, this.currentDateInUTC).subscribe(scoresResult => {
      this.dates = scoresResult.timeStamps;
      this.scores = scoresResult.values;
      this.createScoresChart();
    }, error => console.error(error));
  }

  createScoresChart() {
    this.scoresChart = new Chart("ScoresChart", {
      type: 'line',
      data: {
        labels: this.dates,
        datasets: [
          {
            data: this.scores
          }
        ]
      },
      options: {
        aspectRatio: 2.5
      }
    });
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
