import React from 'react'
import classes from './CustomInput.module.scss'

type ButtomProps = {

} & React.ComponentProps<'input'>

export const CustomInput = ({...rest}: ButtomProps) => {
    return (
        <input className={classes.customInput} {...rest}>
        </input>
    )
}