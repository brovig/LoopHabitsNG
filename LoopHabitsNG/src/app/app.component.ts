import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './auth/auth.service';
import { ConnectionService, ConnectionState } from 'ng-connection-service';
import { Subscription, tap } from 'rxjs';
import { Router } from '@angular/router';
import { ShareService } from './share.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Habits';

  currentState!: ConnectionState;
  isOnline: boolean = true;
  subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private connectionService: ConnectionService,
    private router: Router,
    private shareService: ShareService) { }

  ngOnInit() {
    this.authService.init();
    this.shareService.init();

    this.subscription.add(
      this.connectionService.monitor().pipe(
        tap((newState: ConnectionState) => {
          this.currentState = newState;

          if (this.currentState.hasNetworkConnection && this.currentState.hasInternetAccess) {
            this.isOnline = true;
          } else {
            this.isOnline = false;
          }
        })
      ).subscribe()
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
