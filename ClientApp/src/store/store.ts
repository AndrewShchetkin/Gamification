import { authSlice } from './reducers/auth/authSlice';
import { configureStore } from '@reduxjs/toolkit'

export const store = configureStore({
    reducer:{
        auth: authSlice.reducer // все состояния проложения тут 
    }
});

export type RootState = ReturnType<typeof store.getState>;// возвращает все состояния приложения 
export type AppDispatch = typeof store.dispatch;