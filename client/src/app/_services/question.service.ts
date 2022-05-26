import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { map } from 'rxjs/operators'
import { environment } from 'src/environments/environment'
import { AskerAcceptedOfferDto } from '../_models/askerAcceptedOfferDto'
import { Question, QuestionSummary, Review, Event } from '../_models/question'
import { Comment } from '../_models/question'

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) {}

  askQuestion(model: Question) {
    return this.http.post(this.baseUrl + 'questions/ask-question', model, {responseType: 'text'}).pipe(
      map((questionId:string) => {
        return questionId
      }),
    )
  }

  getQuestion(idOrGuid:any){
    if (isNaN(idOrGuid)){
      return this.getQuestionByGuid(idOrGuid);
    }else{
      return this.getQuestionById(idOrGuid);
    }
  }

  getQuestionById(id: number) {
    const serverId = id ?? 0
    return this.http.get<Question>(
      this.baseUrl + 'questions/get-question/' + serverId,
    )
  }

  getQuestionByGuid(guid: string) {
    return this.http.get<Question>(
      this.baseUrl + 'questions/get-question-by-guid/' + guid,
    )
  }

  getAllQuestions() {
    return this.http
      .get<QuestionSummary[]>(this.baseUrl + 'questions/questions')
      .pipe(
        map((questions) => {
          return questions
        }),
      )
  }

  getEvents() {
    return this.http
      .get<Event[]>(this.baseUrl + 'questions/events')
      .pipe(
        map((events) => {
          return events;
        }),
      )
  }

  getMyQuestions() {
    return this.http
      .get<QuestionSummary[]>(this.baseUrl + 'questions/my-questions')
      .pipe(
        map((questions) => {
          return questions
        }),
      )
  }

  postComment(model: Comment) {
    return this.http.post(this.baseUrl + 'questions/post-comment', model)
  }

  deleteComment(commentId: number) {
    return this.http.delete(
      this.baseUrl + 'questions/delete-comment?commentId=' + commentId,
    )
  }

  deleteOffer(offerId: number) {
    return this.http.delete(
      this.baseUrl + 'questions/delete-offer?offerId=' + offerId,
    )
  }

  makeOffer(questionId: number) {
    var model = {
      questionId: questionId,
    }
    return this.http.post(this.baseUrl + 'questions/make-offer/', model)
  }

  acceptOffer(offerId: number) {
    var model = {
      offerId: offerId,
    }
    return this.http.post<AskerAcceptedOfferDto>(this.baseUrl + 'questions/accept-offer/', model)
  }

  publishReview(model: Review) {
    return this.http.post<Review>(this.baseUrl + 'questions/publish-review/', model);
  }

  markQuestionAsSolved(questionId:number,questionGuid:string) {
    const questionIdOrGuid=questionGuid==undefined?
      questionId.toString():
      questionGuid;
    return this.http.put(this.baseUrl + 'questions/mark-question-as-solved', {questionIdOrGuid}).subscribe(    
      );
  }

  requestFeedback(questionId:number,questionGuid:string) {
    const questionIdOrGuid=questionGuid==undefined?
      questionId.toString():
      questionGuid;

    var model = {
      QuestionIdOrGuid: questionIdOrGuid,
    }
    return this.http.post(this.baseUrl + 'questions/request-feedback/', {questionIdOrGuid});
  }
}
