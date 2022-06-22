import React, {} from "react";
import { Box, Container, CssBaseline } from '@mui/material'
import { useAppSelector } from '../../store/hooks';
import Chat from '../chat/Chat';
import TeamsInfoWhenUserInTeam from './TeamsInfoWhenUserInTeam';
import TeamsInfoWhenUserNotInTeam from './TeamsInfoWhenUserNotInTeam';
import { Temp } from "../temp";

function Lobby() {
    const teamId = useAppSelector(state => state.authReduser.teamId);

    return (
        <Container maxWidth="xl">
            <CssBaseline />
            <Box sx={{
                display: 'flex',
                height: '100vh',
                flexDirection: 'column'
            }}>
                <Box sx={{
                    display: "block",
                    bgcolor: 'primary.main',
                    mb: '10px',
                    flex: '0 0 10%'
                }}
                >Header</Box>
                <Box className="contentMiddle"
                    sx={{
                        display: 'flex',
                        flex: '1 1 auto'
                    }}>
                    {/* <Box className="chatBlock"
                        sx={{
                            flex: '0 0 30%',
                            maxWidth: "30%"
                        }}>
                        <Chat />
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
                        {teamId ? <TeamsInfoWhenUserInTeam teamId={teamId} /> : <TeamsInfoWhenUserNotInTeam />}
                    </Box> */}
                    {teamId ? <TeamsInfoWhenUserInTeam teamId={teamId} /> : <TeamsInfoWhenUserNotInTeam />}
                </Box>
                <Box className="contentFooter" sx={{ 
                    height: '10%', 
                    bgcolor: '#57535c' 
                }}>Footer Content</Box>
            </Box>
        </Container>
    )
}

export default Lobby
