import {Question}from './../Quize/question'

export interface QuizeState{
    questionsIsLoaded: boolean,
    requestSended: boolean,
    currentQuestionIndex: number,
    questions: Question[]
}