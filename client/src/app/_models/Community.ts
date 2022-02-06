export interface Community {
  value: number
  display: string
}

export interface CommunitySummary {
  id: number
  name: string
  description: string
  numOfMembers: number
  numOfQuestionsAsked: number
  bestTimeToGetAnswer: string
}
