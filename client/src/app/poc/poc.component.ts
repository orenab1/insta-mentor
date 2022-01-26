import { ChangeDetectorRef, Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
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
  ) {}

  signIn() {
    this.googleSigninService.signIn()
  }

  signOut() {
    this.googleSigninService.signOut()
  }

  ngOnInit(): void {
    this.googleSigninService.observable().subscribe((user) => {
      this.user = user
      this.ref.detectChanges()
    })

    ;(window as any).fbAsyncInit = function () {
      this.FB.init({
        appId: '490846212400326',
        cookie: true,
        xfbml: true,
        version: 'v3.1',
      })
      this.FB.AppEvents.logPageView()
    }

    ;(function (d, s, id) {
      var js,
        fjs = d.getElementsByTagName(s)[0]
      if (d.getElementById(id)) {
        return
      }
      js = d.createElement(s)
      js.id = id
      js.src = 'https://connect.facebook.net/en_US/sdk.js'
      fjs.parentNode.insertBefore(js, fjs)
    })(document, 'script', 'facebook-jssdk')

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

    this.toastr.success('Hello world!', 'Toastr fun!')
    this.toastr.info('Hello world!', 'Toastr fun!')
    this.toastr.show('Hello world!', 'Toastr fun!')
    this.toastr.warning('Hello world!', 'Toastr fun!')
  }

  submitLogin() {
    console.log('submit login to facebook')
    // FB.login();
    this.FB.login((response) => {
      console.log('submitLogin', response)
      if (response.authResponse) {
        alert('logged in FB')
      } else {
        alert('User login failed')
      }
    })
  }
}
