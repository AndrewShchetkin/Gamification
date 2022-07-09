import * as quizeDto from './quizeDto'
import React, { useEffect, useState } from 'react'
import QuizService from '../../services/QuizService';
import { useAppDispatch, useAppSelector } from '../../store/hooks';

export default function QuestionBox(){
    const teamId = useAppSelector(state => state.authReduser.teamId);
    const uuserName = useAppSelector(state => state.authReduser.userName);
    const [question, setQuestion] = useState<quizeDto.Question>();
    const [connection, setConnection] = useState<signalR.HubConnection>();
    const [asnwerId, setAnswerId] = useState<string | undefined>('');
    const [time, setNewTime] = useState<number>(30);

    useEffect(() => {
        setNewTime(30);
    }, [question])

    useEffect(() => {
        sendAnswer(uuserName, asnwerId);
    }, [asnwerId])

    useEffect(()=>{
        if(time == -1)
            stopTimer()
    }, [time])

    useEffect(() => {
        function handleConnection(connection: signalR.HubConnection ){
            setConnection(connection);
        }
        function handleQuestion(question: quizeDto.Question){
            setQuestion(question);
            console.log("NewQuestion");
        }
        QuizService.openQuizConnection(teamId, handleConnection, handleQuestion);
    }, [])

    let interval: NodeJS.Timer;
    useEffect(()=>{
        interval = setInterval(() => {
            setNewTime(time => time - 1);
        }, 1000);
    }, [])

    function stopTimer(){
        clearInterval(interval);
    }
    
    const sendAnswer = async(userName?:string, asnwerId?: string) =>{
        QuizService.sendAnswer(connection, userName, asnwerId);
    }

    useEffect(() => {
        // при размонтировании компонента будет закрываться соединение
        return function cleanup(){
            QuizService.closeConnection(connection);
        }
     }, [connection])

    return(
        <div>
            <div>time left: {time} seconds</div>
            <div>
                {question?.text}
            </div>
            <button
            onClick={() => setAnswerId(question?.answers[0].id.toString())}>
                {question?.answers[0].text}
            </button>
            <button
            onClick={() => setAnswerId(question?.answers[1].id.toString())}>
                {question?.answers[1].text}
            </button>
            <button
             onClick={() => setAnswerId(question?.answers[2].id.toString())}>
                {question?.answers[2].text}
            </button>
            <button
            onClick={() => setAnswerId(question?.answers[3].id.toString())}>
                {question?.answers[3].text}
            </button>
        </div>
    )
}