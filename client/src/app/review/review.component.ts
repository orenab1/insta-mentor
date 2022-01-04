import { Component, Input, OnInit } from '@angular/core';
@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss']
})

export class ReviewComponent implements OnInit {
  @Input() questionId: number;
  currentRate = 8;

  constructor() { }

  ngOnInit(): void {
  }

}
