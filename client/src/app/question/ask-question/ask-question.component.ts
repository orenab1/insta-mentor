import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Question } from '../../_models/question';
import { QuestionService } from '../../_services/question.service';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.scss']
})
export class AskQuestionComponent implements OnInit {
  model: Question;
  id: number;
  routeSub: Subscription;
  baseUrl = environment.apiUrl;
  isNew = false;

  constructor(private questionService: QuestionService, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {

      this.id = parseInt(params['id']) || 0;

      this.isNew = this.id === 0;
      this.getQuestion();
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  addOffer(){
    this.questionService.makeOffer(this.id).subscribe(response => {
      this.reloadCurrentRoute();
    }, error => {
      alert('error');
      console.log(error);
    });
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe(response => {
      this.reloadCurrentRoute();
    }, error => {
      alert('error');
      console.log(error);
    });
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
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
          offers: [],
          askerId: 0,
          askerUsername: ''
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
