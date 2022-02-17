import { Community } from './community'
import { Tag } from './tag'

export interface Question {
  id: number
  header: string
  body: string
  isSolved: boolean
  comments: Comment[]
  offers: OfferInQuestion[]
  askerId: number
  askerUsername: string
  photoUrl: string
  photoId: number
}

export interface QuestionSummary {
  id: number
  header: string
  body: string
  numOfComments: number
  numOfOffers: number
  askerId: number
  askerUsername: string
  askerPhotoUrl: string
  tags: Tag[]
  communities: Community[]
  isPayed: boolean
  hasCommonTags: boolean
  hasCommonCommunities: boolean
  howLongAgo: string
  length: number;
  lengthAsString:string;
}

export interface Comment {
  id: number
  text: string
  questionId: number
  commentorUsername: string
}

export interface OfferInQuestion {
  id: number
  username: string
}
