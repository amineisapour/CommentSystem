import * as Interfaces from "../interfaces/comment.interface";

export class Comment implements Interfaces.Comment {

    public constructor(
        public id: number,
        public username: string,
        public registerDateTime: string,
        public text: string,
        public parentId: number,
        public Level: number,
        public children?: Comment[] | null,
    ) { }
}