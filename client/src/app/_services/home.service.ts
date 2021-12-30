import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  baseUrl= environment.apiUrl;
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
