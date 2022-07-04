import React from 'react';
import classes from './CustomModal.module.scss'

interface Props {
   children: React.ReactNode, 
   visible: boolean,
   setVisible: (visible:boolean) => void
}

function CustomModal(props: Props) {
    const {children, visible, setVisible} = props
    const rootClasses= [classes.customModal];
    if(visible){
        rootClasses.push(classes.active)
    }

    return (
        <div className={rootClasses.join(' ')} onClick={()=> setVisible(false)}>
            <div className={classes.customModalContent} onClick={(e)=> e.stopPropagation()}>
                {children}
            </div>
        </div>
    )
}

export default CustomModal
