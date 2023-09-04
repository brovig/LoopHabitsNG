import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { TokenDto } from './TokenDto';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { UserForAuth } from './UserForAuth';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  title?: string;
  tokenDto?: TokenDto;
  form!: FormGroup;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    })
  }
  
  getErrorMessage(formControl: FormControl) {
    if (formControl.hasError('required')) {
      return 'Cannot be blank';
    }

    if (formControl.hasError('email')) {
      return 'Not a valid email';
    }

    return '';
  }

  onSubmit() {
    const userForAuth = <UserForAuth>{};
    userForAuth.email = this.form.controls['email'].value;
    userForAuth.password = this.form.controls['password'].value;

    this.authService.login(userForAuth).subscribe(result => {
      if (result) {
        console.log('login successful');
        console.log(result);
        this.router.navigate(['/']);
      }
    }, error => console.log(error));
  }

  cancel(event: Event) {
    event.preventDefault();
    this.router.navigate(['/']);
  }
}
