import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ColorService {
  private colorMap: ColorByIndex = {
    1: '#ed9999',
    2: '#feaa90',
    3: '#fecb7f',
    4: '#feebb1',
    5: '#69efad',
    6: '#c4e0a5',
    7: '#e5ed9a',
    8: '#fef49b',
    9: '#7fcac3',
    10: '#7fdde9',
    11: '#80d4f9',
    12: '#64b4f5',
    13: '#f38eb0',
    14: '#cf91da',
    15: '#b29cda',
    16: '#9da7da',
    17: '#bbaaa3',
    18: '#f4f4f4',
    19: '#dfdfdf',
    20: '#9d9d9d'
  };   

  getColor(value: number): string {
    return this.colorMap[value + 1];
  }
}

interface ColorByIndex {
  [key: number]: string;
}
