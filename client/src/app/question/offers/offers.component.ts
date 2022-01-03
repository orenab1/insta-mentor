import { Component, OnInit } from '@angular/core';
import { Offer } from 'src/app/_models/question';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss']
})
export class OffersComponent implements OnInit {
  offers:Offer[];
  constructor() { }

  ngOnInit(): void {
  }

}
