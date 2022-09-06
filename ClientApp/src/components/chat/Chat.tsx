import React, { useEffect, useState } from 'react'
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import { MessageLeft, MessageRight } from './Message';
import classes from './Chat.module.scss';
import { ActionType } from '../../@types/ReduxTypes/ActionTypes';

interface Props {
    group: string // в качестве названия группы будем использовать id команды 
}

function Chat(props: Props) {
    const { group } = props
    const [message, setMessage] = useState<string>('');
    const userName = useAppSelector(state => state.authReduser.userName);
    let allMessages = useAppSelector(state => state.chatReduser.messages);
    let messages = allMessages.filter( message => message.group == group);
    const dispatch = useAppDispatch();

    const handleTextAreaChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setMessage(event.target.value);
    };

    const sendMessage = () => {
        dispatch({ type: ActionType.SendMessage, payload: {message, group} });
        setMessage("");
    };

    return (
        <div className={classes.chatContent}>
            <div className={classes.messageContent}>
                {messages.map((item) => {
                    if (item.author == userName) {
                        return <MessageRight message={item.text} userName={item.author} />
                    }
                    else {
                        return <MessageLeft message={item.text} userName={item.author}></MessageLeft>
                    }
                }
                )}
            </div>
            <div className={classes.inputContent}>
                <textarea className={classes.chatTextArea}
                    onChange={handleTextAreaChange}
                    value={message}
                    placeholder="Введите сообщение..." />
                <div className={classes.sendBtn}
                    style={{ flex: '0 0 10%' }}
                    onClick={sendMessage}
                >
                </div>
            </div>
        </div>
    )
}

export default Chat

