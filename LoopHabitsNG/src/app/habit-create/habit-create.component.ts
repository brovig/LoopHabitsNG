import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { HabitService } from '../habits/habit.service';
import { Habit } from '../habits/habit';
import { MatDialog } from '@angular/material/dialog';
import { FrequencyDialogComponent } from './frequency-dialog.component';

@Component({
  selector: 'app-habit-create',
  templateUrl: './habit-create.component.html',
  styleUrls: ['./habit-create.component.scss']
})
export class HabitCreateComponent implements OnInit {
  public title!: string;
  public form!: FormGroup;
  public habit!: Habit;
  public habits: Habit[] = [];
  public habitType?: number;
  public selectedFrequency!: string;

  constructor(
    private habitService: HabitService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.habitService.getData().subscribe(data => {
      this.habits = data;
      this.loadData();
    }, error => console.error(error));
  }

  ngOnInit() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      color: [''],
      question: [''],
      unit: [''],
      targetValue: ['', [Validators.required, Validators.min(0)]],
      frequency: [''],
      reminderTime: [''],
      reminderDays: [''],
      notes: ['']
    });
  }

  loadData() {
    const idParam = this.route.snapshot.paramMap.get('id');
    const habitFound = this.habits.find(h => h.id === idParam);
    if (habitFound) {
      console.log(habitFound);
      this.habit = habitFound;
    } else {
      this.initializeHabitForCreation(Number(idParam));
    }
    this.selectedFrequency = this.getFreqMsg(this.habit.type, this.habit.frequencyDensity, this.habit.frequencyNumber);

    if (idParam == undefined) {
      this.habitType = 0;
      this.title = "Create habit";
      this.form.controls['color'].patchValue(this.colors[0]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
      return;
    }

    if (idParam!.length === 1) {
      this.title = "Create habit";
      if (idParam === '1') {
        this.habitType = 1;
      } else {
        this.habitType = 0;
        this.form.controls['targetValue'].patchValue(0);
      }      
      this.form.controls['color'].patchValue(this.colors[0]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
      return;
    }

    if (idParam!.length > 1 && this.habit.id != '0') {
      this.habitType = this.habit.type;
      this.title = "Edit - " + this.habit.name;
      this.form.patchValue(this.habit);
      this.form.controls['color'].patchValue(this.colors[this.habit.color]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);

    }
  }

  initializeHabitForCreation(habitType: number) {
    this.habit = {
      id: '0',
      isArchived: false,
      color: 1,
      description: '',
      frequencyDensity: 1,
      frequencyNumber: 1,
      highlight: 0,
      name: '',
      position: habitType === 1 ? 1 : 0,
      reminderTime: new Date("0001-01-01T00:00:00"),
      reminderDays: 0,
      isMeasurable: false,
      type: habitType,
      targetType: 0,
      targetValue: 0,
      unit: '',
      question: '',
      repetitions: []
    };
  }

  getHabits() {
    this.habitService.getData().subscribe(habitsResult => {
      this.habits = habitsResult;
    }, error => console.error(error));
  }

  frequencyDialog() {
    const dialogRef = this.dialog.open(FrequencyDialogComponent, {
      data: { freqDen: this.habit.frequencyDensity, freqNum: this.habit.frequencyNumber }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
    });
  }

  getFreqMsg(habitType: number, freqDen: number, freqNum: number): string {
    let frequency: string = '';

    if (habitType == 0) {
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
        case 31:
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
    }

    if (habitType == 1) {
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
      }
    }

    return frequency;
  }

  onSubmit() {

  }

  public colors: string[] = [
    '#ed9999',
    '#feaa90',
    '#fecb7f',
    '#feebb1',
    '#69efad',
    '#c4e0a5',
    '#e5ed9a',
    '#fef49b',
    '#7fcac3',
    '#7fdde9',
    '#80d4f9',
    '#64b4f5',
    '#f38eb0',
    '#cf91da',
    '#b29cda',
    '#9da7da',
    '#bbaaa3',
    '#f4f4f4',
    '#dfdfdf',
    '#9d9d9d',
    '#ed9999'
  ];

}
