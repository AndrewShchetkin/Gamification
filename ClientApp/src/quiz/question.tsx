import * as React from 'react';

interface IQuestion{
    question: string
}

export default function Question({question}: IQuestion): JSX.Element{
    return (<div>
        {question}
    </div>)
}