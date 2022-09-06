export interface ChatState {
    messages: Message[],
    requestSended: boolean,
    error: boolean
}

export interface Message{
    author: string,
    text: string,
    dispatchTime: Date,
    group: string
}