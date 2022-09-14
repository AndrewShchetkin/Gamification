import { LogLevel } from "@microsoft/signalr";
import * as signalR from '@microsoft/signalr';
import { Message } from '../../@types/ReduxTypes/ChatState';
import { messageReceived } from '../reducers/chat/chatSlice'
import { ActionType } from "../../@types/ReduxTypes/ActionTypes";
import { ITeam } from "../../@types/ITeam";
import { addTeam, updateTeam } from "../reducers/teams/teamSlise";


export const createSocketMiddleware = (storeAPI: any) => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/global")
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMessage", message => {
        const ChatMessage: Message = message;
        storeAPI.dispatch(messageReceived(ChatMessage))
        console.log(ChatMessage);
    });

    connection.on("AddTeam", team => {
        const NewTeam: ITeam = team;
        storeAPI.dispatch(addTeam(NewTeam))
    });

    connection.on("UpdateTeam", team => {
        const NewTeam: ITeam = team;
        storeAPI.dispatch(updateTeam(NewTeam))
    });

    return (next: any) => (action: any) => {
        switch (action.type) {
            case ActionType.StartConnection:
                connection.start().then(() => {
                    connection.invoke("ConnectToGeneralGroup");
                    const state = storeAPI.getState();
                    console.log(storeAPI);
                    if (state.authReduser.teamId != "") {
                        connection.invoke("JoinTeamGroup", state.authReduser.teamId)
                    }
                });
                break;
            case ActionType.SendMessage:
                connection.send("SendMessage", action.payload.message, action.payload.group);
                break;
            case ActionType.AddTeam:
                connection.send("AddTeam", action.payload);
                break;
            case ActionType.UpdateTeams:
                connection.send("UpdateTeam", action.payload);
                break;
        }

        return next(action);
    }
}
