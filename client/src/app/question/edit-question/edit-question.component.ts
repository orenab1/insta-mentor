import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Subscription } from 'rxjs'
import { take } from 'rxjs/operators'
import { Community } from 'src/app/_models/community'
import { Tag } from 'src/app/_models/tag'
import { AccountService } from 'src/app/_services/account.service'
import { CommonService } from 'src/app/_services/common.service'
import { CommunityService } from 'src/app/_services/community.service'
import { environment } from 'src/environments/environment'
import { Question } from '../../_models/question'
import { QuestionService } from '../../_services/question.service'

@Component({
  selector: 'app-edit-question',
  templateUrl: './edit-question.component.html',
  styleUrls: ['./edit-question.component.scss'],
})
export class EditQuestionComponent implements OnInit {
  model: Question
  id: number = 0
  routeSub: Subscription
  baseUrl = environment.apiUrl
  isNew = false
  shouldDisplayComments = true
  currentUserUsername: string
  isCurrentUserQuestionOwner: boolean
  isInEditMode = false
  pageTitle = ''
  allTags: Tag[]
  allCommunities: Community[]

  constructor(
    private questionService: QuestionService,
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private commonService: CommonService,
    private communityService: CommunityService,
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.currentUserUsername = user.username))
  }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe((params) => {
      this.id = parseInt(params['id']) || 0

      if (this.id === 0) {
        this.isInEditMode = true
        this.isNew = true
      }

      if (this.isNew) {
        this.pageTitle = 'Ask a Question'
      } else {
        this.pageTitle = 'Question'
      }

      this.model = {
        id: 0,
        header: '',
        body: '',
        isSolved: false,
        comments: [],
        offers: [],
        askerId: 0,
        askerUsername: '',
        photo: { Url: '', Id: 0 },
        isActive: false,
        tags: [],
        communities: [],
        isPayed: false,
        length: 1,
        created: '',
        lastAnswererUserId: undefined,
        lastAnswererUserName:'',
        revieweeUsername:''
      }
      this.getQuestion()
    })

    this.loadTags()
    this.loadCommunities()
  }

  toggleDisplayComments() {
    this.shouldDisplayComments = !this.shouldDisplayComments
  }

  loadTags() {
    this.commonService.getTags().subscribe((response) => {
      this.allTags = response
      error: (error) => console.log(error)
    })
  }

  loadCommunities() {
    this.communityService.getCommunitiesTags().subscribe((response) => {
      this.allCommunities = response
      error: (error) => console.log(error)
    })
  }

  getQuestion() {
    this.questionService.getQuestion(this.id).subscribe(
      (response) => {
        if (response != null) {
          this.model = response
          this.isCurrentUserQuestionOwner =
            this.model.askerUsername === this.currentUserUsername
        }
      },
      (error) => {
        console.log(error)
      },
    )
  }

  toggleValueChanged() {
    this.model.isActive = !this.model.isActive
  }

  onUploadPhotoSuccess = (response: any) => {
    if (response) {
      let photo = JSON.parse(response)
      this.model.photo = { Url: '', Id: 0 }
      this.model.photo.Url = photo.url
      this.model.photo.Id = photo.id
    }
  }

  deletePhoto = () => {
    this.model.photo.Url = ''
    this.model.photo.Id = 0
  }

  askQuestion() {
    this.questionService.askQuestion(this.model).subscribe({
      next: (questionId) =>
        this.router.navigateByUrl('display-question/' + questionId),
      error: (error) => console.log(error),
    })
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe()
  }

  setLength(length: number) {
    this.model.length = length
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl])
    })
  }
}
