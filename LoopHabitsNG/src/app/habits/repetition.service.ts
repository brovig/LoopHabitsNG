import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService} from '../base.service';
import { Observable } from 'rxjs';

import { Repetition } from './repetition';

@Injectable({
  providedIn: 'root',
})
export class RepetitionService extends BaseService<Repetition> {
  constructor(http: HttpClient) {
    super(http);
  }

  getData(habitId: string,
    startDate: Date | null,
    endDate: Date | null): Observable<Repetition[]> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions");

    let params = new HttpParams();

    if (startDate) {
      params = params.set("startDate", startDate.toISOString().split('.')[0] + 'Z');
    }
    if (endDate) {
      params = params.set("endDate", endDate.toISOString().split('.')[0] + 'Z');
    }

    return this.http.get<Repetition[]>(url, {params});
  }

  get(habitId: string,
    id: string): Observable<Repetition> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions/" + id);
    return this.http.get<Repetition>(url);
  }

  put(habitId: string,
    item: Repetition): Observable<Repetition> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions/" + item.id);
    return this.http.put<Repetition>(url, item);
  }

  post(habitId: string,
    item: Repetition): Observable<Repetition> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions");
    return this.http.post<Repetition>(url, item);
  }

  delete(habitId: string,
    id: string): Observable<Repetition> {
    const url = this.getUrl("api/habits/" + habitId + "/repetitions/" + id);
    return this.http.delete<Repetition>(url);
  }
}
