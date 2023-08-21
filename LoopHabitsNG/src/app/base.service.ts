import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export abstract class BaseService<T> {
  constructor(
    protected http: HttpClient) {
  }

  //abstract getData(): Observable<T[]>;
  //abstract get(id: number | string): Observable<T>;
  //abstract put(item: T): Observable<T>;
  //abstract post(item: T): Observable<T>;
  //abstract delete(id: number | string): Observable<T>;

  protected getUrl(url: string) {
    return environment.baseUrl + url;
  }
}
