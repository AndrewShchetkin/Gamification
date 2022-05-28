import { IUser } from "../IUser";

export interface AuthState extends IUser {
    isAuthenticated: boolean,
    requestSended: boolean,
    error: boolean
}