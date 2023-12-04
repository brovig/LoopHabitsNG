export interface HabitStatistics {
  scores: Scores;
  totalReps: number;
  history: History;
  dates: string[];
  calendarMarks: boolean[];
}

interface Scores {
  scoreTimeStamps: string[];
  scoreValues: number[];
}

interface History {
  historyTimeStamps: string[];
  historyValues: number[];
}
