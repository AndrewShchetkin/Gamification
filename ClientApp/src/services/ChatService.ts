import { LogLevel } from "@microsoft/signalr";
import * as signalR from '@microsoft/signalr';
import { ChatMessage } from "../components/chat/Chat";
export default class ChatService {
    static async openConnection(room: string, handleConnection: (connection: signalR.HubConnection) => void, handleMessage :(message: ChatMessage)=> void) {
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(LogLevel.Information)
        .build();

        await connection.start();
        await connection.invoke("JoinRoom", room);

        connection.on("ReceiveMessage", (author, text, dispatchTime ) => {
            debugger;
            const ChatMessage : ChatMessage = { 
                author: author,
                text: text, 
                dispatchTime: dispatchTime
            }
            handleMessage(ChatMessage)
        });

        connection.on("RoundEnded", () => {
            connection.stop();
        })

        handleConnection(connection);
    }

    static async closeConnection (connection?: signalR.HubConnection) {
        await connection?.stop();
    }
    static async sendMessage(connection?: signalR.HubConnection, message?:string, room?: string){
        if(room == "" && room == undefined){
            room = "generalRoom"
        }
        if(message != ""){
            await connection?.send("SendMessage", message, room);
        }
        
    }
}