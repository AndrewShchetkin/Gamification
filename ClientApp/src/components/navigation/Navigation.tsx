
import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";
import classes from './Navigation.module.scss'
import Logo from '../../img/Logo.svg';

export default function Navigation() {

    const isAuthenticated = useAppSelector(state => state.authReduser.isAuthenticated);
    const userName = useAppSelector(state => state.authReduser.userName);

    return (
        <div className={classes.navBar}>
            <div className={classes.logo}>
                <Logo />
                <h2>The Game</h2>
            </div>
            <div className={classes.menu}>
                <ul>
                    <li><Link to={"/quiz"}>Quize</Link></li>
                    <li><Link to={"/Lobby"}>Lobby</Link></li>
                    <li>Help</li>
                </ul>
                {isAuthenticated ? <div className={classes.userName}>{userName}</div> : <div><Link to={"/Login"}>Login</Link></div>}
            </div>
        </div>)
}