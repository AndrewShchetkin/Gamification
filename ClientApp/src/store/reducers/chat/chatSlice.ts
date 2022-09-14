import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ChatState, Message } from "../../../@types/ReduxTypes/ChatState";
import { fetchMessageHistory } from "./actionCreators"


export const initialState: ChatState = {
    messages: [],
    requestSended: false,
    error: false
    
}

export const chatSlice = createSlice({
    name: 'chat',
    initialState,
    reducers:{
        messageReceived:(state:ChatState, action: PayloadAction<Message>) => {
            // при получении сообщения добавляем его в state 
            state.messages.push(action.payload); 
        },
    },
    extraReducers: {
        [fetchMessageHistory.fulfilled.type]: (state: ChatState, action: PayloadAction<Message[]>) => { // Данные получены
            state.messages = action.payload;
            state.requestSended = false;
        },
        [fetchMessageHistory.pending.type]: (state: ChatState) => { // идет запрос
            state.requestSended = true
        },
        [fetchMessageHistory.rejected.type]: (state: ChatState, action: PayloadAction) => { // ошибка
            // if (isAxiosError(action.payload)) {
            //     if (action.payload.response?.status != 401) {
            //         console.error(action.payload.message);
            //     }
            // }
            // else {
            //     console.error(action.payload)
            // }
            state.requestSended = false;
        }
    },
})


export default chatSlice.reducer;

export const { messageReceived } = chatSlice.actions;


// import { RootState } from '../../store';
// import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit'
// import { useAppDispatch } from '../../hooks';
// import signalR from '@microsoft/signalr';

// interface Message{
//     text: string,
//     author: string
// }


// export interface ChatState {
//     messages: Message[],
// }

// const initialState: ChatState = {
//     messages: [],
// }

// interface GetCurrentUserResponse{
//     isOk: boolean,
//     userName?: string,
//     error?: boolean
// }

// const connection = new signalR.HubConnectionBuilder()
//     .withUrl("/hub")
//     .configureLogging(signalR.LogLevel.Information)
//     .build();

// connection.on("messageReceived", (username: string, message: string) => {
    
// });

// connection.start().catch(err => document.write(err));



// export const authSlice = createSlice({
//     name: 'chat',
//     initialState,
//     reducers: {
//         send: (state: ChatState, action: PayloadAction<{username: string, message: string}> ) => {
//             connection.send("newMessage", action.payload.username, action.payload.message);
//         },
//     }
// });

// export const { send } = authSlice.actions;

// export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated

// export default authSlice.reducer;



// function useAppSelector(arg0: (state: any) => any) {
//     throw new Error('Function not implemented.');
// }

