import { Box, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from '@mui/material'
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { IError } from '../../@types/IError';
import { ITeam } from '../../@types/ITeam';
import ReusedList from '../shared/components/ReusedList';
import CreateTeamForm from './CreateTeamForm';
import JoinToTeamForm from './JoinToTeamForm';
import TeamItem from './TeamItem';

function TeamsInfoWhenUserNotInTeam() {
    const [selectedTeam, setSelectedTeam] = useState<ITeam>({ id: 0, teamName: '', users: []})
    const [openCreateTeamForm, setOpenCreateTeamForm] = useState<boolean>(false);
    const [openJoinTeamForm, setOpenJoinTeamForm] = useState<boolean>(false);
    const [disableJoinButton, setDisableJoinButton] = useState<boolean>(true);
    const [teams, setTeams] = useState<ITeam[]>([]);
    const [errors, setErrors] = useState<IError[]>([]); //вспомнить зачем вот это 
    const [isLoading, setIsLoading] = useState<boolean>(true);

    async function fetchTeams() {
        try {
            const response = await axios.get<ITeam[]>('api/team/getallteams')
            setTeams(response.data);
        }
        catch (e) {
            console.log(e);
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        fetchTeams();
    }, [])

    const handleClickOpenCreateTeamForm = () => {
        setOpenCreateTeamForm(true);
    }

    const handleClickCloseCreateTeamForm = () => {
        setOpenCreateTeamForm(false);
    }

    const handleClickOpenJoinTeamForm = () => {
        setOpenJoinTeamForm(true);
    }

    const handleClickCloseJoinTeamForm = () => {
        setOpenJoinTeamForm(false);
    }

    // Обработчик нажатия по команде из списка
    const handleListIndexClick = (selectedTeamIndex: number) => {
        const team = teams.find(t => t.id == selectedTeamIndex);
        if(team){
            setSelectedTeam(team);
        }
        const numberOfUsers = team == undefined ? 0 : team.users.length;
        if (numberOfUsers > 4) {
            setDisableJoinButton(true);
        }
        else {
            setDisableJoinButton(false);
        }
    };

    return (
        <>
            {isLoading
                ? <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                    <CircularProgress />
                </Box>
                : <Box sx={{ flexGrow: 10, display: 'flex', flexDirection: 'column'}}>
                    <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
                        <ReusedList items={teams} renderItem={(team: ITeam) =>
                            <TeamItem
                                selectedIndex={selectedTeam.id}
                                onClickListItem={handleListIndexClick}
                                team={team}
                                users={team.users}
                                key={team.id}
                            />
                        }
                        />
                    </Box>
                    <Box sx={{
                        flexGrow: 1, bgcolor: '#fff', display: 'flex', alignItems: 'center', justifyContent: 'flex-end'
                    }}>
                        <Button
                            variant="outlined"
                            sx={{ mr: 1, ml: 1 }}
                            onClick={handleClickOpenJoinTeamForm}
                            disabled={disableJoinButton}
                        >Присоедениться</Button>
                        <Dialog open={openJoinTeamForm} onClose={handleClickCloseJoinTeamForm}>
                            <JoinToTeamForm team={selectedTeam} closeForm={handleClickCloseJoinTeamForm}/> 
                        </Dialog>
                        <Button variant="outlined" sx={{ mr: 1, ml: 1 }} onClick={handleClickOpenCreateTeamForm}>Создать</Button>
                        <Dialog open={openCreateTeamForm} onClose={handleClickCloseCreateTeamForm}>
                            <CreateTeamForm closeForm={handleClickCloseCreateTeamForm} />
                        </Dialog>
                    </Box>
                </Box>
            }
        </>
    )
}

export default TeamsInfoWhenUserNotInTeam
