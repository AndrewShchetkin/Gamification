import { LoginResponse } from '../../../@types/loginResponse';
import { RootState } from '../../store';
import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit'
import { useAppDispatch } from '../../hooks';
import { AuthState } from '../../../@types/ReduxTypes/AuthState';
import axios from 'axios';
import { fetchUser } from './actionCreators';

export const initialState: AuthState = {
    isAuthenticated: false,
    requestSended: true,
    error: false,
    name: ''
}

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
        builder.addCase(fetchUser.fulfilled, (state, action) => {
            state.requestSended = false;
        });
    }
});

export const { signInComplete, startLoadData, endLoadData } = authSlice.actions;

export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated

export default authSlice.reducer;



