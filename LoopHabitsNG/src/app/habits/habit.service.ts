import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
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

  getData(): Observable<Habit[]> {
    const url = this.getUrl("api/habits");
    return this.http.get<Habit[]>(url);
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
    const url = this.getUrl("api/habits/");
    return this.http.post<Habit>(url, item);
  }

  delete(id: string): Observable<Habit> {
    const url = this.getUrl("api/habits/" + id);
    return this.http.delete<Habit>(url);
  }
}
