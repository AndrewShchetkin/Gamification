import React from 'react'
import classes from "./Tabs.module.css";

interface Props<T> {
    tabs: ITab[],
    selectedTab: string | number, 
    onClick: (selectedId: string | number) => void
}

export interface ITab { 
    id: string | number,
    header: string
}
function Tabs<T>(props: Props<T>) {
    const {tabs, selectedTab , onClick} = props

    return (
        <div className={classes.tabs}>
            {tabs && tabs.map(tab =>
            <div className={classes.tab} key={tab.id} onClick={() => onClick(tab.id)}>
                <div className={selectedTab == tab.id ? classes.active_header : classes.header }>
                    {tab.header}
                </div>
            </div>
            )}
        </div>
    )
}

export default Tabs
