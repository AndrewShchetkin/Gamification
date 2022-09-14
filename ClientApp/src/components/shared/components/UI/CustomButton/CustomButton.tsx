import React from 'react'
import classes from './CustomButton.module.scss'

type ButtomProps = {

} & React.ComponentProps<'button'>

export const CustomButton = ({children , ...rest}: ButtomProps) => {
    return (
        <button className={classes.customBtn} {...rest}>
            {children}
        </button>
    )
}

