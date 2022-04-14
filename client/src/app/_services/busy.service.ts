import { Injectable } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;


  constructor(private ngxLoader: NgxUiLoaderService) { }

  busy() {
    this.busyRequestCount++;
    this.ngxLoader.start();
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.ngxLoader.stop();
    }
  }
}
