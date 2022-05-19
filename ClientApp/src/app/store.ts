import { quizeSlice } from './../quiz/quizeSlice';
import { authSlice } from './../auth/authSlice';
import { configureStore } from '@reduxjs/toolkit'

export const store = configureStore({
    reducer:{
        auth: authSlice.reducer, // все состояния проложения тут 
        quize: quizeSlice.reducer,
    }
});

export type RootState = ReturnType<typeof store.getState>;// возвращает все состояния приложения 
export type AppDispatch = typeof store.dispatch;