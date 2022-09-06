import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";
import { Message } from "../../../@types/ReduxTypes/ChatState";


export const fetchMessageHistory = createAsyncThunk(
    'get/api/messages/getAllCommonMessages',
    async (_, thunkAPI) => {
        try {
            const response = await axios.get<Message[]>('api/messages/getAllCommonMessages');
            return response.data;
        } catch (e) {
            return thunkAPI.rejectWithValue(e);
        }
    }
)