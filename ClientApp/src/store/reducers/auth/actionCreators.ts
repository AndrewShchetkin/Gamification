import { createAsyncThunk } from "@reduxjs/toolkit";
import axios, { AxiosError } from "axios";
import { LoginResponse } from "../../../@types/loginResponse";
import { useAppDispatch } from "../../hooks";
import { endLoadData, signInComplete, startLoadData } from "./authSlice";

function isAxiosError(error: any): error is AxiosError {
    return error.isAxiosError === true;
}

export const fetchUser = createAsyncThunk('get/api/user', async (): Promise<void> => {
    const dispatch = useAppDispatch();
    dispatch(startLoadData());
    try {
        const response = await axios.get<LoginResponse>('api/auth/user');
        dispatch(signInComplete(response.data.username ?? ""));
    } catch (e) {
       if(isAxiosError(e)){
           if(e.response?.status != 401){
               console.error(e.message);
           }
       }
       else{
           console.error(e);
       }
    }
    dispatch(endLoadData());
});