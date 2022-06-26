import React from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';

function AdminPanelHeader() {
    return (
        <div style={{display:'flex', justifyContent:'space-between',
            alignItems:'center'}}>
            <div>Викторины</div>
            <CustomButton>Загрузить викторину</CustomButton>
        </div>
    );
}

export default AdminPanelHeader;