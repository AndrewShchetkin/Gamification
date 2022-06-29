import React, {FC, MouseEvent} from 'react';
import classes from './CustomModal.module.css';

export interface ICustomModal {

    children:JSX.Element,
    modal:boolean,
    setModal: React.Dispatch<React.SetStateAction<boolean>>;
}

const CustomModal:FC<ICustomModal> = ({children,modal, setModal}) => {

    const modalState: string[] = [classes.modal];
    if (modal)
        modalState.push(classes.active);
    
    return (
        <div className={modalState.join(' ')} onClick={() => 
            setModal(false)
        }>
            <div className={classes.modalContent} onClick={(e:MouseEvent<HTMLDivElement>) => {
                e.stopPropagation()
            }}>
                {children}
            </div>
        </div>
    );
}

export default CustomModal;