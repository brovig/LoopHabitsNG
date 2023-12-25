import { Repetition } from "./repetition";

export interface Habit {
  id: string;
  isArchived: boolean;
  color: number;
  description: string;
  frequencyDensity: number;
  frequencyNumber: number;
  highlight: number;
  name: string;
  position: number;
  reminderTime: Date;
  reminderDays: number;
  isMeasurable: boolean;
  type: number;
  targetType: number;
  targetValue: number;
  unit: string;
  question: string;
  colorString: string;
  repetitions: Repetition[];
}
