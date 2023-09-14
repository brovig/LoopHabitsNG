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
  failedLogMsg = '';

  constructor(
    private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    })
  }

  clearFailedLog($event : KeyboardEvent) {
    if ($event.keyCode != 13) {
      this.failedLogMsg = '';
    }    
  }

  onSubmit() {
    const userForAuth = <UserForAuth>{};
    userForAuth.email = this.form.controls['email'].value;
    userForAuth.password = this.form.controls['password'].value;

    this.authService.login(userForAuth).subscribe(result => {
      if (result) {
        this.router.navigate(['/']);
      }
    }, () => {
      this.failedLogMsg = 'Login failed'; 
    });
  }
}
