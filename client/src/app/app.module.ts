import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { DisplayUserComponent } from './_components/user/display-user/display-user.component';
import { MessagesComponent } from './messages/messages.component';
import { QuestionComponent } from './question/display-question/display-question.component';
import { CommentsComponent } from './question/comments/comments.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { QuestionsComponent } from './question/questions/questions.component';
import { OffersComponent } from './question/offers/offers.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { ReviewComponent } from './review/review.component';
import { RatingModule } from 'ngx-bootstrap/rating';


import { RatingBasicComponent } from './rating-basic/rating-basic.component';
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import { PocComponent } from './poc/poc.component'

import { TagInputModule } from 'ngx-chips';
import { EditUserComponent } from './_components/user/edit-user/edit-user.component';
import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';
import { ToastrModule } from 'ngx-toastr';
import { NgxUiLoaderModule, NgxUiLoaderRouterModule } from 'ngx-ui-loader';
import { CommunitiesComponent } from './_components/community/communities/communities.component';
import { DisplayCommunityComponent } from './_components/community/display-community/display-community.component';
import { AddCommunityComponent } from './_components/community/add-community/add-community.component';
import { NgToggleModule } from 'ng-toggle-button';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { RouterModule, ROUTES } from '@angular/router';
import { DisplayUserSummaryComponent } from './_components/user/display-user-summary/display-user-summary.component';
import { DisplayUsernameComponent } from './_components/user/display-username/display-username.component';
import { DisplayUserImageComponent } from './_components/user/display-user-image/display-user-image.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    DisplayUserComponent,
    MessagesComponent,
    QuestionComponent,
    CommentsComponent,
    QuestionsComponent,
    OffersComponent,
    PhotoEditorComponent,
    ReviewComponent,
    RatingBasicComponent,
    EditQuestionComponent,
    PocComponent,
    EditUserComponent,
    CommunitiesComponent,
    DisplayCommunityComponent,
    AddCommunityComponent,
    DisplayUserSummaryComponent,
    DisplayUsernameComponent,
    DisplayUserImageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    RatingModule.forRoot(),
    FileUploadModule,
    TagInputModule,    
    ReactiveFormsModule,
    SocialLoginModule,
    ToastrModule.forRoot(),
    NgxUiLoaderModule,
    NgxUiLoaderRouterModule,
    NgToggleModule,
    TabsModule.forRoot(),
    RouterModule.forRoot([],{scrollPositionRestoration: 'enabled'}),
  ],
  providers: [
    BsModalService,
    HttpClientModule,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '155014635586-ir4obp64jsr2do7e2cdnh0msauq11lbo.apps.googleusercontent.com'
            )
          }
        ]
      } as SocialAuthServiceConfig,
    } 
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
