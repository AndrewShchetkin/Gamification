import React from 'react';
import classes from './CustomModal.module.css';

function CustomModal({children, modal, setModal} : {children:JSX.Element,
    modal:boolean, setModal:any}) {

    const modalState: string[] = [classes.modal];
    if (modal)
        modalState.push(classes.active);
    
    return (
        <div className={modalState.join(' ')} onClick={() => 
            setModal(false)
        }>
            <div className={classes.modalContent} onClick={(e:any) => {
                e.stopPropagation()
            }}>
                {children}
            </div>
        </div>
    );
}

export default CustomModal;