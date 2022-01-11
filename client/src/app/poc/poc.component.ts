import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-poc',
  templateUrl: './poc.component.html',
  styleUrls: ['./poc.component.scss']
})
export class PocComponent implements OnInit {
  items = ['Pizza', 'Pasta', 'Parmesan'];

  placeholder=''
  constructor() { }

  ngOnInit(): void {
    
  }

}
