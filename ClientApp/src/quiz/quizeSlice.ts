import { Question } from './quizeDto';
import { createSlice } from '@reduxjs/toolkit'


export interface quizeState{
    currentQuestionIndex: number,
    questions: Question[]
}

export const initialState: quizeState = {
    currentQuestionIndex: 0,
    questions: [{text: 'First question', answers: []},
    {text: 'Second question', answers: []}] // TODO: remove after adding get question method
}

export const quizeSlice = createSlice({
    name: 'quize',
    initialState,
    reducers: {// функции которые меняют состояние 
        nextQuestion: state => {
            if (state.questions.length > state.currentQuestionIndex + 1 ) {
                state.currentQuestionIndex+=1;
                return;
            }
            alert('Questions are over');
        },
        // TODO: add get questions method
    }
});

export const { nextQuestion } = quizeSlice.actions;

export default quizeSlice.reducer;



