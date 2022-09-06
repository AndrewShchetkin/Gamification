import { LogLevel } from "@microsoft/signalr";
import * as signalR from '@microsoft/signalr';
import { Message } from '../../@types/ReduxTypes/ChatState';
import { messageReceived } from '../reducers/chat/chatSlice'
import { ActionType } from "../../@types/ReduxTypes/ActionTypes";


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

    return (next: any) => (action: any) => {
        switch(action.type){
            case ActionType.StartConnection:
                    connection.start().then(()=>{
                        connection.invoke("ConnectToGeneralGroup");
                        debugger;
                        const state = storeAPI.getState();
                        console.log(storeAPI);
                        if(state.authReduser.teamId != ""){
                            connection.invoke("JoinTeamGroup", state.authReduser.teamId )
                        }
                    });
                break;
            case ActionType.SendMessage:
                // connection.send("SendMessage", action.payload, "generalGroup");
                debugger;
                connection.send("SendMessage", action.payload.message, action.payload.group);
                break;
        }

        return next(action);
    }

    // async sendMessage(connection?: signalR.HubConnection, message?:string, room?: string){
    //     if(room == "" && room == undefined){
    //         room = "generalRoom"
    //     }
    //     if(message != ""){
    //         await connection?.send("SendMessage", message, room);
    //     }

    // }
}
