import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
 
@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: 'rating-basic',
  templateUrl: './rating-basic.component.html',
  styleUrls: ['./rating-basic.component.scss']
})
export class RatingBasicComponent  implements OnInit{
  max = 5;
  rate = 0;
  isReadonly = false;
  overStar: number | undefined;
  starsAsText:string=''; //without empty string no place will be reserved for text
  @Output() onRatingChanged=new EventEmitter<number>();
  @Input() isDisplayOnly:false;
  @Input() ratingOneToFive:number;

  starsCountPercentage:number;

  ngOnInit(): void {
    
    this.starsCountPercentage=(this.ratingOneToFive/5)*100
   
  }


  
  confirmSelection(event: KeyboardEvent) {
    if (event.keyCode === 13 || event.key === 'Enter') {
      this.isReadonly = true;
    }
    this.onRatingChanged.emit(this.rate);
  }

  clicked(value: number)
  { 
    this.onRatingChanged.emit(this.rate);
  }

  hoveringOver(value: number): void {
    this.overStar = value;
    switch (this.overStar){
      case 1:this.starsAsText='Not helpful'; break;
      case 2:this.starsAsText='Not so much'; break;
      case 3:this.starsAsText='Somewhat helpful'; break;
      case 4:this.starsAsText='Very helpful'; break;
      case 5:this.starsAsText='Awesome!'; break;
    }
  }
 
  resetStars() {
    this.rate = 0;
    this.isReadonly = false;
  }
}






