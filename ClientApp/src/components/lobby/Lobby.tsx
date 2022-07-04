import React, {} from "react";
import { useAppSelector } from '../../store/hooks';
import Chat from '../chat/Chat';
import TeamsInfoWhenUserInTeam from './TeamsInfoWhenUserInTeam';
import TeamsInfoWhenUserNotInTeam from './TeamsInfoWhenUserNotInTeam';
import { Temp } from "../temp";
import classes from './Lobby.module.scss'


function Lobby() {
    const teamId = useAppSelector(state => state.authReduser.teamId);

    return (
    <div className={classes.wrapper}>
        <div className={classes.container}>
            <div className={classes.header}>header content</div>
            <div className={classes.content}>
                {teamId ? <TeamsInfoWhenUserInTeam teamId={teamId} /> : <TeamsInfoWhenUserNotInTeam />}
            </div>
            <div className={classes.footer}>footer content</div>
        </div>
    </div>
    )
}

export default Lobby
