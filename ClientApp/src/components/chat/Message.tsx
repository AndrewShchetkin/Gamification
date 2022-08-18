import React from 'react'
import classes from './Chat.module.scss';

interface Props {
    message: string,
    userName: string
}

export function MessageLeft(props: Props) {
    const {message, userName} = props

    return (
        <div className={classes.messageLeftRow}>
            <div className={classes.messageLeft}>
                <div>
                    <div className={classes.messageLeftAuthor}>{userName}</div>
                    <div className={classes.messageText}>{message}</div>
                </div>
            </div>
        </div>
    )
}

export function MessageRight(props: Props) {
    const {message, userName} = props

    return (
        <div className={classes.messageRightRow}>
            <div className={classes.messageRigth}>
                <div>
                    <div className={classes.messageRightAuthor}>{userName}</div>
                    <div className={classes.messageText}>{message}</div>
                </div>
            </div>
        </div>
    )
}

