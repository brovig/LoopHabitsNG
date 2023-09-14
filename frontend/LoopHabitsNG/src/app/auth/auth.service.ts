import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, tap, throwError } from 'rxjs';
import { environment } from '../../environments/environment';

import { UserForAuth } from './UserForAuth';
import { TokenDto } from './TokenDto';
import { UserForReg } from './UserForReg';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private accessTokenKey: string = "accessToken";
  private refreshTokenKey: string = "refreshToken";

  private _authStatus = new Subject<boolean>();
  public authStatus = this._authStatus.asObservable();

  constructor(protected http: HttpClient) { }

  isAuthenticated(): boolean {
    return this.getAccessToken() !== null;
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  init() {
    if (this.isAuthenticated()) {
      this.setAuthStatus(true);
    }
  }

  register(item: UserForReg): Observable<UserForReg> {
    const url = environment.baseUrl + "api/authentication";
    return this.http.post<UserForReg>(url, item);
  }

  login(item: UserForAuth): Observable<TokenDto> {
    const url = environment.baseUrl + "api/authentication/login";
    return this.http.post<TokenDto>(url, item)
      .pipe(        
        tap(result => {
          if (result && result.accessToken && result.refreshToken) {
            localStorage.setItem(this.accessTokenKey, result.accessToken);
            localStorage.setItem(this.refreshTokenKey, result.refreshToken);
            this.setAuthStatus(true);
          }
        }),
        catchError(err => {
          return throwError(err);
        })
      );
  }

  refresh(item: TokenDto): Observable<TokenDto> {
    const url = environment.baseUrl + "api/authentication/refresh";
    return this.http.post<TokenDto>(url, item)
      .pipe(tap(result => {
        if (result && result.accessToken && result.refreshToken) {
          localStorage.setItem(this.accessTokenKey, result.accessToken);
          localStorage.setItem(this.refreshTokenKey, result.refreshToken);
          this.setAuthStatus(true);
        }
      }));
  }

  logout() {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.setAuthStatus(false);
  }

  private setAuthStatus(isAuthenticated: boolean) {
    this._authStatus.next(isAuthenticated);
  }
}
