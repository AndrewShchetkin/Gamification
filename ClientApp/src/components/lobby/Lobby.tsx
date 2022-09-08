import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import LobbyWhenUserInTeam from './LobbyWhenUserInTeam';
import LobbyWhenUserNotInTeam from './LobbyWhenUserNotInTeam';
import classes from './Lobby.module.scss'
import Header from "../header/Header";
import { ActionType } from "../../@types/ReduxTypes/ActionTypes";
import { fetchMessageHistory } from "../../store/reducers/chat/actionCreators";
import { fetchTeams } from "../../store/reducers/teams/actionCreators";


function Lobby() {
    const teamId = useAppSelector(state => state.authReduser.teamId);
    const dispatch = useAppDispatch();

    // При первоначальном рендере запрашиваем все сообшения из БД, устанавливаем соединение signalr
    useEffect(() => {
        dispatch({ 
            type: ActionType.StartConnection, payload: null 
        });
        dispatch(fetchTeams());
        dispatch(fetchMessageHistory());
    }, [])

    return (
        <>
            <Header />
            <div className={classes.wrapper}>
                <div className={classes.container}>
                    <div className={classes.content}>
                        {teamId ? <LobbyWhenUserInTeam teamId={teamId} /> : <LobbyWhenUserNotInTeam />}
                    </div>
                </div>
            </div>
        </>

    )
}

export default Lobby
