import { Component, OnInit, TemplateRef } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Observable, Subscription } from 'rxjs'
import { take } from 'rxjs/operators'
import { AccountService } from 'src/app/_services/account.service'
import { environment } from 'src/environments/environment'
import { Question } from '../../_models/question'
import { Review } from '../../_models/question'
import { QuestionService } from '../../_services/question.service'
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal'
import { CommonService } from 'src/app/_services/common.service'
import { collapseTextChangeRangesAcrossMultipleVersions } from 'typescript'

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
  shouldDisplayComments = false
  currentUserUsername: string
  isCurrentUserQuestionOwner: boolean
  communitiesAsString: string
  lengthAsString: string
  questionGuid:string
  review: string
  rating: number
  hasCurrentUserMadeAnOffer: boolean = false
  questionId:number
  answererUserId:number=0

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
    private commonService:CommonService
  ) {}

  ngOnInit(): void {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.currentUserUsername = user? user.username: undefined))

    this.routeSub = this.route.params.subscribe((params) => {

      if (params['userid']!=undefined){
        this.answererUserId=parseInt(params['userid']);
      }

      if (isNaN(params['id'])) {
        this.questionGuid=params['id'];
      }else{
        this.questionId=parseInt(params['id']);
      }

      this.getQuestion()
    })
  }

  openImage() {
    window.open(this.model.photoUrl, 'Question Image')
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
  editQuestion(){
    if (this.questionId===undefined){
      this.router.navigateByUrl('edit-question/'+this.questionGuid);
    }else{
      this.router.navigateByUrl('edit-question/'+this.questionId);
    }
  }

  getQuestion() {

    let getGuestion:Observable<Question>;
    if (this.questionId===undefined){
      getGuestion=this.questionService.getQuestionByGuid(this.questionGuid)
    } else{
      getGuestion=this.questionService.getQuestion(this.questionId);
    }

    getGuestion.subscribe(
      (response) => {
        if (response != null) {
          this.model = response
        
          this.isCurrentUserQuestionOwner =
            this.model.askerUsername === this.currentUserUsername || this.questionGuid!=null;
        }
      },
      (error) => {
        console.log(error)
      },
    )
  }

  solved(template: TemplateRef<any>) {
    this.questionService.markQuestionAsSolved(this.model.id,this.questionGuid);
    this.model.isSolved=true;
  }
 
  ratingChanged(rating: any) {
    this.rating = rating
    //rating
    //this.review

    //alert(rating);
    console.log(rating)
  }

  requestFeedback(){
    this.questionService.requestFeedback(this.model.id,this.questionGuid).subscribe(() => {
      const currentUrl = this.router.url
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate([currentUrl])
      })
    })
  }

  submitReview() {
    let review: Review = {
      id: 0,
      questionId: this.model.id,
      rating: this.rating,
      text: this.review,
      reviewerId: this.model.askerId,
      revieweeId: this.answererUserId,
    }

    this.questionService.publishReview(review).subscribe(() => {
      alert('Review received. Thank you');
      const currentUrl = this.router.url
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate([currentUrl])
      })
    })
  }
}
