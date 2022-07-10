import { Box, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from '@mui/material'
import { blue } from '@mui/material/colors';
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { ITeam } from '../../@types/ITeam';
import { IUser } from '../../@types/IUser';
import Chat from '../chat/Chat';
import ReusedList from '../shared/components/ReusedList'
import Tabs, { ITab } from '../shared/components/UI/CustomTab/Tabs';
import TeamItem from './TeamItem'
import UserItem from './UserItem';

interface Props {
    teamId: string
}


function TeamsInfoWhenUserInTeam(props: Props) {

    const [usersTeam, setUsersTeam] = useState<ITeam>({
        id: 1,
        teamName: '',
        users: [{
            id: "1",
            userName: '',
            teamId: '',
            role:''
        }]
    });
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [selectedTab , setSelectedTab] = useState<string|number>(1);

    const chatTabs: ITab[] = [
        {id: 1, header: 'Командный чат'},
        {id: 2, header: 'Общий чат'}]
    
    
    const onTabClick = (selectedTab: string | number) =>{
        setSelectedTab(selectedTab);
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

    useEffect(() => {
        fetchUserTeam();
    }, [])

    return (
        <>
            <Box className="chatBlock"
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
            </Box>

        </>

    )
}

export default TeamsInfoWhenUserInTeam
