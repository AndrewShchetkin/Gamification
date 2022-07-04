import * as quizeDto from './quizeDto'
import React, { useEffect, useState } from 'react'
import QuizService from '../../services/QuizService';
import { useAppDispatch, useAppSelector } from '../../store/hooks';

interface Props{
    teamName: string
}

export default function QuestionBox(props: Props){
    const userName = useAppSelector(state => state.authReduser.userName);
    const [question, setQuestion] = useState<quizeDto.Question>();
    const [connection, setConnection] = useState<signalR.HubConnection>();

    useEffect(() => {
        function handleConnection(connection: signalR.HubConnection ){
            setConnection(connection);
        }
        function handleQuestion(question: quizeDto.Question){
            setQuestion(question);
            console.log("NewQuestion");
        }
        QuizService.openQuizConnection(props.teamName, handleConnection, handleQuestion);
        
    }, [])
    useEffect(() => {
        QuizService.roundStart(connection, props.teamName) //временное решение
        function cleanup(){
            QuizService.closeConnection(connection);
        }
    }, [connection])

    return(
        <div>
            <div>
                {question?.text}
            </div>
            <button>
                {question?.answers[0].text}
            </button>
            <button>
                {question?.answers[1].text}
            </button>
            <button>
                {question?.answers[2].text}
            </button>
            <button>
                {question?.answers[3].text}
            </button>
        </div>
    )
}