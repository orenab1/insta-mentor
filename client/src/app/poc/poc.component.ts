import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-poc',
  templateUrl: './poc.component.html',
  styleUrls: ['./poc.component.scss']
})
export class PocComponent implements OnInit {
  items = [];

  placeholder=''
  constructor(private toastr: ToastrService) { }

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


    this.toastr.success('Hello world!', 'Toastr fun!');
    this.toastr.info('Hello world!', 'Toastr fun!');
    this.toastr.show('Hello world!', 'Toastr fun!');
    this.toastr.warning('Hello world!', 'Toastr fun!');
    
  }

}
