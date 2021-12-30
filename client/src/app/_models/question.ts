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
}

export interface offer {
    id: number;
    text: string;
}