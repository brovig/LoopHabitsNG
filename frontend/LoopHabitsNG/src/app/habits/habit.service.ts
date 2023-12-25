import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';
import { BaseService} from '../base.service';
import { Observable } from 'rxjs';

import { Habit } from './habit';

@Injectable({
  providedIn: 'root',
})
export class HabitService extends BaseService<Habit> {
  constructor(http: HttpClient) {
    super(http);
  }

  getData(startDate: Date | null,
          endDate: Date | null): Observable<Habit[]> {
    const url = this.getUrl("api/habits");

    let params = new HttpParams();

    if (startDate) {
      params = params.set("startDate", startDate.toISOString().split('.')[0] + 'Z');
    }
    if (endDate) {
      params = params.set("endDate", endDate.toISOString().split('.')[0] + 'Z');
    }

    return this.http.get<Habit[]>(url, { params });
  }

  get(id: string): Observable<Habit> {
    const url = this.getUrl("api/habits/" + id);
    return this.http.get<Habit>(url);
  }

  put(item: Habit): Observable<Habit> {
    const url = this.getUrl("api/habits/" + item.id);
    return this.http.put<Habit>(url, item);
  }

  post(item: Habit): Observable<Habit> {
    const url = this.getUrl("api/habits");
    return this.http.post<Habit>(url, item);
  }

  delete(id: string): Observable<Habit> {
    const url = this.getUrl("api/habits/" + id);
    return this.http.delete<Habit>(url);
  }
}
