import { createAsyncThunk } from "@reduxjs/toolkit";
import axios, { AxiosError } from "axios";
import { LoginResponse } from "../../../@types/loginResponse";
import { useAppDispatch } from "../../hooks";
import { endLoadData, signInComplete, startLoadData } from "./authSlice";

function isAxiosError(error: any): error is AxiosError {
    return error.isAxiosError === true;
}

export const fetchUser = createAsyncThunk(
    'get/api/user',
    async (_, thunkAPI) => {
        try {
            const response = await axios.get<LoginResponse>('api/auth/user')
            return response.data;
        } catch (e) {
            return thunkAPI.rejectWithValue(e);
        }
    }
)