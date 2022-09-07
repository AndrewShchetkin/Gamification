import { ITeam } from "../ITeam";

export interface TeamsState {
    teams: ITeam[],
    requestSended: boolean,
    error: boolean
}