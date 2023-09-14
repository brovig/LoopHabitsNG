import { Component, OnInit } from '@angular/core';
import { UserForReg } from './UserForReg';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  userForReg?: UserForReg;
  form!: FormGroup;
  hidePassword = true;
  failedRegMsg = '';

  constructor(
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar) { }


  ngOnInit() {
    this.form = new FormGroup({
      username: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-+_!@#$%^&*.,?]).{10,}/)])
    })
  }

  clearFailedReg($event: KeyboardEvent) {
    if ($event.keyCode != 13) {
      this.failedRegMsg = '';
    }    
  }

  onSubmit() {
    const userForReg = <UserForReg>{};
    userForReg.username = this.form.controls['username'].value;
    userForReg.email = this.form.controls['email'].value;
    userForReg.password = this.form.controls['password'].value;

    this.authService.register(userForReg).subscribe(() => {
      const snackBarRef = this.snackBar.open('Successful registration!', 'Login', { panelClass: ['reg-snackbar'] });
      snackBarRef.onAction().subscribe(() => {
        this.router.navigate(['/login']);
      });
    }, error => {
      this.failedRegMsg = '';
      for (const property in error.error) {
        if (property === 'DuplicateUserName' || property === 'DuplicateEmail') {
          this.failedRegMsg += `${error.error[property]} \n`;
        } else {
          this.failedRegMsg = 'Registration failed';
        }        
      }
    })
  }
}
