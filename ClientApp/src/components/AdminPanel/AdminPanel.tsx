import React, { useState } from 'react';
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