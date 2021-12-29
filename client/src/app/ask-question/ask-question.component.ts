import { Component, OnInit } from '@angular/core';
import { QuestionService } from '../_services/question.service';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.scss']
})
export class AskQuestionComponent implements OnInit {
  model:any={};


  constructor(private questionService:QuestionService) {
    
   }

  ngOnInit(): void {
  }

  askQuestion(){
    this.questionService.askQuestion(this.model).subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    });
  }

  cancel(){

  }
}
