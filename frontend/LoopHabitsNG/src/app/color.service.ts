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

  private colorMap2: IndexByColor = {
    '#ed9999': 1, 
    '#feaa90': 2, 
    '#fecb7f': 3, 
    '#feebb1': 4, 
    '#69efad': 5, 
    '#c4e0a5': 6, 
    '#e5ed9a': 7, 
    '#fef49b': 8, 
    '#7fcac3': 9, 
    '#7fdde9': 10,
    '#80d4f9': 11,
    '#64b4f5': 12,
    '#f38eb0': 13,
    '#cf91da': 14,
    '#b29cda': 15,
    '#9da7da': 16,
    '#bbaaa3': 17,
    '#f4f4f4': 18,
    '#dfdfdf': 19,
    '#9d9d9d': 20
  }

  getColor(value: number): string {
    return this.colorMap[value + 1];
  }

  getColorNumber(value: string): number {
    return this.colorMap2[value] - 1;
  }

  getAllColors(): string[] {
    return Object.values(this.colorMap);
  }
}

interface ColorByIndex {
  [key: number]: string;
}

interface IndexByColor {
  [key: string]: number;
}
