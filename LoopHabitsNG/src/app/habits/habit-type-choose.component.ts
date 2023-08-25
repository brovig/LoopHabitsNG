import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-habit-type-choose',
  templateUrl: './habit-type-choose.component.html',
  styleUrls: ['./habit-type-choose.component.scss']
})
export class HabitTypeChooseComponent {
  constructor(public dialogRef: MatDialogRef<HabitTypeChooseComponent>) {}
}
