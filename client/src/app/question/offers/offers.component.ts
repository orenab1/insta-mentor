import { Component, Input, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { take } from 'rxjs'
import { OfferInQuestion } from 'src/app/_models/question'
import { AccountService } from 'src/app/_services/account.service'
import { QuestionService } from 'src/app/_services/question.service'

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss'],
})
export class OffersComponent implements OnInit {
  @Input() offers: OfferInQuestion[]
  @Input() isCurrentUserQuestionOwner: boolean
  @Input() currentUserId:number

  constructor(private questionService: QuestionService, private accountService: AccountService,private router: Router) {
    this.accountService.currentUser$
    .pipe(take(1))
    .subscribe((user) => (this.currentUserId = user.id))
  }

  ngOnInit(): void {}

  acceptOffer(offerId: number) {
    this.questionService.acceptOffer(offerId).subscribe(
      (response) => {
        window.open(response.meetingUrl, '_blank')
      },
      (error) => {
        console.log(error)
      },
    )
  }

  deleteOffer(offerId:number) {
    this.questionService.deleteOffer(offerId).subscribe((response) => {
      this.reloadCurrentRoute()
    });
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl])
    })
  }
}
