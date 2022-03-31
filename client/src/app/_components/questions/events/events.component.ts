import { Component, OnInit } from '@angular/core'
import { QuestionService } from 'src/app/_services/question.service'
import { Event } from 'src/app/_models/question'

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
})
export class EventsComponent implements OnInit {
  model: Event[]
  minutesOffsetLocalUtc: number

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.loadEvents()
    this.minutesOffsetLocalUtc = new Date().getTimezoneOffset()
    
    setInterval(() => {
      this.model.forEach((x) => {
      x.minutesToStart=this.getMinutesToStart(x);
      x.minutesToEnd=this.getMinutesToEnd(x);
    })}, 60_000);
  }

  loadEvents() {
    this.questionService.getEvents().subscribe((events) => {
      this.model = events
      this.model.forEach((x) => {
        x.localTime = new Date(x.utcTime)
        x.localTime.setMinutes(
          x.localTime.getMinutes() - this.minutesOffsetLocalUtc,
        )
        x.minutesToStart =this.getMinutesToStart(x);
        x.minutesToEnd =this.getMinutesToEnd(x);

        // var breakAfterCharsNums = x.nextText.lastIndexOf('Featuring')
        // x.nextText = x.nextText.substring(0, breakAfterCharsNums) + ' <br/> ' + x.nextText.substring(breakAfterCharsNums)
      })


    this.model.sort((a,b)=>this.eventsComparetor(a,b)).reverse();
    })
  }
  eventsComparetor(event1:Event, event2:Event)
  { 
    if (event1.minutesToStart>event2.minutesToStart){
      return -1;
    }
    if (event2.minutesToStart>event1.minutesToStart){
      return 1;
    }
    return 0;
  }

  getMinutesToStart(event:Event):number {
    return (event.localTime.getTime() - new Date().getTime()) / (1000 * 60)
  }

  getMinutesToEnd(event:Event):number{
    return event.minutesToStart+event.durationMinutes;
  }
}
