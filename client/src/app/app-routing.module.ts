import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { QuestionComponent } from './question/display-question/question.component';
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import { QuestionsComponent } from './question/questions/questions.component';

const routes: Routes = [
  { path: '', component: QuestionsComponent },
 // { path: 'members', component: MemberListComponent },
  { path: 'questions', component: QuestionsComponent },
  { path: 'members/:username', component: MemberDetailComponent },
  { path: 'lists', component: ListsComponent },
 // { path: 'messages', component: MessagesComponent },
  { path: 'edit-question', component: EditQuestionComponent },
  { path: 'edit-question/:id', component: EditQuestionComponent },
  { path: 'question/:id', component: QuestionComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
