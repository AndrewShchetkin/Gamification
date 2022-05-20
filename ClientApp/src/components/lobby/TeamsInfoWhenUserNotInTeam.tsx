import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from '@mui/material'
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { IError } from '../../@types/IError';
import { ITeam } from '../../@types/ITeam';
import ReusedList from '../shared/components/ReusedList';
import TeamItem from './TeamItem';

interface Props{
    updateTeam: (flag: boolean) => void
}


function TeamsInfoWhenUserNotInTeam(props: Props) {

    const [selectedTeam, setSelectedTeam] = useState(0);
    const [openCreateTeamForm, setOpenCreateTeamForm] = useState<boolean>(false);
    const [openJoinTeamForm, setOpenJoinTeamForm] = useState<boolean>(false);
    const [disableJoinButton, setDisableJoinButton] = useState<boolean>(true);
    const [disableConfirmButton, setDisableConfirmButton] = useState<boolean>(true);
    const [showErrorMessage, setShowErrorMessage] = useState<boolean>(true); // вспомнить зачем это 
    const [teams, setTeams] = useState<ITeam[]>([]);
    const [errors, setErrors] = useState<IError[]>([]); //вспомнить зачем вот это 
    
    async function fetchTeams() {
        try {
            const response = await axios.get<ITeam[]>('api/team/getallteams')
            setTeams(response.data);
        }
        catch (e) {
            console.log(e);
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
        setSelectedTeam(selectedTeamIndex);
        const selectedTeam = teams.find(t => t.id == selectedTeamIndex);
        const numberOfUsers = selectedTeam == undefined ? 0 : selectedTeam.users.length;
        if (numberOfUsers > 4) {
            setDisableJoinButton(true);
        }
        else {
            setDisableJoinButton(false);
        }
    };

    // Обработчик создания команды
    const handleCreateFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const body = {
            TeamName: data.get('teamName'),
            Password: data.get('teamPassword')
        };
        debugger;
        const response = await axios.post('api/team/teamregister', body)
            .then(function (response) {
                // fetchTeams();  Скорее всего не нужно, так как будет перенаправление на другой компонент 
                props.updateTeam(true);
                handleClickCloseCreateTeamForm();
            })
            .catch(function (error) {
                debugger;
                const errors: IError[] = error.response.data.errors;
                setErrors(errors);
            });
    }

    // Обработчик присоединения к команде команды
    const handleJoinFormSubmit = async (event: React.FormEvent<HTMLFormElement>, teamId: number) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const body = {
            teamId: teamId,
            password: data.get('teamPassword')
        };
        const response = await axios.post('api/team/jointheteam', body)
            .then(function () {
                // fetchTeams(); 
                props.updateTeam(true);
                handleClickCloseJoinTeamForm();
            })
            .catch(function (error) {
                console.log(error);
            });
    }
    
    return (
        <>

            <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
                <ReusedList items={teams} renderItem={(team: ITeam) =>
                    <TeamItem
                        selectedIndex={selectedTeam}
                        onClickListItem={(selectedTeam) => handleListIndexClick(selectedTeam)}
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
                    <Box component="form" onSubmit={(event: React.FormEvent<HTMLFormElement>) => handleJoinFormSubmit(event, selectedTeam)}>
                        <DialogTitle>Присоедениться к команде {teams.find(x => x.id == selectedTeam)?.teamName} </DialogTitle>
                        <DialogContent>
                            <TextField
                                autoFocus
                                name="teamPassword"
                                margin="dense"
                                id="teamName"
                                label="Введите пароль команды"
                                type="password"
                                fullWidth
                                variant="outlined"
                            />
                        </DialogContent>

                        <DialogActions>
                            <Button onClick={handleClickCloseJoinTeamForm}>Отмена</Button>
                            <Button type="submit">Подтвердить</Button>
                        </DialogActions>
                    </Box>
                </Dialog>
                <Button variant="outlined" sx={{ mr: 1, ml: 1 }} onClick={handleClickOpenCreateTeamForm}>Создать</Button>
                <Dialog open={openCreateTeamForm} onClose={handleClickCloseCreateTeamForm}>
                    <Box component="form" onSubmit={handleCreateFormSubmit}>
                        <DialogTitle>Создать команду</DialogTitle>
                        <DialogContent>
                            <TextField
                                autoFocus
                                autoComplete='off'
                                name="teamName"
                                margin="normal"
                                id="teamName"
                                label="Введите название команды"
                                type="text"
                                fullWidth
                                variant="outlined"
                            // onChange={(event: React.ChangeEvent<HTMLInputElement> ) => onCreateFormChange(event, "teamName")}
                            />
                            <TextField
                                autoComplete='off'
                                name="teamPassword"
                                margin="normal"
                                id="teamName"
                                label="Введите пароль"
                                type="password"
                                fullWidth
                                variant="outlined"
                            // onChange={(event: React.ChangeEvent<HTMLInputElement> ) => onCreateFormChange(event, "password")}
                            />
                            {showErrorMessage ? <Typography sx={{ color: 'red', mt: 1, display: 'block' }} >Нужно написать функцию на отображение ошибок</Typography> : null}
                        </DialogContent>
                        <DialogActions>
                            <Button onClick={handleClickCloseCreateTeamForm}>Отмена</Button>
                            <Button type="submit" disabled={disableConfirmButton}>Подтвердить</Button>
                        </DialogActions>
                    </Box>
                </Dialog>
            </Box>
        </>
    )
}

export default TeamsInfoWhenUserNotInTeam
