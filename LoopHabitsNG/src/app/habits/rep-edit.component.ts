import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-rep-edit',
  templateUrl: './rep-edit.component.html',
  styleUrls: ['./rep-edit.component.scss']
})
export class RepEditComponent implements OnInit {
  public repValue: number;
  public repValueControl!: FormControl;
  form!: FormGroup;

  constructor(public dialogRef: MatDialogRef<RepEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number) {
    this.repValue = data;    
  }

  ngOnInit() {
    this.form = new FormGroup({
      value: new FormControl('', [Validators.min(0)])
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
