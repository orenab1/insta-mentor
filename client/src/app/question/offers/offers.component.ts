import { Component, Input, OnInit } from '@angular/core';
import { OfferInQuestion } from 'src/app/_models/question';
import { QuestionService } from 'src/app/_services/question.service';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss']
})
export class OffersComponent implements OnInit {
  @Input() offers: OfferInQuestion[];
  constructor(
    private questionService: QuestionService,
  ) { }

  ngOnInit(): void {
  }

  acceptOffer(offerId:number){
    this.questionService.acceptOffer(offerId).subscribe(response => {
      window.open(response.meetingUrl, "_blank");      
    }, error => {
      console.log(error);
    });
  }
}
