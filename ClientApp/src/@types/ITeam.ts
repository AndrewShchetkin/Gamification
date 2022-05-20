import { IUser } from "./IUser";

export  interface ITeam{
    id: number;
    teamName: string;
    users: IUser[]
}