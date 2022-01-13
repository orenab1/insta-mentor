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
    asdf:string[];
}
