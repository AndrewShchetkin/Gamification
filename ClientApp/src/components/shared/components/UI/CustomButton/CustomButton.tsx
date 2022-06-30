import React from 'react'
import classes from './CustomButton.module.css'

type ButtonProps = {

} & React.ComponentProps<'button'>

export const CustomButton = ({children , ...rest}: ButtonProps) => {
    return (
        <button className={classes.customBtn} {...rest}>
            {children}
        </button>
    )
}

