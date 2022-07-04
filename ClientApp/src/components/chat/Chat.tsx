import * as signalR from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import ChatService from '../../services/ChatService';
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import {CustomButton} from '../shared/components/UI/CustomButton/CustomButton';
import { MessageLeft, MessageRight } from './Message';
import classes from './Chat.module.scss';

export interface ChatMessage { 
     author: string,
     text: string,
     dispatchTime: Date  
}

interface Props{
    chatRoom: string // в качестве названия группы будем использовать id команды 
}

function Chat(props: Props) {
    const {chatRoom} = props
    const [message, setMessage] = useState<string>('');
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const [connection, setConnection] = useState<signalR.HubConnection>();
    const userName = useAppSelector(state => state.authReduser.userName);
    

    const handleTextAreaChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setMessage(event.target.value);
    };

    //Запрос на загрузку всех сообщений из БД
    async function fetchAllCommonMessages() {
        try {
            const response = await axios.get<ChatMessage[]>('api/messages/getAllCommonMessages');
            setMessages(response.data);
        }
        catch (e) {
            console.log(e);
        }
        finally {
            console.log("Загрузка завершена");
        }
    }

    // пока не используется 
    async function saveMessageToDatabase(author: string, text: string ) {
        const body = {
            author: author,
            text: text
        };
        try {
            const response = await axios.post<ChatMessage>('api/messages/addmessage', body)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            console.log("Загрузка завершена");
        }
    }



    useEffect(() => {
        // При первоначальном рендере запрашиваем все сообшения из БД, устанавливаем соединение signalr
        fetchAllCommonMessages();
        function handleConnection(connection: signalR.HubConnection ){
            setConnection(connection);
        }
        function handleMessage(message: ChatMessage){
            setMessages(messages => [...messages , message]);
        }
        ChatService.openConnection(chatRoom, handleConnection, handleMessage);
     }, [])

    useEffect(() => {
        // при размонтировании компонента будет закрываться соединение
        return function cleanup(){
            ChatService.closeConnection(connection);
        }
     }, [connection])

    const sendMessage = async() => {
          await ChatService.sendMessage(connection, message, chatRoom);
          setMessage("");
    };

    return (
        <div className={classes.chatContent}>
            <div className={classes.messageContent}>   
            {messages.map( (item) =>{
                    if(item.author == userName){
                       return <MessageRight message={item.text} userName={item.author}/>
                    }
                    else{
                        return <MessageLeft message={item.text} userName={item.author}></MessageLeft>
                    }
                }
            )}      
            </div>
            <div className={classes.inputContent}>
                <textarea className={classes.chatTextArea}
                    onChange={handleTextAreaChange}
                    value={message}
                    placeholder="Введите сообщение..."/>
                <div className={classes.sendBtn} 
                    style={{flex: '0 0 10%'}}
                    onClick={sendMessage}
                >
                </div>
            </div>
        </div>
    )
}

export default Chat

//  Material UI
//     return (
//         <Box sx={{
//             border: 2, borderColor: 'primary.main', borderRadius: 2,
//             display: 'flex',
//             flexDirection: 'column',
//             height: '100%'
//         }}>
//             <Box sx={{
//                 flex: "1 1 auto"
//             }}>
//                 {/* <MessageLeft message={"sdfsdf"} userName={"sdfsf"} />
//                 <MessageRight message={"sdfsdf"} userName={"sdfsf"} /> */}
//                 {messages.map( (item) =>{
//                     debugger;
//                     if(item.userName == "admin"){
//                        return <MessageRight message={item.message} userName={item.userName}/>
//                     }
//                     else{
//                         return <MessageLeft message={item.message} userName={item.userName}></MessageLeft>
//                     }
//                 }
                    
//                 )}
//             </Box>
//             <Box sx={{
//                 flex: '0 0 15%',
//                 display: 'flex',
//                 alignItems: 'center'
//             }}
//             >
//                 <TextField fullWidth multiline label='Введите сообщение...' sx={{ ml: 3, mr: 2 }} maxRows={3} inputProps={{}} />

//                 <IconButton
//                     aria-label="menu"
//                     sx={{
//                         flex: '0 0 10%',
//                         mr: 2
//                     }}>
//                     <SendRoundedIcon />
//                 </IconButton>
//             </Box>
//         </Box>
//     )
// }

// export default Chat
