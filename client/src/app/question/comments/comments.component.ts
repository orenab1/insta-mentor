import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Comment } from '../../_models/question';
import { QuestionService } from '../../_services/question.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() comments: Comment[];
  @Input() questionId: number;
  shouldDisplayAddForm: boolean = false;
  shouldDisplayCommentButton: boolean = true;
  model: any={};

  constructor(private questionService: QuestionService, private router: Router) { }

  ngOnInit(): void {
  }

  cancel() {
    this.shouldDisplayCommentButton = true;
  }

  postComment() {

    this.shouldDisplayCommentButton = true;
    alert(this.model);
    const model = {
    }
    // this.questionService.postComment(this.model).subscribe(response => {
    //   this.reloadCurrentRoute();
    // });
  }

  displayAddForm() {
    this.shouldDisplayAddForm = !this.shouldDisplayAddForm;
    this.shouldDisplayCommentButton = false;
  }

  post() {
    this.shouldDisplayAddForm = false;
    this.shouldDisplayCommentButton = true;
    this.model.questionId=this.questionId;
    this.questionService.postComment(this.model).subscribe(response => {
    
      this.reloadCurrentRoute();
    });
  }


  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
}
