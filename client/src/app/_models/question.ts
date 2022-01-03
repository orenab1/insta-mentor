export interface Question {
    id: number;
    header: string;
    body: string;
    comments: Comment[];
    offers: Offer[];
    askerId: number;
    askerUsername: string;
}


export interface Comment {
    id: number;
    text: string;
    questionId: number;
    commentorId: number;
}

export interface Offer {
    id: number;
    text: string;
}