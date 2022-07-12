import * as quizeDto from './quizeDto'
import React, { useEffect, useState } from 'react'
import QuizService from '../../services/QuizService';
import { useAppDispatch, useAppSelector } from '../../store/hooks';

export default function QuestionBox(){
    const teamId = useAppSelector(state => state.authReduser.teamId);
    const uuserName = useAppSelector(state => state.authReduser.userName);
    const [question, setQuestion] = useState<quizeDto.Question>();
    const [connection, setConnection] = useState<signalR.HubConnection>();
    const [asnwerId, setAnswerId] = useState<string | undefined>();
    const [time, setNewTime] = useState<number>(30);

    useEffect(() => {
        setNewTime(30);
        setAnswerId(undefined);
    }, [question])

    useEffect(()=>{
        if(time == 1 && asnwerId != undefined){
            sendAnswer();
        }
    },[time])

    useEffect(() => {
        function stopTimer(){
            clearInterval(interval);
        }
        function handleConnection(connection: signalR.HubConnection ){
            setConnection(connection);
        }
        function handleQuestion(question: quizeDto.Question){
            setQuestion(question);
            console.log("NewQuestion");
        }
        const interval = setInterval(() => {
            setNewTime(time => time - 1);
        }, 1000);
        QuizService.openQuizConnection(teamId, handleConnection, handleQuestion, stopTimer);
    }, [])

    const sendAnswer = async() =>{
        QuizService.sendAnswer(connection, uuserName, asnwerId);
        console.log("answer was send");
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
            <div>
            {question?.answers.map((element, index) =>
                <button
                    disabled = {element.id.toString() == asnwerId}
                    key = {index}
                    onClick={() => setAnswerId(element.id.toString())}>
                        {element.text}
                </button>       
            )}
            </div>
        </div>
    )
}