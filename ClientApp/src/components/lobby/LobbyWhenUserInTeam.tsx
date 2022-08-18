import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { ITeam } from '../../@types/ITeam';
import { IUser } from '../../@types/IUser';
import Chat from '../chat/Chat';
import ReusedList from '../shared/components/ReusedList'
import Tabs, { ITab } from '../shared/components/UI/CustomTab/Tabs';
import classes from './LobbyWhenUserInTeam.module.scss'
import { useAppSelector } from '../../store/hooks';

interface Props {
    teamId: string
}


function LobbyWhenUserInTeam(props: Props) {

    const [usersTeam, setUsersTeam] = useState<ITeam>();
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [selectedTab, setSelectedTab] = useState<string | number>(2);
    const [selectedChatTab, setSelectedChatTab] = useState<string | number>(1);
    const [isChatVisible, setIsChatVisible] = useState<boolean>(false);

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

    const fetchUserTeam = async () => {
        try {
            const response = await axios.get('api/team/getTeamByID', { params: { teamID: props.teamId } });
            setUsersTeam(response.data);
            setIsLoading(false);
        }
        catch (e) {
            console.log(e);
        }
    }

    const changeIsChatVisible = () => {
        setIsChatVisible(isChatVisible => !isChatVisible);
    }

    useEffect(() => {
        fetchUserTeam();
    }, [])

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
                                        <Chat chatRoom={currentUser.teamId} />
                                    )}
                                    {selectedChatTab === chatTabs[1].id && (
                                        <Chat chatRoom='generalRoom' />
                                    )}
                                    
                                </div>}

                        </div>
                    </>
                )}
            </div>
        </div>
        /* <Box className="chatBlock"
            sx={{
                flex: '0 0 30%',
                maxWidth: "30%",
                display: "flex",
                flexDirection: 'column'
            }}>
            <Tabs tabs={chatTabs} onClick={onTabClick} selectedTab={selectedTab} />
            {selectedTab === chatTabs[0].id && (
                <Chat chatRoom={props.teamId}/>
            )}
            {selectedTab === chatTabs[1].id && (
                <Chat chatRoom='generalRoom'/>
            )}
        </Box>


        <Box className="teamsBlock"
            sx={{
                bgcolor: '#fff',
                display: 'flex',
                flex: '1 1 auto',
                flexDirection: 'column'
            }}>
            <Box sx={{
                flex: '0 0 10%',
                bgcolor: '#c1c7b7'
            }}>Информация о командах</Box>
            <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
                {isLoading
                    ? <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                        <CircularProgress />
                    </Box>

                    : <Box sx={{ height: "100%", display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                        <Typography sx={{ mt: 10, display: 'flex', justifyContent: 'center' }} >{usersTeam.teamName}</Typography>
                        <Box sx={{ border: 2, borderColor: 'primary.main', borderRadius: 2, mt: 2, display: 'flex', justifyContent: 'center', width: '33%' }}>
                            <ReusedList items={usersTeam.users} renderItem={(user: IUser) =>
                                <UserItem user={user} key={user.id} />}
                            />
                        </Box>
                    </Box>
                }
            </Box>
        </Box> */
    )
}

export default LobbyWhenUserInTeam
