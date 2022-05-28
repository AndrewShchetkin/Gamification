import { Box, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from '@mui/material'
import { blue } from '@mui/material/colors';
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { ITeam } from '../../@types/ITeam';
import { IUser } from '../../@types/IUser';
import ReusedList from '../shared/components/ReusedList'
import TeamItem from './TeamItem'
import UserItem from './UserItem';

interface Props {
    teamId: string
}


function TeamsInfoWhenUserInTeam(props: Props) {

    const [usersTeam, setUsersTeam] = useState<ITeam>({
        id: 1,
        teamName: '',
        users : [{
            id: "1",
            userName : '',
            teamId: ''
        }]
    });
    const [isLoading, setIsLoading] = useState<boolean>(true);

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
        <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
            {isLoading
                ? <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                    <CircularProgress />
                </Box>

                : <Box sx={{ height: "100%", display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                    <Typography sx={{ mt: 10, display: 'flex', justifyContent: 'center' }} >{usersTeam.teamName}</Typography>
                    <Box sx={{border: 2, borderColor: 'primary.main', borderRadius: 2,  mt: 2, display: 'flex', justifyContent: 'center', width: '33%' }}>
                        <ReusedList items={usersTeam.users} renderItem={(user: IUser) =>
                            <UserItem user={user} key={user.id} />}
                        />
                    </Box>
                </Box>
            }
        </Box>
    )
}

export default TeamsInfoWhenUserInTeam
