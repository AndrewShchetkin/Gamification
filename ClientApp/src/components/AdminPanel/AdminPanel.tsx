import React, { useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomTable from './QuizTable';
import AdminPanelBody from './AdminPanelBody';
import AdminPanelHeader from './AdminPanelHeader';

export default function AdminPanel() {

    return (
        <div>
            <AdminPanelHeader/>
            <hr />
            <AdminPanelBody/>
        </div>
    );
}