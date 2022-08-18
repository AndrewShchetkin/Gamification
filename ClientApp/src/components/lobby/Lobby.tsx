import React, { } from "react";
import { useAppSelector } from '../../store/hooks';
import Chat from '../chat/Chat';
import LobbyWhenUserInTeam from './LobbyWhenUserInTeam';
import LobbyWhenUserNotInTeam from './LobbyWhenUserNotInTeam';
import classes from './Lobby.module.scss'
import Header from "../header/Header";


function Lobby() {
    const teamId = useAppSelector(state => state.authReduser.teamId);

    return (
        <>
            <Header/>
            <div className={classes.wrapper}>
                <div className={classes.container}>
                    {/* <div className={classes.header}>header content</div> */}
                    <div className={classes.content}>
                        {teamId ? <LobbyWhenUserInTeam teamId={teamId} /> : <LobbyWhenUserNotInTeam />}
                    </div>
                    {/* <div className={classes.footer}>footer content</div> */}
                </div>
            </div>
        </>

    )
}

export default Lobby
