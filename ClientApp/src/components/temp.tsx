import { Button, TextField } from '@mui/material'
import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import { useAppSelector } from '../store/hooks';
import * as signalR from "@microsoft/signalr";




export const Temp = () => {
    const [state, setState] = useState(false);
    const [messages, setMessages] = useState([{ message: '', author: '' }]);
    const [message, setMessage] = useState('');
    const [connection, setConnection] = useState<signalR.HubConnection>();


    const s = useAppSelector(state => state.authReduser.isAuthenticated);
    const username = useAppSelector((state) => state.authReduser.userName);

    const send = async() => {
        const response = await connection?.send("NewMessage", username, message);
        console.log(response);
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setMessage(event.target.value);
    };

    const openConnection = async () => {
        const conn = new signalR.HubConnectionBuilder()
            .withUrl("/hub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        conn.on("messageReceived", (username: string, message: string) => {
            setMessages(oldArray => [...oldArray, { message: message, author: username }]);
        });

        await conn.start();
        setConnection(conn)
    
    }

    if(!connection && !state){
        setState(true)
        openConnection();
    }

    return (
        <>
            <Link to='/game2'>game2</Link>
            <Link to='/quiz'>quiz</Link>
            Hey hey storage state is {s ? <>true</> : <>false</>}
            <br />But local state is {state ? <>true</> : <>false</>}
            <Button
                onClick={() => setState(!state)}
            >Update
            </Button>
            {messages.map(e =>
                <div key={e.author}>
                    {e.author}<br />{e.message}
                </div>)}
            <TextField
                id="outlined-multiline-static"
                label="Type your message"
                multiline
                rows={4}
                onChange={handleChange}
            />
            <Button
                onClick={send}
            >Send
            </Button>
        </>
    )
}
