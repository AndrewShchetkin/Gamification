import { LoginResponse } from '../../../@types/loginResponse';
import { RootState } from '../../store';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AuthState } from '../../../@types/ReduxTypes/AuthState';
import { fetchUser } from './actionCreators';
import { Axios, AxiosError } from 'axios';


export const initialState: AuthState = {
    isAuthenticated: false,
    requestSended: true,
    error: false,
    userName: '',
    id: '',
    teamId: ''
}

function isAxiosError(error: any): error is AxiosError {
    return error.isAxiosError === true;
}

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        signInComplete: (state: AuthState, action: PayloadAction<LoginResponse>) => {
            state.isAuthenticated = true;
            state.userName = action.payload.userName ?? "";
            state.teamId = action.payload.userTeamId ?? "";
        },
        startLoadData: state => {
            state.requestSended = true
        },
        endLoadData: state => {
            state.requestSended = false
        },
        setTeamId: (state: AuthState, action: PayloadAction<string>) => {
            state.teamId = action.payload;
        }
    },
    extraReducers: {
        [fetchUser.fulfilled.type]: (state: AuthState, action: PayloadAction<LoginResponse>) => { // Данные получены
            state.isAuthenticated = true;
            state.userName = action.payload.userName ?? "";
            state.teamId = action.payload.userTeamId ?? "";
            state.requestSended = false;
        },
        [fetchUser.pending.type]: (state: AuthState) => { // идет запрос
            state.requestSended = true
        },
        [fetchUser.rejected.type]: (state: AuthState, action: PayloadAction) => { // ошибка
            if (isAxiosError(action.payload)) {
                if (action.payload.response?.status != 401) {
                    console.error(action.payload.message);
                }
            }
            else {
                console.error(action.payload)
            }
            state.requestSended = false;
        }
    },
});

export const { signInComplete, startLoadData, endLoadData, setTeamId } = authSlice.actions;

export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated

export default authSlice.reducer;



