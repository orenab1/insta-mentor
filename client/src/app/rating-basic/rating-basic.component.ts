import { Component } from '@angular/core';
 
@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: 'rating-basic',
  templateUrl: './rating-basic.component.html',
  styleUrls: ['./rating-basic.component.scss']
})
export class RatingBasicComponent {
  max = 5;
  rate = 0;
  isReadonly = false;
  overStar: number | undefined;
  starsAsText:string;


  confirmSelection(event: KeyboardEvent) {
    if (event.keyCode === 13 || event.key === 'Enter') {
      this.isReadonly = true;
    }
  }

  hoveringOver(value: number): void {
    this.overStar = value;
    switch (this.overStar){
      case 1:this.starsAsText='Very Poor'; break;
      case 2:this.starsAsText='Poor'; break;
      case 3:this.starsAsText='Ok'; break;
      case 4:this.starsAsText='Very Good'; break;
      case 5:this.starsAsText='Great!'; break;
    }
  }
 
  resetStars() {
    this.rate = 0;
    this.isReadonly = false;
  }
}






