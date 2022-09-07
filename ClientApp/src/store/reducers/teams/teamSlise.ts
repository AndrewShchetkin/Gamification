import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ITeam } from "../../../@types/ITeam";
import { TeamsState } from "../../../@types/ReduxTypes/TeamsState";
import { fetchTeams } from "./actionCreators";

export const initialState: TeamsState = {
    teams: [],
    requestSended: false,
    error: false
    
}

export const teamSlice = createSlice({
    name: 'team',
    initialState,
     reducers:{
        updateTeam:(state:TeamsState, action: PayloadAction<ITeam>) => {
            state.teams.push(action.payload); 
        },
    },
    extraReducers: {
        [fetchTeams.fulfilled.type]: (state: TeamsState, action: PayloadAction<ITeam[]>) => { // Данные получены
            state.teams = action.payload;
            state.requestSended = false;
        },
        [fetchTeams.pending.type]: (state: TeamsState) => { // идет запрос
            state.requestSended = true
        },
        [fetchTeams.rejected.type]: (state: TeamsState, action: PayloadAction) => { // ошибка
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


export default teamSlice.reducer;

export const { updateTeam } = teamSlice.actions;