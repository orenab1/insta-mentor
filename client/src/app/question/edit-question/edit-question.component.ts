import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';
import { Question } from '../../_models/question';
import { QuestionService } from '../../_services/question.service';

@Component({
  selector: 'app-edit-question',
  templateUrl: './edit-question.component.html',
  styleUrls: ['./edit-question.component.scss']
})
export class EditQuestionComponent implements OnInit {
  model: Question;
  id: number = 0;
  routeSub: Subscription;
  baseUrl = environment.apiUrl;
  isNew = false;
  shouldDisplayComments = true;
  currentUserUsername: string;
  isCurrentUserQuestionOwner: boolean;
  isInEditMode = false;


  constructor(
    private questionService: QuestionService,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.currentUserUsername = user.username);
  }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {

      this.id = parseInt(params['id']) || 0;

      if (this.id === 0) {
        this.isInEditMode = true;
        this.isNew = true;
      }

      this.getQuestion();
    });
  }

  toggleDisplayComments() {
    this.shouldDisplayComments = !this.shouldDisplayComments;
  }


  getQuestion() {
    this.questionService.getQuestion(this.id).subscribe(response => {
      if (response == null) {
        this.model = {
          id: 0,
          header: '',
          body: '',
          isSolved: false,
          comments: [],
          offers: [],
          askerId: 0,
          askerUsername: '',
          photoId: 0,
          photoUrl: ''
        };
      } else {
        this.model = response;

        this.isCurrentUserQuestionOwner =
          this.model.askerUsername === this.currentUserUsername;

      }
    }, error => {
      console.log(error);
    });
  }



  cancel() {
    this.reloadCurrentRoute();
  }


  onUploadPhotoSuccess(response: any) {
    if (response) {
      const photo = JSON.parse(response);
      this.model.photoUrl = photo.url;
      this.model.photoId = photo.id;
    }
  }

  deletePhoto() {
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe({
      next: (questionId) => this.router.navigateByUrl('question/' + questionId),
      error: (error) => console.log(error)
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }



  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
}
