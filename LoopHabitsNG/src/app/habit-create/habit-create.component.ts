import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { HabitService } from '../habits/habit.service';
import { Habit } from '../habits/habit';
import { MatDialog } from '@angular/material/dialog';
import { FrequencyDialogComponent } from './frequency-dialog.component';
import { ShareService } from '../share.service';

@Component({
  selector: 'app-habit-create',
  templateUrl: './habit-create.component.html',
  styleUrls: ['./habit-create.component.scss']
})
export class HabitCreateComponent implements OnInit {
  public form!: FormGroup;
  public habit!: Habit;
  public habits: Habit[] = [];
  public selectedFrequency!: string;
  public position!: number;

  constructor(
    private habitService: HabitService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog,
    private shareService: ShareService
  ) {
    this.habitService.getData().subscribe(data => {
      this.habits = data;
      this.loadData();
      this.shareService.setHabit(this.habit);
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
      // For later implementation
      // reminderTime: [''],
      // reminderDays: [''],
      description: ['']
    });
  }

  loadData() {
    const idParam = this.route.snapshot.paramMap.get('id');
    const habitFound = this.habits.find(h => h.id === idParam);
    if (habitFound) {
      this.habit = habitFound;
      this.position = this.habit.position;
    } else {
      this.initializeHabitForCreation();
      if (this.habits.length != 0) {
        const maxPosition = this.habits.reduce((prev, current) => (prev.position > current.position) ? prev : current).position;
        this.habit.position = maxPosition + 1;
      } else {
        this.habit.position = 0;
      }
      
    }
    this.selectedFrequency = this.getFreqMsg(this.habit.type, this.habit.frequencyDensity, this.habit.frequencyNumber);

    if (idParam == undefined) {
      this.habit.type = 0;
      this.form.controls['color'].patchValue(this.colors[0]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
      return;
    }

    if (idParam!.length === 1) {
      if (idParam === '1') {
        this.habit.type = 1;
        this.habit.isMeasurable = true;
      } else {
        this.form.controls['targetValue'].patchValue(0);
      }      
      this.form.controls['color'].patchValue(this.colors[0]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
      return;
    }

    if (idParam!.length > 1 && this.habit.id != '0') {
      this.form.patchValue(this.habit);
      this.form.controls['color'].patchValue(this.colors[this.habit.color]);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
    }
  }

  initializeHabitForCreation() {
    this.habit = {
      id: '-1',
      isArchived: false,
      color: 1,
      description: '',
      frequencyDensity: 1,
      frequencyNumber: 1,
      highlight: 0,
      name: '',
      position: 0,
      reminderTime: new Date(Date.UTC(1, 1, 1, 0, 0, 0, 0)),
      reminderDays: 0,
      isMeasurable: false,
      type: 0,
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
      data: { freqDen: this.habit.frequencyDensity, freqNum: this.habit.frequencyNumber },
      autoFocus: '__non_existing_element__'
    });

    dialogRef.afterClosed().subscribe(data => {
      if (!data) { return; }
      this.habit.frequencyDensity = data.freqDen;
      this.habit.frequencyNumber = data.freqNum;
      this.selectedFrequency = this.getFreqMsg(0, this.habit.frequencyDensity, this.habit.frequencyNumber);
      this.form.controls['frequency'].patchValue(this.selectedFrequency);
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
    this.habit.name = this.form.controls['name'].value;
    this.habit.color = this.colors.indexOf(this.form.controls['color'].value);
    this.habit.question = this.form.controls['question'].value;
    this.habit.unit = this.form.controls['unit'].value;
    this.habit.targetValue = this.form.controls['targetValue'].value;
    if (this.habit.type === 1) {
      switch (this.selectedFrequency) {
        case "Every day":
          this.habit.frequencyDensity = 1;
          break;
        case "Every week":
          this.habit.frequencyDensity = 7;
          break;
        case "Every month":
          this.habit.frequencyDensity = 30;
          break;
      }
    }
    this.habit.description = this.form.controls['description'].value;
    
    if (this.habit.id === '-1') {
      this.habitService.post(this.habit).subscribe(result => {
        this.router.navigate(['/habits']);
      }, error => console.log(error));
    } else {
      this.habitService.put(this.habit).subscribe(() => {
        this.router.navigate(['/habits']);
      }, error => console.log(error));
    }
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
    '#9d9d9d'
  ];

}
