export interface Question {
    id: number;
    header: string;
    body: string;
    comments: Comment[];
    offers: OfferInQuestion[];
    askerId: number;
    askerUsername: string;
    photoUrl: string;
    photoId: number;
}

export interface QuestionSummary {
    id: number;
    header: string;
    body: string;
    numOfComments: number;
    numOfOffers: number;
    askerId: number;
    askerUsername: string;
    askerPhotoUrl: string;
    askerPhotoId: number;
}


export interface Comment {
    id: number;
    text: string;
    questionId: number;
    commentorUsername: string;
}

export interface OfferInQuestion {
    id: number;
    username: string;
}