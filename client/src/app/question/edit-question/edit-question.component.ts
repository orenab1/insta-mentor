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
  doesQuestionHavePhotoBeforeEdit:boolean
  questionId:number
  questionGuid:string
  idOrGuid:any

  userFilledHeader = true
  userFilledTags = true

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

      this.idOrGuid=params['id'];

      // if (isNaN(parseInt(params['id']))) {
      //   this.questionGuid=params['id'];
      // }else{
      //   this.questionId=parseInt(params['id']);
      // }
      

      if (this.id === 0) {
        this.isInEditMode = true
        this.isNew = true
      }

      if (this.isNew) {
        this.pageTitle = 'Find Helpers'
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
        lastAnswererUserName: '',
        revieweeUsername: '',
        photoUrl: '',
        photoId: 0,
        neededSkills:'',
        userEmail:'',
        discordLink:'',
        hasCurrentUserRequestedFeedback:false
      }
      this.getQuestion()
    })

    this.loadTags()
  }

  openImage() {
    window.open(this.model.photoUrl, 'Question Image')
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



    this.questionService.getQuestion(this.idOrGuid).subscribe(
      (response) => {
        if (response.id != 0) {
          this.model = response
          this.isCurrentUserQuestionOwner =
            this.model.askerUsername === this.currentUserUsername

          this.doesQuestionHavePhotoBeforeEdit=this.model.photoId!=0;
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
      this.model.photoUrl = photo.url
      this.model.photoId = photo.id
    }
  }

  deletePhoto = () => {
    this.model.photoUrl = ''
    this.model.photoId = 0
  }

  askQuestion() {
    this.userFilledHeader = this.model.header!=null && this.model.header!=undefined && this.model.header!='' && this.model.header.length>0;
    this.userFilledTags =!(this.model.tags == null || this.model.tags.length == 0)
  
    if (!this.userFilledHeader || !this.userFilledTags) {
      return;
    }

    if (this.model.tags != null && this.model.tags.length != 0) {
      for (let i = 0; i < this.model.tags.length; i++) {
        if (isNaN(this.model.tags[i].value)) {
          this.model.tags[i].value = 0
        }
      }
    }

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

  cancel() {
    let url='';
    if (this.model.id == 0) {
      url=''
    } else {
      url='display-question/' + this.idOrGuid
    }

    this.router.navigateByUrl(url)
  }

  removePhoto(){
    this.model.photoId=0;
    this.model.photoUrl=''
    this.model.photo={ Url: '', Id: 0 }
  }
}
