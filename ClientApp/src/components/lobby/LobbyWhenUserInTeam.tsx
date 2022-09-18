import axios from 'axios';
import { useEffect, useState } from 'react'
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
            <div className={classes.quizAndTeamInfoBlock}>
                <div className={classes.quizBlock}>
                    <div className={classes.title}>Игра</div>
                </div>
                <div className={classes.teamInfoBlock}>
                    <div className={classes.teamInfoBlockLeftPart}>
                        <div className={classes.title}>Команда</div>
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
                    <div className={classes.lineUp}>
                        <p>"{usersTeam?.teamName}"</p>
                        <div className={classes.userItems}>
                            <ReusedList items={usersTeam?.users} renderItem={(user: IUser) =>
                                <div className={classes.userItem} key={user.id}>
                                    {currentUser.id == user.id 
                                        ? (<div className={classes.currentUserIcon}></div>) 
                                        : (<div className={classes.noUserIcon}></div>)}
                                    <div className={classes.userName}>{user.userName}</div>
                                </div>
                            }
                            />
                        </div>
                    </div>
                </div>
            </div>
            <div className={classes.mapBlock}>
                <div className={classes.title}>Карта</div>
            </div>
        </div>
    )
}

export default LobbyWhenUserInTeam
