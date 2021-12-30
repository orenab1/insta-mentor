import { Component, OnInit } from '@angular/core';
import { question } from '../_models/question';
import { QuestionService } from '../_services/question.service';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.scss']
})
export class AskQuestionComponent implements OnInit {
  model: question;


  constructor(private questionService: QuestionService) {
    
  }

  ngOnInit(): void {
    this.getQuestion();
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  getQuestion() {
    this.questionService.getQuestion(2).subscribe(response => {
      this.model = response;
    }, error => {
      console.log(error);
    });
  }

  cancel() {

  }
}
