import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { question } from '../_models/question';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token,
    'Content-Type': 'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl= environment.apiUrl;

  constructor(private http:HttpClient) { }

  askQuestion(model:any){
    return this.http.post(this.baseUrl+'questions/ask-question',model);
  }

  getQuestion(id:number){
    const serverId=id?? 0;
    return this.http.get<question>(this.baseUrl+'questions/'+serverId, httpOptions);
  }

  postComment(model:Comment){
    return this.http.post(this.baseUrl+'questions/postComment',model);
  }
}
