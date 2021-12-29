import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl= 'https://localhost:5001/api/';

  constructor(private http:HttpClient) { }

  askQuestion(model:any){
    return this.http.post(this.baseUrl+'question/ask-question',model).pipe();
  }
}
