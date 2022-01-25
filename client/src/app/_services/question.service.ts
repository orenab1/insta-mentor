import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Question, QuestionSummary } from '../_models/question';
import { Comment } from '../_models/question';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl = environment.apiUrl;
 // questions: Question[] = [];

  constructor(private http: HttpClient) { }

  askQuestion(model: any) {
    return this.http.post(this.baseUrl + 'questions/ask-question', model).pipe(
      map(questionId => {
        return questionId;
      })
    );
  }

  getQuestion(id: number) {
    const serverId = id ?? 0;
    return this.http.get<Question>(this.baseUrl + 'questions/get-question/' + serverId);
  }

  getQuestions() {
    return this.http.get<QuestionSummary[]>(this.baseUrl + 'questions/questions').pipe(
      map(questions => {
     //   this.questions = questions;
        return questions;
      })
    );
  }

  postComment(model: Comment) {
    return this.http.post(this.baseUrl + 'questions/post-comment', model);
  }

  makeOffer(questionId: number) {
    var model={
      questionId:questionId
    }
    return this.http.post(this.baseUrl + 'questions/make-offer/',model);
  }
}