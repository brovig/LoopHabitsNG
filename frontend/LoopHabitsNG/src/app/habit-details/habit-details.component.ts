import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Habit } from '../habits/habit';
import { HabitService } from '../habits/habit.service';
import { ShareService } from '../share.service';
import { ColorService } from '../color.service';
import { StatisticsService } from './statistics.service';
import { Chart, registerables } from 'chart.js';
import zoomPlugin from 'chartjs-plugin-zoom';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { format } from 'date-fns';

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
  public historyDates!: string[];
  public historyValues!: number[];
  public overview = new Map<string, string>();
  public scores!: number[];
  public overviewChart!: Chart<"doughnut", number[], unknown>;
  public scoresChart!: Chart;
  public historyChart!: Chart;

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
      this.getStats();
    }, error => console.log(error));

    const d = new Date();
    this.currentDateInUTC = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), 0));
  }

  ngOnInit() {
    
  }

  getStats() {
    this.statService.getStats(this.currentHabit.id, this.currentDateInUTC).subscribe(statsResult => {
      this.overview.set('Total', statsResult.totalReps.toString());
      this.dates = statsResult.scores.scoreTimeStamps;
      this.scores = statsResult.scores.scoreValues;
      this.historyDates = statsResult.history.historyTimeStamps.map(d => this.convertDate(d));      
      this.historyValues = statsResult.history.historyValues;

      const screenWidth = window.innerWidth;
      const initialPointsAmount = Math.floor((screenWidth / 28));

      this.createOverview();
      this.createScoresChart(initialPointsAmount);
      this.createHistoryChart(initialPointsAmount);

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

  createScoresChart(pointsToShow: number) {
    const lastX = this.dates.length;    

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
            max: lastX,
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

    this.scoresChart.zoomScale('x', { min: lastX - pointsToShow, max: lastX}, "zoom");    
  }

  createHistoryChart(barsToShow: number) {
    const lastX = this.historyDates.length;

    this.historyChart = new Chart("HistoryChart", {
      type: 'bar',
      data: {
        labels: this.historyDates,
        datasets: [
          {
            data: this.historyValues,
            backgroundColor: this.habitColor,
            borderRadius: 2,
            maxBarThickness: 10,
            datalabels: {
              color: this.habitColor,
              anchor: 'end',
              align: 'top'
            }
          }
        ]
      },
      plugins: [ChartDataLabels],
      options: {
        events: [],
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          y: {
            min: 0,
            max: this.currentHabit.type == 1 ? Math.max(...this.historyValues) + Math.max(...this.historyValues) / 3 : 9,
            display: false
          },
          x: {
            max: lastX,
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
          },
          datalabels: {
            display: function (context) {
              return context.dataset.data[context.dataIndex] !== 0;
            }
          }
        }
      }
    });

    this.historyChart.zoomScale('x', { min: lastX - barsToShow, max: lastX }, "zoom"); 
  }

  convertDate(date: string) {
    const parsedDate = new Date(date);

    if (parsedDate.getMonth() === 0 && parsedDate.getDate() >= 1 && parsedDate.getDate() <= 7) {
      return format(parsedDate, 'yyyy');
    }

    if (parsedDate.getDate() >= 1 && parsedDate.getDate() <= 7) {
      return format(parsedDate, 'MMM');
    }

    return format(parsedDate, 'd');
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
