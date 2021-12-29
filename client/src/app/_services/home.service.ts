import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  baseUrl= 'https://localhost:5001/api/';
  questions:any;

  constructor(private http:HttpClient) {
    this.getQuestions();
   }

  getQuestions(){
    this.http.get(this.baseUrl+'home/getquestions').subscribe(response => {
      this.questions = response;
    }, error => {
      console.log(error);
    })
  }
}
