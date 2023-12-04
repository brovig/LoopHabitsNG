import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { HabitStatistics } from './habitStatistics';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  constructor(private http: HttpClient) {
  }

  getStats(habitId: string,
    endDate: Date | null): Observable<HabitStatistics> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions/stats");

    let params = new HttpParams();

    if (endDate) {
      params = params.set("endDate", endDate.toISOString().split('.')[0] + 'Z');
    }

    return this.http.get<HabitStatistics>(url, { params });
  }

  protected getUrl(url: string) {
    return environment.baseUrl + url;
  }
}
