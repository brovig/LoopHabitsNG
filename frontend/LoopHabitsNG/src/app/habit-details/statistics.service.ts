import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Scores } from './scores';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  constructor(private http: HttpClient) {
  }

  getScores(habitId: string,
    endDate: Date | null): Observable<Scores> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions/scores");

    let params = new HttpParams();

    if (endDate) {
      params = params.set("endDate", endDate.toISOString().split('.')[0] + 'Z');
    }

    return this.http.get<Scores>(url, { params });
  }

  protected getUrl(url: string) {
    return environment.baseUrl + url;
  }
}
