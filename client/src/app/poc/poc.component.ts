import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-poc',
  templateUrl: './poc.component.html',
  styleUrls: ['./poc.component.scss']
})
export class PocComponent implements OnInit {
  items = [];

  placeholder=''
  constructor() { }

  ngOnInit(): void {
    this.items=[
      {
        display:'SQL',
        value:'1',
      },
      {
        display:'C#',
        value:'2',
      },
      {
        display:'Python',
        value:'3',
      },
      {
        display:'NodeJS',
        value:'4',
      },
      {
        display:'React',
        value:'5',
      },
    ]
  }

}
