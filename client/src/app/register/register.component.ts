import { Component, Input, OnInit, Output } from '@angular/core'
import { Router } from '@angular/router'
import { take } from 'rxjs'
import { Register } from '../_models/register'
import { AccountService } from '../_services/account.service'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  model: Register
  shouldDisplayPassword: boolean = false
  formMode: string = 'register'
  passwordIsValid = true
  hasForgotPassword = false
  user: any = {}
  errorMessage: string = ''

  constructor(private accountService: AccountService, private router: Router) {
    this.model = {
      email: 'orenab1@gmail.com',
      password: 'password1',
    }

    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user))
  }

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.model).subscribe(
      (response) => {
        setTimeout(() => {
          this.router.navigateByUrl('/questions')
        }, 500)
      },
      (error) => {
        console.log(error)
        this.errorMessage = error.error;
      },
    )
  }

  submitForm() {
    if (this.formMode == 'register') {
      if (
        this.model.password.search(/\d/) == -1 ||
        this.model.password.search(/[a-zA-Z]/) == -1 ||
        this.model.password.length < 6
      ) {
        this.passwordIsValid = false
        return
      }
      this.register()
    } else {
      if (this.hasForgotPassword) {
        this.forgotPassword();
      } else {
        this.login()
      }
      this.passwordIsValid = true
    }
  }

  forgotPassword(){
    this.accountService.forgotPassword(this.model.email).subscribe(
      (response) => {
       
      },
      (error) => {
        this.errorMessage = error.error;
      },
    )
  }

  register() {
    this.accountService.register(this.model).subscribe(
      (response) => {
        setTimeout(() => {
          this.router.navigateByUrl('/questions')
        }, 500)
      },
      (error) => {
        console.log(error);
        this.errorMessage = error.error;
      },
    )
  }

  toggleDisplayPassword() {
    this.shouldDisplayPassword = !this.shouldDisplayPassword
  }

  toggleRegisterMode(mode: string) {
    this.formMode = mode;
    this.hasForgotPassword = false;
    this.errorMessage ='';
  }

  toggleForgotPassword() {
    this.hasForgotPassword = !this.hasForgotPassword;
    this.model.password = '';
    this.errorMessage ='';
  }
}
