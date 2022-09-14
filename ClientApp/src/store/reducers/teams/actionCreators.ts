import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";
import { ITeam } from "../../../@types/ITeam";


export const fetchTeams = createAsyncThunk(
    'get/api/team/getallteams',
    async (_, thunkAPI) => {
        try {
            const response = await axios.get<ITeam[]>('api/team/getallteams')
            return response.data;
        } catch (e) {
            return thunkAPI.rejectWithValue(e);
        }
    }
)


