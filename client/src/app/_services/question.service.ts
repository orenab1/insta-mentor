import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { question } from '../_models/question';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl= environment.apiUrl;

  constructor(private http:HttpClient) { }

  askQuestion(model:any){
    return this.http.post(this.baseUrl+'question/ask-question',model).pipe();
  }

  getQuestion(id:number){
    return this.http.get<question>(this.baseUrl+'questions/'+id, httpOptions);
  }
}
