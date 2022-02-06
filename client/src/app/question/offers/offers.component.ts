import { Component, Input, OnInit } from '@angular/core';
import { OfferInQuestion } from 'src/app/_models/question';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss']
})
export class OffersComponent implements OnInit {
  @Input() offers: OfferInQuestion[];
  constructor() { }

  ngOnInit(): void {
  }

  acceptOffer(offerId:number){
    alert('offer accepted!' +offerId);
  }
}
