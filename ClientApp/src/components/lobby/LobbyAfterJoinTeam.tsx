import { Container, CssBaseline } from '@mui/material'
import Box from '@mui/material/Box';
import React, { useEffect, useState } from 'react'
import { ITeam } from '../../@types/ITeam';




function LobbyAfterJoinTeam() {

    // Что нужно сделать : 
    // Получить команду, в которой состоит игрок 
    // Возможно придется написать метод в контроллер 
    const [team, setTeam] = useState<ITeam>(); // 

    /* useEffect() => {

    } */

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
                        Chat Block
                    </Box>
                    <Box className="teamsBlock"
                        sx={{
                            flexGrow: 2,
                            bgcolor: '#fff',
                            display: 'flex',
                            flexDirection: 'column'
                        }}>
                        <Box sx={{ flexGrow: 1, bgcolor: '#c1c7b7' }}>Информация о команде</Box>

                        <Box sx={{
                            flexGrow: 10, bgcolor: '#fff'
                        }}>
                                Тут инфа о команде после присоединения
                        </Box>
                    </Box>

                </Box>
                <Box className="contentFooter" sx={{ height: '10%', bgcolor: '#57535c' }}
                >Footer Content</Box>
            </Box>
        </Container>
    )
}

export default LobbyAfterJoinTeam
