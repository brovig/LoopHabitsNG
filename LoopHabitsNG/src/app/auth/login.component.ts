import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { UserForAuth } from './UserForAuth';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  hidePassword = true;
  failedLoginAttempt = false;

  constructor(
    private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    })
  }

  onSubmit() {
    const userForAuth = <UserForAuth>{};
    userForAuth.email = this.form.controls['email'].value;
    userForAuth.password = this.form.controls['password'].value;

    this.authService.login(userForAuth).subscribe(result => {
      if (result) {
        this.failedLoginAttempt = true;
        this.router.navigate(['/']);
      }
    }, error => {
      if (error.status === 401) {
        this.failedLoginAttempt = true;
      }
    });
  }

  //cancel(event: Event) {
  //  event.preventDefault();
  //  this.router.navigate(['/']);
  //}
}
