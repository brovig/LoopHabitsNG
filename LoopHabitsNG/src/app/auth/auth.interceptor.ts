import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "./auth.service";
import { BehaviorSubject, Observable, catchError, filter, switchMap, take, throwError } from "rxjs";
import { TokenDto } from "./TokenDto";

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
  private accessToken?: string | null;
  private refreshToken?: string | null;
  
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(
    private authService: AuthService,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.accessToken = this.authService.getAccessToken();    

    if (this.accessToken) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.accessToken}`
        }
      });
    }
    return next.handle(req).pipe(catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401 && req.url !== 'api/authorization/refresh') {
        return this.handle401Error(req, next);
      }
      return throwError(error);
    }));
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.refreshToken = this.authService.getRefreshToken();
      const tokenDto = <TokenDto>{};      

      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      if (this.refreshToken && this.accessToken) {
        tokenDto.accessToken = this.accessToken;
        tokenDto.refreshToken = this.refreshToken;
        return this.authService.refresh(tokenDto).pipe(switchMap((token: any) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(token.accessToken);

          return next.handle(request.clone({
            setHeaders: {
              Authorization: `Bearer ${token.accessToken}`
            }
          }));
        }), catchError((err) => {
          this.isRefreshing = false;
          return throwError(err);
        }));
      }
    }
    return this.refreshTokenSubject.pipe(
      filter(token => token != null),
      take(1),
      switchMap((accessToken) => next.handle(request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      })))
    )

  }
}
