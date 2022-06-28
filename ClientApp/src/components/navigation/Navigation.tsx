
import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";
import classes from './Navigation.module.css'

export default function Navigation() {

    const isAuthenticated = useAppSelector(state => state.authReduser.isAuthenticated);
    const userName = useAppSelector(state => state.authReduser.userName);

    return (
        <div className={classes.navBar}>
            <div>
                <h2>The Game</h2>
            </div>
            <ul>
                <li><Link to={"/quiz"}>Quize</Link></li>
                <li><Link to={"/Lobby"}>Lobby</Link></li>
                <li>Help</li>
            </ul>
            {isAuthenticated ? <div>{userName}</div> : <div><Link to={"/Login"}>Login</Link>Login</div>}
        </div>)
}