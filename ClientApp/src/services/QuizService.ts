import * as signalR from '@microsoft/signalr';
import { Console } from 'console';
import * as quizeDto from './../components/quiz/quizeDto'
export default class QuizService {
    static async openQuizConnection(teamId: string,
        handleConnection: (connection: signalR.HubConnection) => void,
        handleQuestion :(question: quizeDto.Question)=> void,
        stopTimer: () => void,
        sendAnswer: () => void)
    {
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("/quizHub")
        .build();

        await connection.start();
        await connection.invoke("JoinTeamRound", teamId);

        connection.on("RoundOver", ()=>{
            stopTimer();
            sendAnswer();
        })

        connection.on("NewQuestion", (question)=>{
            handleQuestion(question);
        })
        handleConnection(connection);
        this.roundStart(connection, teamId);
    }
    static async closeConnection (connection?: signalR.HubConnection) {
        await connection?.stop();
    }
    static async roundStart(connection?: signalR.HubConnection, teamId?:string)
    {
        await connection?.invoke("RoundStart", teamId);
        console.log("RoundStart");
    }
    static async sendAnswer(connection?: signalR.HubConnection, userName?:string, asnwerId?: string)
    {
        connection?.invoke("NewUserAnswer", userName, asnwerId);
    }

}