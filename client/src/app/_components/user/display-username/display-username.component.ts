import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-username',
  templateUrl: './display-username.component.html',
  styleUrls: ['./display-username.component.scss']
})
export class DisplayUsernameComponent implements OnInit {
  @Input() username: string

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  clicked(){
    this.router.navigateByUrl('users/' + this.username);
  }
}
