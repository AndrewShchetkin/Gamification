import React, { Component } from 'react';
import { Question } from './question';

interface QuestionsState{
    questions: Question[]
    activeQuestion: Question,

}
export class Questions extends Component<{}, QuestionsState>{
    static displayName = Questions.name;

    render (){
        return (
            <div>
                <h3>{this.state.activeQuestion.title}</h3>
                <div>
                    <div>{this.state.questions.map(q =>{
                        // TODO: create block with link to question
                    })}</div>
                </div>
            </div>
        )
    }
}