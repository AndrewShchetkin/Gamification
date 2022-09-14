import { authSlice } from './reducers/auth/authSlice';
import { combineReducers, configureStore } from '@reduxjs/toolkit'
import authReduser from './reducers/auth/authSlice'
import chatReduser from './reducers/chat/chatSlice'
import teamReduser from './reducers/teams/teamSlise'
import {createSocketMiddleware} from './reduxmiddleware/socketMiddleware'

const rootReducer = combineReducers({
    authReduser,
    chatReduser,
    teamReduser
})


export const store = configureStore({
    // reducer:{
    //     auth: authSlice.reducer // все состояния проложения тут 
    // }
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(createSocketMiddleware),
});

export type RootState = ReturnType<typeof store.getState>;// возвращает все состояния приложения 
export type AppDispatch = typeof store.dispatch;