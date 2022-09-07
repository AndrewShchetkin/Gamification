import React from 'react'
import classes from './index.module.scss'

export const LoadAnimation = () => {
    return (
        <div className={classes.flexAnimLine}>
            <div className={classes.flexAnimColumn}>
                <div className={classes.animateLogo}></div>
            </div>
        </div>
    )
}

