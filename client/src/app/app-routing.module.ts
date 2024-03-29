import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { CommunitiesComponent } from './_components/community/communities/communities.component'
import { HomeComponent } from './home/home.component'
import { DisplayUserComponent } from './_components/user/display-user/display-user.component'
import { EditUserComponent } from './_components/user/edit-user/edit-user.component'
import { PocComponent } from './poc/poc.component'
import { QuestionComponent } from './question/display-question/display-question.component'
import { EditQuestionComponent } from './question/edit-question/edit-question.component'
import { QuestionsComponent } from './question/questions/questions.component'
import { RegisterComponent } from './register/register.component'

const routes: Routes = [
  { path: '', redirectTo:'questions', pathMatch: 'full' },
  { path: 'questions', component: QuestionsComponent },
  { path: 'questions/javascript', component: QuestionsComponent },
  { path: 'questions/software-craftsmanship', component: QuestionsComponent },
  { path: 'my-questions', component: QuestionsComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'users/:username', component: DisplayUserComponent },
  { path: 'edit-user/:username', component: EditUserComponent },
  { path: 'poc', component: PocComponent },
  { path: 'question/edit-question', component: EditQuestionComponent },
  { path: 'edit-question/:id', component: EditQuestionComponent },
  { path: 'display-question/:id', component: QuestionComponent },
  { path: 'display-question/:id/fb/user/:userid', component: QuestionComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
