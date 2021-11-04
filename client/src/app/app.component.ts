import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpClientModule} from '@angular/common/http';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'insta mentor';
  users:any;


  constructor(private http:HttpClient){}


  ngOnInit(){
    this.getUsers();
  }

  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe(response =>{
      this.users=response;
    }, error =>
    console.log(error));
  }

}

