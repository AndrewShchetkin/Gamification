import React from 'react';
import classes from './CustomTextInput.module.css';

function CustomTextInput({...props}) {
    return (
        <input type="text" {...props} className={classes.inp} />
    );
}

export default CustomTextInput;