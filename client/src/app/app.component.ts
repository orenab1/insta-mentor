import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { PresenceService } from './_services/signalR/presence.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GoogleLoginProvider, SocialAuthService,SocialUser  } from 'angularx-social-login';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'insta mentor';
  users: any;

  
  loginForm: FormGroup;
  socialUser: SocialUser;
  isLoggedin: boolean;  


  constructor(private accountService: AccountService, private presenceService: PresenceService,
    private formBuilder: FormBuilder, 
    private socialAuthService: SocialAuthService) { }


  ngOnInit() {
    this.setCurrentUser();

    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });    

    
    // this.socialAuthService.authState.subscribe((user) => {
    //   this.socialUser = user;
    //  // this.isLoggedin = (user != null);
    //   console.log(this.socialUser);
    // });
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);

    if (user) {
      this.accountService.setCurrentUser(user);
      this.presenceService.createHubConnection(user);
    }
  }

  // loginWithGoogle(): void {
  //   this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  // }

  // logOut(): void {
  //   this.socialAuthService.signOut();
  // }
}

