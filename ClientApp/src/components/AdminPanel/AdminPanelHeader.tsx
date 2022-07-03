import React from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import classes from './styles/Header.module.css';

function AdminPanelHeader() {
    return (
        <div className={classes.header}>
            Викторины
            
        </div>
    );
}

export default AdminPanelHeader;