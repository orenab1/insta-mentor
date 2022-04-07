import { ChangeDetectorRef, Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { QuestionService } from '../_services/question.service'
import { GoogleSigninService } from '../_services/SignIn/google-signin.service'

@Component({
  selector: 'app-poc',
  templateUrl: './poc.component.html',
  styleUrls: ['./poc.component.scss'],
})
export class PocComponent implements OnInit {
  items = []
  user: gapi.auth2.GoogleUser
  FB: any

  placeholder = ''
  constructor(
    private toastr: ToastrService,
    private googleSigninService: GoogleSigninService,
    private ref: ChangeDetectorRef,
    private questionService: QuestionService
    
  ) {}

  signIn() {
    this.googleSigninService.signIn()
  }

  signOut() {
    this.googleSigninService.signOut()
  }

  startZoom(){
    this.questionService.acceptOffer(50).subscribe(response => {    
    });
  }

  ngOnInit(): void {
    this.googleSigninService.observable().subscribe((user) => {
      this.user = user
      this.ref.detectChanges()
    })

  

    this.items = [
      {
        display: 'SQL',
        value: '1',
      },
      {
        display: 'C#',
        value: '2',
      },
      {
        display: 'Python',
        value: '3',
      },
      {
        display: 'NodeJS',
        value: '4',
      },
      {
        display: 'React',
        value: '5',
      },
    ]

    this.toastr.info(
      `<div><b>dave11</b> wants your help in answering their question:</div><br/><div><b>How to prevent Sql Injection?</b></div><br/> <a href="https://www.w3schools.com/" target="_blank">Click here to start a Zoom session with them</a><br/><br/> <a href="https://www.w3schools.com/" target="_blank">Click here to see the question</a>`,
      '',
      {
        timeOut: 600000,
        enableHtml: true,
        progressBar: true,
        progressAnimation: 'increasing',
        tapToDismiss:false,
        closeButton:true,
        disableTimeOut:'extendedTimeOut'
      },
    )


   // this.toastr.info('<br/><div>How do you prevent Sql Injection?</div><br/> <a href="https://www.w3schools.com/" target="_blank">Click here to start a Zoom session with them</a><br/><br/> <a href="https://www.w3schools.com/">Click here to see the question</a>', 'username wants your help in answering their question',{disableTimeOut:true, closeButton:true, enableHtml:true,progressBar:true,progressAnimation:'increasing' })
    // this.toastr.info('Hello world!', 'Toastr fun!')
    // this.toastr.show('Hello world!', 'Toastr fun!')
    // this.toastr.warning('Hello world!', 'Toastr fun!')
  }

 
}
