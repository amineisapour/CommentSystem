export interface Comment {
    id: number;
    username: string;
    registerDateTime: string;
    text: string;
    parentId: number | null;
    Level: number;
    children?: Comment[] | null;
}