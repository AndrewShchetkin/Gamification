import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { ITeam } from '../../@types/ITeam';
import { IUser } from '../../@types/IUser';
import Chat from '../chat/Chat';
import ReusedList from '../shared/components/ReusedList'
import Tabs, { ITab } from '../shared/components/UI/CustomTab/Tabs';
import classes from './LobbyWhenUserInTeam.module.scss'
import { useAppDispatch, useAppSelector } from '../../store/hooks';

interface Props {
    teamId: number | string
}


function LobbyWhenUserInTeam(props: Props) {

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [selectedTab, setSelectedTab] = useState<string | number>(2);
    const [selectedChatTab, setSelectedChatTab] = useState<string | number>(1);
    const [isChatVisible, setIsChatVisible] = useState<boolean>(false);
    const dispatch = useAppDispatch();
    const usersTeam = useAppSelector(state => state.teamReduser.teams.find(team => team.id == props.teamId));
    
    const currentUser = useAppSelector(state => state.authReduser);
    const chatTabs: ITab[] =
        [
            { id: 1, header: 'Командный чат' },
            { id: 2, header: 'Общий чат' }
        ]


    const tabs: ITab[] = [
        { id: 1, header: "Карта" },
        { id: 2, header: "Игра" }
    ]

    const onTabClick = (selectedTab: string | number) => {
        setSelectedTab(selectedTab);
    }

    const onChatTabClick = (selectedTab: string | number) => {
        setSelectedChatTab(selectedTab);
    }

    const changeIsChatVisible = () => {
        setIsChatVisible(isChatVisible => !isChatVisible);
    }

    

    return (
        <div className={classes.wrapper}>
            <div className={classes.tabBlock}>
                <Tabs tabs={tabs} onClick={onTabClick} selectedTab={selectedTab}></Tabs>
            </div>
            <div className={classes.contentBlock}>
                {selectedTab === tabs[0].id && (
                    <p>Компонент карты будет тутава</p>
                )}
                {selectedTab === tabs[1].id && (
                    <>
                        <div className={classes.quizBlock}></div>
                        <div className={classes.teamBlock}>
                            <div className={classes.teamInfoBlock}>
                                <p>Команда "{usersTeam?.teamName}"</p>
                                <div className={classes.userItems}>
                                    <ReusedList items={usersTeam?.users} renderItem={(user: IUser) =>
                                        <div className={classes.userItem} key={user.id}>
                                            <div className={classes.userName}>{user.userName}</div>
                                            <button className={classes.userItemReadyButton} disabled={user.id != currentUser.id}>Готов</button>
                                        </div>
                                    }
                                    />
                                </div>
                            </div>
                            <div className={classes.chatBtn} onClick={changeIsChatVisible}></div>
                            {isChatVisible &&
                                <div className={classes.chatModal}>
                                    <div className={classes.chatTabBlock}>
                                        <Tabs tabs={chatTabs} onClick={onChatTabClick} selectedTab={selectedChatTab} />
                                    </div>
                                    {selectedChatTab === chatTabs[0].id && (
                                        <Chat group={currentUser.teamId.toString()} />
                                    )}
                                    {selectedChatTab === chatTabs[1].id && (
                                        <Chat group='generalGroup' />
                                    )}
                                    
                                </div>}

                        </div>
                    </>
                )}
            </div>
        </div>
    )
}

export default LobbyWhenUserInTeam
