import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-frequency-dialog',
  templateUrl: './frequency-dialog.component.html',
  styleUrls: ['./frequency-dialog.component.scss']
})
export class FrequencyDialogComponent {
  public selectedFrequency!: string;
  public perDays = new FormControl('', Validators.min(1));
  public perWeek = new FormControl('', Validators.min(1));
  public perMonth = new FormControl('', Validators.min(1));

  constructor(@Inject(MAT_DIALOG_DATA) public data: { freqDen: number, freqNum: number }) {
    this.patchInputValues();
    this.preselectFrequency(data.freqDen, data.freqNum);
  }

  preselectFrequency(freqDen: number, freqNum: number) {
    switch (freqNum) {
      case 1:
        switch (freqDen) {
          case 1:
            this.selectedFrequency = '1';
            break;
          case 7:
            this.selectedFrequency = '2';
            this.perDays.patchValue("7");
            break;
          case 30:
            this.selectedFrequency = '4';
            this.perMonth.patchValue("1");
            break;
          default:
            this.selectedFrequency = "2";
            this.perDays.patchValue(freqDen.toString());
        }
        break;

      case 7:
        switch (freqDen) {
          case 7:
            this.selectedFrequency = '1';
            break;
          case 30:
            this.selectedFrequency = '4';
            this.perMonth.patchValue("7");
            break;
        }
        break;

      case 30:
        switch (freqDen) {
          case 30:
            this.selectedFrequency = '1';
            break;
        }
        break;

      default:
        switch (freqDen) {
          case 7:
            this.selectedFrequency = '3';
            this.perWeek.patchValue(freqNum.toString());
            this.perWeek.enable();
            break;
          case 30:
          case 31:
            this.selectedFrequency = '4';
            this.perMonth.patchValue(freqNum.toString());
            break;
        }
    }
  }

  patchInputValues() {
    this.perDays.patchValue("3");
    this.perWeek.patchValue("3");
    this.perMonth.patchValue("10");
  }

  onSaveClick() {
    let selectedFreqDen: number;
    let selectedFreqNum: number;

    switch (this.selectedFrequency) {
      case '1':
        selectedFreqDen = 1;
        selectedFreqNum = 1;
        break;
      case '2':
        if (Number(this.perDays.value) == 31) {
          selectedFreqDen = 30;
        } else {
          selectedFreqDen = Number(this.perDays.value);
        }        
        selectedFreqNum = 1;
        break;
      case '3':
        selectedFreqDen = 7;
        if (Number(this.perWeek.value) > 7) {
          selectedFreqNum = 7;
        } else {
          selectedFreqNum = Number(this.perWeek.value);
        }        
        break;
      case '4':
        selectedFreqDen = 30;
        if (Number(this.perMonth.value) > 30) {
          selectedFreqNum = 30;
        } else {
          selectedFreqNum = Number(this.perMonth.value);
        }         
        break;
    }

    this.data.freqDen = selectedFreqDen!;
    this.data.freqNum = selectedFreqNum!;

    if (this.data.freqDen < 1) { this.data.freqDen = 1; }
    if (this.data.freqNum < 1) { this.data.freqNum = 1; }
  }
}
