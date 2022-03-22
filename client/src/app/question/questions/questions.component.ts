import { Component, OnInit } from '@angular/core'
import { Question, QuestionSummary } from 'src/app/_models/question'
import { PresenceService } from 'src/app/_services/signalR/presence.service'
import { QuestionService } from 'src/app/_services/question.service'
import { NavigationEnd, Router } from '@angular/router'
import { filter, Observable } from 'rxjs'

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss'],
})
export class QuestionsComponent implements OnInit {
  questions: QuestionSummary[]
  isFromMyQuestionsRoute: boolean = false

  constructor(
    private questionService: QuestionService,
    public presenceService: PresenceService,
    private router: Router,
  ) {
    router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        let currentRoute = event.url
        if (currentRoute.indexOf('my') != -1) {
          this.isFromMyQuestionsRoute = true
        }
      })
  }

  ngOnInit(): void {
    this.loadQuestions()
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
          switch (q.length) {
            case 1:
              q.lengthAsString = '<5 mins.'
              break;
            case 2:
              q.lengthAsString = '<15 mins.'
              break;
            case 3:
              q.lengthAsString = '>15 mins.'
              break;
          }
        })
      },
      (error) => {
        console.log(error)
      },
    )
  }

  goToAsk(){
    this.router.navigateByUrl('question/edit-question');
  }
}
