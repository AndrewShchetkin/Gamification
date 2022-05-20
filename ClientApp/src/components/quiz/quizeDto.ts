export interface QuizDto{
    question: Question[],
    team: User[],
}

export interface Question{
    text: string,
    answers: Answer[]
}

export interface Answer{
    text: string,
    id: number
}


export interface User{
    userName: string
}
