import { Component, OnInit } from '@angular/core';
import { Question, QuestionSummary } from 'src/app/_models/question';
import { QuestionService } from 'src/app/_services/question.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss']
})
export class QuestionsComponent implements OnInit {
  questions: QuestionSummary[];

  constructor(private questionService: QuestionService) { }

  ngOnInit(): void {
    this.loadQuestions();
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
