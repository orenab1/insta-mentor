import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HomeService } from '../_services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  registerMode=false;
  questions:any;
  

  constructor(private homeService:HomeService) { }

  ngOnInit(): void {
    this.getQuestions();
  }


  registerToggle(){
    this.registerMode=!this.registerMode;
  }
  
  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }

    getQuestions(){
     this.questions= this.homeService.questions;
  }
}
