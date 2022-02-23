import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Subscription } from 'rxjs'
import { take } from 'rxjs/operators'
import { AccountService } from 'src/app/_services/account.service'
import { environment } from 'src/environments/environment'
import { Question } from '../../_models/question'
import { QuestionService } from '../../_services/question.service'

@Component({
  selector: 'app-display-question',
  templateUrl: './display-question.component.html',
  styleUrls: ['./display-question.component.scss'],
})
export class QuestionComponent implements OnInit {
  model: Question
  id: number = 0
  routeSub: Subscription
  baseUrl = environment.apiUrl
  isNew = false
  shouldDisplayComments = true
  currentUserUsername: string
  isCurrentUserQuestionOwner: boolean
  isInEditMode = false
  communitiesAsString: string
  lengthAsString: string

  constructor(
    private questionService: QuestionService,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.currentUserUsername = user.username))
  }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe((params) => {
      const questionIdFromUrl = parseInt(params['id']) || 0

      if (questionIdFromUrl === 0) {
        this.isInEditMode = true
        this.isNew = true
      }

      this.getQuestion(questionIdFromUrl)
    })
  }

  toggleDisplayComments() {
    this.shouldDisplayComments = !this.shouldDisplayComments
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe()
  }

  addOffer() {
    this.questionService.makeOffer(this.model.id).subscribe({
      next: () => this.reloadCurrentRoute(),
      error: (error) => console.log(error),
    })
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe({
      next: () => this.reloadCurrentRoute(),
      error: (error) => console.log(error),
    })
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl])
    })
  }

  getQuestion(id: number) {
    this.questionService.getQuestion(id).subscribe(
      (response) => {
        if (response != null) {
          this.model = response
          this.communitiesAsString = this.model.communities
            .map((item) => item.display)
            .join(', ')

          switch (this.model.length) {
            case 1:
              this.lengthAsString = 'less than 5 minutes'
              break
            case 2:
              this.lengthAsString = 'less than 15 minutes'
              break
            case 3:
              this.lengthAsString = 'more than 15 minutes'
              break
          }
          this.isCurrentUserQuestionOwner =
            this.model.askerUsername === this.currentUserUsername
        }
      },
      (error) => {
        console.log(error)
      },
    )
  }

  edit() {
    this.router.navigateByUrl('edit-question/' + this.model.id)
  }

  toggleSolved() {
    this.model.isSolved = !this.model.isSolved
  }
}
