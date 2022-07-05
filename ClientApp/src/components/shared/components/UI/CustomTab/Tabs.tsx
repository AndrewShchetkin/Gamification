import React from 'react'
import classes from "./Tabs.module.scss";

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
    const widthInPersent: string = (100/tabs.length).toString() + "%";


    return (
        <div className={classes.tabs}>
            {tabs && tabs.map(tab =>
            <div className={classes.tab} key={tab.id} onClick={() => onClick(tab.id)} style={{width: widthInPersent}}>
                <div className={selectedTab == tab.id ? classes.active_header : classes.header }>
                   <p>{tab.header}</p> 
                </div>
            </div>
            )}
        </div>
    )
}

export default Tabs
