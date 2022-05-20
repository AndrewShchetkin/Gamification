import * as React from 'react';
import {Question} from './../../@types/Quize/question'

export interface QuestionElementProps{
    text: string
}

const QuestionElement: React.FC<QuestionElementProps> = (props) => {
    return (<div>
        {props.text}
    </div>)
}

export default QuestionElement;