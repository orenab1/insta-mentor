import { Component, Input, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { take } from 'rxjs'
import { AccountService } from 'src/app/_services/account.service'
import { UsersService } from 'src/app/_services/Users.service'
import { Comment } from '../../_models/question'
import { QuestionService } from '../../_services/question.service'

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
})
export class CommentsComponent implements OnInit {
  @Input() comments: Comment[]
  @Input() questionId: number
  @Input() askerId: number
  shouldDisplayAddForm: boolean = false
  shouldDisplayCommentButton: boolean = true
  model: any = {}  
  currentUserId: number

  constructor(
    private questionService: QuestionService,
    private usersService: UsersService,
    private accountService: AccountService,
    private router: Router,
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.currentUserId = user.id))
  }

  ngOnInit(): void {

  }

  cancel() {
    this.shouldDisplayCommentButton = true
  }

  postComment() {
    this.shouldDisplayCommentButton = true
    const model = {}
    // this.questionService.postComment(this.model).subscribe(response => {
    //   this.reloadCurrentRoute();
    // });
  }

  displayAddForm() {
    this.shouldDisplayAddForm = !this.shouldDisplayAddForm
    this.shouldDisplayCommentButton = false
  }

  post() {
    this.shouldDisplayAddForm = false
    this.shouldDisplayCommentButton = true
    this.model.questionId = this.questionId
    this.questionService.postComment(this.model).subscribe((response) => {
      this.reloadCurrentRoute()
      this.ngOnInit()
      error: (error) => this.ngOnInit()
    })
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl])
    })
  }

  deleteComment(commentId:number) {
    this.questionService.deleteComment(commentId).subscribe((response) => {
      this.reloadCurrentRoute()
    });
  }
}
