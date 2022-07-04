import * as signalR from '@microsoft/signalr';
import * as quizeDto from './../components/quiz/quizeDto'
export default class QuizService {
    static async openQuizConnection(teamName: string, handleConnection: (connection: signalR.HubConnection) => void,handleQuestion :(question: quizeDto.Question)=> void)
    {
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("/quizHub")
        .build();

        await connection.start();
        await connection.invoke("JoinTeamRound", teamName);

        connection.on("NewQuestion", (question)=>{
            handleQuestion(question);
        })
        handleConnection(connection);
    }
    static async closeConnection (connection?: signalR.HubConnection) {
        await connection?.stop();
    }
    static async roundStart(connection?: signalR.HubConnection, teamName?:string)
    {
        await connection?.invoke("RoundStart", teamName);
        console.log("RoundStart");
    }

}