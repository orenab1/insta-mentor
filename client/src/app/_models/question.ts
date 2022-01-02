export interface question {
    id: number;
    header: string;
    body: string;
    comments: comment[];
    offers: offer[];
}


export interface comment {
    id: number;
    text: string;
    questionId:number;
    commentorId:number;
}

export interface offer {
    id: number;
    text: string;
}