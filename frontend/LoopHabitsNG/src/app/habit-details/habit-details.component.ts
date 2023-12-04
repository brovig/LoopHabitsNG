import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Habit } from '../habits/habit';
import { HabitService } from '../habits/habit.service';
import { ShareService } from '../share.service';
import { ColorService } from '../color.service';
import { StatisticsService } from './statistics.service';
import { Chart, registerables } from 'chart.js';
import zoomPlugin from 'chartjs-plugin-zoom';

@Component({
  selector: 'app-habit-details',
  templateUrl: './habit-details.component.html',
  styleUrls: ['./habit-details.component.scss']
})
export class HabitDetailsComponent implements OnInit {
  public currentHabit!: Habit;
  public habitColor!: string;
  public currentDateInUTC: Date;
  public dates!: string[];
  public overview = new Map<string, string>();
  public scores!: number[];
  public scoresChart!: Chart;
  public overviewChart!: Chart<"doughnut", number[], unknown>;

  constructor(
    private route: ActivatedRoute,
    private habitService: HabitService,
    private statService: StatisticsService,
    private shareService: ShareService,
    private colorService: ColorService
  ) {
    Chart.register(...registerables, zoomPlugin);
    const id = this.route.snapshot.params['id'];
    this.habitService.get(id).subscribe(data => {
      this.currentHabit = data;
      this.habitColor = this.colorService.getColor(data.color);
      this.shareService.setHabit(data);
      this.getScores();
    }, error => console.log(error));

    const d = new Date();
    this.currentDateInUTC = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), 0));
  }

  ngOnInit() {
    
  }

  getScores() {
    this.statService.getStats(this.currentHabit.id, this.currentDateInUTC).subscribe(statsResult => {
      this.dates = statsResult.scores.scoreTimeStamps;
      this.scores = statsResult.scores.scoreValues;
      this.overview.set('Total', statsResult.totalReps.toString());
      this.createOverview();
      this.createScoresChart();
    }, error => console.error(error));
  }

  createOverview() {
    const scoresTotal = this.scores.length;
    const currentScore = this.scores[scoresTotal - 1];

    this.overviewChart = new Chart("OverviewChart", {
      type: 'doughnut',
      data: {
        datasets: [
          {
            data: [currentScore, 100 - currentScore],
            backgroundColor: [
              this.habitColor,
              'grey'
            ],
            borderWidth: 0
          }
        ]
      },
      options: {
        events: [],
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%'
      }
    });      

    this.overview.set('Score', Math.round(currentScore) + '%');

    const valueMonth = scoresTotal > 31 ? Math.round(currentScore - this.scores[scoresTotal - 31]) : Math.round(currentScore);
    if (scoresTotal > 31) {
      const change = valueMonth > 0 ? '+' + valueMonth : valueMonth;
      this.overview.set('Month', change + '%');
    } else {
      this.overview.set('Month', '+' + valueMonth + '%');
    }

    const valueYear = scoresTotal > 365 ? Math.round(currentScore - this.scores[scoresTotal - 366]) : Math.round(currentScore);
    if (scoresTotal > 365) {      
      const change = valueYear > 0 ? '+' + valueYear : valueYear;
      this.overview.set('Year', change + '%');
    } else {
      this.overview.set('Year', '+' + valueYear + '%');
    }
  }

  createScoresChart() {
    const maxX = this.dates.length;
    const screenWidth = window.innerWidth;
    const initialPointsAmount = Math.floor((screenWidth / 28));

    this.scoresChart = new Chart("ScoresChart", {
      type: 'line',
      data: {
        labels: this.dates,
        datasets: [
          {
            data: this.scores,
            pointBackgroundColor: this.habitColor,
            pointBorderColor: this.habitColor,
            borderColor: this.habitColor
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        color: this.habitColor,
        scales: {
          y: {
            min: 0,
            max: 100
          },
          x: {
            max: maxX,
            ticks: {
              maxRotation: 0
            }
          }
        },
        plugins: {
          legend: {
            display: false
          },
          zoom: {
            limits: {
              x: { minRange: 10 }
            },
            pan: {
              enabled: true,
              mode: 'x',
              scaleMode: 'x'
            },
            zoom: {
              wheel: {
                enabled: true
              },
              pinch: {
                enabled: true
              },
              mode: 'x'
            }            
          }
        }
      }     
    });          

    this.scoresChart.zoomScale('x', { min: maxX - initialPointsAmount, max: maxX}, "zoom");    
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
