import { Component, OnInit, TemplateRef } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Subscription } from 'rxjs'
import { take } from 'rxjs/operators'
import { AccountService } from 'src/app/_services/account.service'
import { environment } from 'src/environments/environment'
import { Question } from '../../_models/question'
import { Review } from '../../_models/question'
import { QuestionService } from '../../_services/question.service'
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal'

@Component({
  selector: 'app-display-question',
  templateUrl: './display-question.component.html',
  styleUrls: ['./display-question.component.scss'],
})
export class QuestionComponent implements OnInit {
  modalRef?: BsModalRef
  bsModalRefReview?: BsModalRef
  model: Question
  id: number = 0
  routeSub: Subscription
  baseUrl = environment.apiUrl
  isNew = false
  shouldDisplayComments = false
  currentUserUsername: string
  isCurrentUserQuestionOwner: boolean
  isInEditMode = false
  communitiesAsString: string
  lengthAsString: string
  review: string
  rating: number
  hasCurrentUserMadeAnOffer: boolean = false

  modalConfig = {
    animated: true,
    backdrop: true,
    ignoreBackdropClick: true,
  }

  constructor(
    private questionService: QuestionService,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private modalService: BsModalService,
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

  openImage(){
    window.open( this.model.photoUrl,'Question Image');
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
            this.model.askerUsername === this.currentUserUsername;

          this.hasCurrentUserMadeAnOffer=this.model.offers.some((offer)=> offer.username==this.currentUserUsername);
          
        }
      },
      (error) => {
        console.log(error)
      },
    )
  }


  solved(template: TemplateRef<any>) {
    this.questionService.markQuestionAsSolved(this.model.id);
    this.modalRef = this.modalService.show(template, this.modalConfig)
  }

  answererHelped(template: TemplateRef<any>) {
    this.modalRef?.hide()

    setTimeout(() => {
      let reviewModal: TemplateRef<any>
      this.bsModalRefReview = this.modalService.show(template, this.modalConfig)
    }, 100)

    // this.modalRef = this.modalService.show('reviewModal', this.modalConfig)
    //   alert('yo')
  }
  answererDidntHelp() {
    this.modalRef?.hide()
  }

  ratingChanged(rating: any) {
    this.rating = rating
    //rating
    //this.review

    //alert(rating);
    console.log(rating)
  }

  submitReview() {
    let review: Review = {
      id: 0,
      questionId: this.model.id,
      rating: this.rating,
      text: this.review,
      reviewerId: this.model.askerId,
      revieweeId: this.model.lastAnswererUserId,
    }

    this.questionService.publishReview(review).subscribe(() => {
      this.bsModalRefReview.hide()
    })
  }
}
