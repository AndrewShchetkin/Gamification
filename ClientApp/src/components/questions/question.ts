
export interface Question{
    id: string,
    title: string
    questionText: string,
    answers: Answer[]
}

export interface Answer{
    id: string,
    text: string
}