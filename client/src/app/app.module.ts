import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { QuestionComponent } from './question/display-question/question.component';
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
import { MemberEditComponent } from './members/member-edit/member-edit.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
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
    MemberEditComponent
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
    BrowserAnimationsModule,
    ReactiveFormsModule
  ],
  providers: [
    HttpClientModule,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
