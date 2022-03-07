import { Community } from "./community";
import { Review } from "./question";
import { Tag } from "./tag";

export interface Member {
    id: number;
    username: string;
    email: string;
    title: string;
    aboutMe: string;
    photoUrl: string;
    created: Date;
    photoId: number;
    tags: Tag[];
    communities:Community[];
    questionAskedOnMyTags: boolean;
    onlyNotifyOnCommunityQuestionAsked: boolean;
    myQuestionReceivedNewOffer: boolean;
    myQuestionReceivedNewComment: boolean;
    askerAverageRating:number;
    askerNumOfRatings:number;
    reviews:Review[];

}
