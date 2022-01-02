import { Component, OnInit } from '@angular/core';
import { question } from 'src/app/_models/question';
import { QuestionService } from 'src/app/_services/question.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss']
})
export class QuestionsComponent implements OnInit {
  questions: question[];

  constructor(private questionService: QuestionService) { }

  ngOnInit(): void {
  }

  loadQuestions(): void {
    this.questionService.getQuestions().subscribe(response => {
      this.questions = response;
    }, error => {
      alert('error');
      console.log(error);
    });
  }

}
