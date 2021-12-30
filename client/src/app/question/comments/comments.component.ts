import { Component, Input, OnInit } from '@angular/core';
import { comment } from '../../_models/question';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() comment:comment;

  constructor() { }

  ngOnInit(): void {
  }

}
