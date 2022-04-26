import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { BusyService } from '../_services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService:BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if (!request.url.includes('questions/questions') && !request.url.includes('questions/my-questions')){
      this.busyService.busy();
    }
    
    return next.handle(request).pipe(
      finalize(()=>
        {
        this.busyService.idle()          
        }
      )
    );
  }
}