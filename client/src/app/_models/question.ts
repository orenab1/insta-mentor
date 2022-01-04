export interface Question {
    id: number;
    header: string;
    body: string;
    comments: Comment[];
    offers: OfferInQuestion[];
    askerId: number;
    askerUsername: string;
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