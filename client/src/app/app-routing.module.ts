import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { PocComponent } from './poc/poc.component';
import { QuestionComponent } from './question/display-question/question.component';
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import { QuestionsComponent } from './question/questions/questions.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: '', component: QuestionsComponent },
 // { path: 'members', component: MemberListComponent },
  { path: 'questions', component: QuestionsComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'members/:username', component: MemberDetailComponent },
  { path: 'member-edit/:username', component: MemberEditComponent },
  { path: 'poc', component: PocComponent },
  { path: 'question/edit-question', component: EditQuestionComponent },
  { path: 'edit-question/:id', component: EditQuestionComponent },
  { path: 'question/:id', component: QuestionComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
