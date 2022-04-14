import { Component, Input, OnInit, Output } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Subscription, take } from 'rxjs'
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
  routeSub: Subscription

  constructor(private accountService: AccountService, private router: Router,private route: ActivatedRoute) {
    this.model = {
      email: '',
      password: '',
      verificationCode:''
    }

    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user))
  }

  ngOnInit(): void {
    this.routeSub = this.route.queryParams.subscribe((params) => {
      if (params['verificationCode']){
        console.log(this.model);
      this.setSignInMode();
      this.model.verificationCode=params['verificationCode'];
      this.model.email=params['email'];
      this.model.password='';
      }
      
    });

  }

  login() {
    this.accountService.login(this.model).subscribe(
      (response) => {
        this.router.navigateByUrl('/questions')

      
      },
      (error) => {
        this.handleErrorMessage(error.error);
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
        this.handleErrorMessage(error.error);
      },
    )
  }

  register() {
    this.accountService.register(this.model).subscribe(
      (response) => {
        this.errorMessage='We have sent an email with a confirmation link to your email address.<br/>In order to complete the registration process, please click the confirmation link. <br/>If you do not receive a confirmation email,<br/>please check your spam folder.'
      },
      (error) => {
        this.handleErrorMessage(error.error)
      },
    )
  }

  handleErrorMessage(errorMessage:string){
    if (errorMessage.startsWith('Your') || errorMessage.startsWith('Email') || errorMessage.startsWith('Something')){
      this.errorMessage = errorMessage;
      } else{
        this.errorMessage='Something went wrong. Please try again later, or contact us at oren@vidcallme.com';
        console.log(errorMessage)
      }    
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

  setSignInMode(){
    this.formMode = 'signin';
    this.hasForgotPassword = false;
    this.errorMessage ='';
  }
}
