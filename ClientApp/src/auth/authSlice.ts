import { LoginResponse } from './loginResponse';
import { RootState } from './../app/store';
import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit'
import { useAppDispatch } from '../app/hooks';

export interface AuthState {
    isAuthenticated: boolean,
    requestSended: boolean,
    error: boolean,
    name: string
}

export const initialState: AuthState = {
    isAuthenticated: false,
    requestSended: true,
    error: false,
    name: ''
}


export const userOkFetch = createAsyncThunk('get/api/user', async (): Promise<void> => {
    const dispatch = useAppDispatch();

    dispatch(startLoadData());
    try {

        const response = await fetch('api/user');
        let body: LoginResponse = {};
        console.log(response);
        if (response.ok && response.body) {
            body = await response.json();
            dispatch(signInComplete(body.username ?? ""));
        }
        
    } catch (ex) {
        console.error(ex);
    } 
    dispatch(endLoadData());
});

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {// функции которые меняют состояние 
        signInComplete: (state: AuthState, action: PayloadAction<string>) => {
            state.isAuthenticated = true;
            state.name = action.payload
        },
        startLoadData: state => {
            state.requestSended = true
        },
        endLoadData: state => {
            state.requestSended = false
        },

    },
    extraReducers: (builder) => {
        builder.addCase(userOkFetch.fulfilled, (state, action) => {
            state.requestSended = false;
        });
    }
});

export const { signInComplete, startLoadData, endLoadData } = authSlice.actions;

export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated

export default authSlice.reducer;



