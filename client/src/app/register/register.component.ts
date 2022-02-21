import { Component, Input, OnInit, Output } from '@angular/core';
import { Register } from '../_models/register';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  model:Register;
  shouldDisplayPassword: boolean = false;

  constructor(private accountService:AccountService) {
    this.model={
      email: '',
      password: ''
    }
   }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(response =>{
      console.log(response);
      this.cancel();
    }, error =>{
      console.log(error);
    })
  }

  cancel(){
  }

  toggleDisplayPassword() {
    this.shouldDisplayPassword = !this.shouldDisplayPassword;
}
}
