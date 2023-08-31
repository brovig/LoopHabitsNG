import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HabitService } from '../habits/habit.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-habit-delete-dialog',
  templateUrl: './habit-delete-dialog.component.html',
  styleUrls: ['./habit-delete-dialog.component.scss']
})
export class HabitDeleteDialogComponent {
  public id: string;

  constructor(
    private habitService: HabitService,
    private router: Router,
    public dialogRef: MatDialogRef<HabitDeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { habitId: string }) {
    this.id = data.habitId;
  }

  onNoClick() {
    this.dialogRef.close();
  }

  onYesClick() {
    this.habitService.delete(this.id).subscribe(() => {
      console.log("Habit deleted");
      this.router.navigate(['/habits']);
      this.dialogRef.close();
    }, error => console.log(error));
  }
}
