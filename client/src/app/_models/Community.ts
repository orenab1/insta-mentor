export interface Community {
  value: number
  display: string
}

export interface AddCommunity {
  name: string
  description: string
}


export interface CommunityFull {
  id: number
  name: string
  description: string
  numOfMembers: number
  numOfQuestionsAsked: number
  bestTimeToGetAnswer: string
  isCurrentUserCreator: boolean
  isCurrentUserMember: boolean
}
