import { Component, Input, OnInit } from '@angular/core';
import { comment } from '../../_models/question';
import { QuestionService } from '../../_services/question.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() comments:comment[];
  @Input() questionId:number;
  shouldDisplayAddForm:boolean=false;
  shouldDisplayCommentButton:boolean=true;
  model:Comment;

  constructor(private questionService: QuestionService) { }

  ngOnInit(): void {
  }

  cancel() {
    this.shouldDisplayCommentButton=true;
  }

  postComment(){
    
    this.shouldDisplayCommentButton=true;
    alert(this.model);
    const model={

    }

    // this.questionService.postComment(this.model).subscribe(response => {
    // });
  }

  displayAddForm(){
    this.shouldDisplayAddForm=!this.shouldDisplayAddForm;
    this.shouldDisplayCommentButton=false;
  }

  post(){
    this.shouldDisplayAddForm=false;
    this.shouldDisplayCommentButton=true;
  }
}
