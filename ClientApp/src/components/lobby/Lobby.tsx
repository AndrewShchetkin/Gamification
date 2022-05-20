import { Box, Container, CssBaseline } from '@mui/material'
import axios from 'axios'
import React, { useEffect, useState } from 'react'
import Chat from './Chat'
import TeamsInfoWhenUserInTeam from './TeamsInfoWhenUserInTeam';
import TeamsInfoWhenUserNotInTeam from './TeamsInfoWhenUserNotInTeam';



function Lobby() {
    const [isUserInTeam , setIsUserInTeam] = useState<boolean>(false);
    const [userTeamId, setUserTeamId] = useState<string>("");
    // Временное решение, запрашиваем команду текущего пользователя
    const fetchUserTeam = async () => {
        debugger;
        try {
            const response = await axios.get('api/auth/getUserTeam');
            if(response.data.teamId != null){
                setIsUserInTeam(true);
                setUserTeamId(response.data.teamId );
            }
            else{
                setIsUserInTeam(false);
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    useEffect(() => {
        fetchUserTeam();
    }, [userTeamId])

    const updateTeam = (updateFlag: boolean) => {
        setIsUserInTeam(updateFlag);
    }

    return (
        <Container maxWidth="xl">
        <CssBaseline />
        <Box sx={{
            bgcolor: '#cfe8fc',
            display: 'flex',
            height: '100vh',
            flexDirection: 'column'
        }}>
            <Box sx={{
                display: "block",
                bgcolor: 'primary.main',
                height: '10%',
                mb: '10px'
            }}
            >Header</Box>
            <Box className="contentMiddle"
                sx={{
                    height: '80%',
                    display: 'flex'
                }}
            >
                <Box className="chatBlock"
                    sx={{
                        flexGrow: 1,
                        bgcolor: '#98bf93'
                    }}>
                    <Chat/>
                </Box>
                <Box className="teamsBlock"
                    sx={{
                        flexGrow: 2,
                        bgcolor: '#fff',
                        display: 'flex',
                        flexDirection: 'column'
                    }}>
                    <Box sx={{ flexGrow: 1, bgcolor: '#c1c7b7' }}>Информация о командах</Box>
                    {isUserInTeam ? <TeamsInfoWhenUserInTeam teamId={userTeamId}/> : <TeamsInfoWhenUserNotInTeam updateTeam={updateTeam} />}
                </Box>

            </Box>
            <Box className="contentFooter" sx={{ height: '10%', bgcolor: '#57535c' }}
            >Footer Content</Box>
        </Box>
    </Container> 
    )
}

export default Lobby
