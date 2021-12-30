import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { question } from '../../_models/question';
import { QuestionService } from '../../_services/question.service';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.scss']
})
export class AskQuestionComponent implements OnInit {
  model: question;
  id: number;
  routeSub: Subscription;
  baseUrl = environment.apiUrl;
  // ={
  //   id:0,
  //   header:'',
  //   body:'',
  //   comments:[],
  //   offers:[]
  // };


  constructor(private questionService: QuestionService, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.getQuestion();
    this.routeSub = this.route.params.subscribe(params => {

      this.id = parseInt(params.get('id'));
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe(response => {
      alert('/questions/' + response);
      this.router.navigate(['/ask-question/' + response]);
    }, error => {
      alert('error');
      console.log(error);
    });



  }

  getQuestion() {
    this.questionService.getQuestion(this.id).subscribe(response => {
      if (response == null) {
        this.model = {
          id: 0,
          header: '',
          body: '',
          comments: [],
          offers: []
        };
      } else {
        this.model = response;
      }
    }, error => {
      console.log(error);
    });
  }

  cancel() {

  }
}
