import { Component, OnDestroy, OnInit } from '@angular/core'
import { Question, QuestionSummary } from 'src/app/_models/question'
import { PresenceService } from 'src/app/_services/signalR/presence.service'
import { QuestionService } from 'src/app/_services/question.service'
import { NavigationEnd, Router } from '@angular/router'
import { filter, interval, Observable, take, takeWhile } from 'rxjs'
import { User } from 'src/app/_models/user'
import { AccountService } from 'src/app/_services/account.service'
import { json } from 'stream/consumers'
import { UserConnectedDuration } from 'src/app/_models/userConnectedDuration'

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss'],
})
export class QuestionsComponent implements OnInit, OnDestroy {
  questions: QuestionSummary[]
  currentUser: User

  isFromMyQuestionsRoute: boolean = false
  isJavascriptPage=false;
  isSoftwareCraftsmanshipPage=false;

  intervalSeconds = 10
  userTimes: UserConnectedDuration[]
  intervalId

  constructor(
    private questionService: QuestionService,
    public presenceService: PresenceService,
    private router: Router,
    private accountService: AccountService,
  ) {
    router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        let currentRoute = event.url
        if (currentRoute.indexOf('my') != -1) {
          this.isFromMyQuestionsRoute = true
        }

        if (currentRoute.indexOf('javascript') != -1) {
          this.isJavascriptPage = true
        }

        if (currentRoute.indexOf('software-craftsmanship') != -1) {
          this.isSoftwareCraftsmanshipPage = true
        }
      })

    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.currentUser = user))
  }

  ngOnInit(): void {
    this.presenceService.onlineUsersTimesSource$.subscribe((usernamesTimes) => {
      this.userTimes = new Array() as Array<UserConnectedDuration>
      usernamesTimes.forEach((userTime) => {
        this.userTimes.push({
          username: userTime.username,
          secondsElapsed: userTime.secondsElapsed,
        })
      })

      this.loadQuestions()
    })

    this.intervalId = setInterval(() => {
      this.loadQuestions()
    }, this.intervalSeconds * 1000)
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId)
    }
  }

  loadQuestions(): void {
    let questions = new Observable<QuestionSummary[]>()
    if (this.isFromMyQuestionsRoute) {
      questions = this.questionService.getMyQuestions()
    } else {
      questions = this.questionService.getAllQuestions()
    }

    questions.subscribe(
      (response) => {
        this.questions = response
       
        this.questions.forEach((q) => {
          q.isUserOnline =
            q.onlineAgeSeconds!=0;

          if (q.isUserOnline) {
            q.onlineAgeString = this.getQuestionTime(q.onlineAgeSeconds)
          }

          switch (q.length) {
            case 1:
              q.lengthAsString = '<5 mins.'
              break
            case 2:
              q.lengthAsString = '<15 mins.'
              break
            case 3:
              q.lengthAsString = '>15 mins.'
              break
          }
        })

        this.questions = this.questions
          .sort((a, b) => this.questionsOrderComparetor(a, b))
          .reverse()
      },
      (error) => {
        console.log(error)
      },
    )
  }

  questionsOrderComparetor(
    question1: QuestionSummary,
    question2: QuestionSummary,
  ) {
    if (question1.onlineAgeSeconds !== 0 || question2.onlineAgeSeconds !== 0) {
      return this.numbersComperator(
        question1.onlineAgeSeconds,
        question2.onlineAgeSeconds,
      )
    }

    return this.numbersComperator(
      question1.ageInSeconds,
      question2.ageInSeconds,
    )
  }

  numbersComperator(num1: number = 0, num2: number = 0) {
    if (num1 > num2) return 1
    if (num2 > num1) return -1
    return 0
  }

  getQuestionTime(seconds) {
    let minutes = Math.floor(seconds / 60)
    let hours = 0

    if (minutes <= 60) {
      return minutes + ' Mins.'
    }
    hours = Math.floor(minutes / 60)
    minutes = minutes - hours * 60
    let minutesAsString = minutes.toString()
    if (minutes < 10) {
      minutesAsString = '0' + minutes
    }
    return hours + ':' + minutesAsString
  }

  goToAsk() {
    if (this.currentUser == null) {
      this.router.navigateByUrl('register')
    } else {
      this.router.navigateByUrl('question/edit-question')
    }
  }
}
