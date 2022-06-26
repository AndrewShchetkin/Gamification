import { Box, Button, DialogActions, DialogContent, DialogTitle, TextField } from '@mui/material'
import axios from 'axios'
import React from 'react'
import { ITeam } from '../../@types/ITeam'
import { useAppDispatch } from '../../store/hooks'
import { setTeamId } from '../../store/reducers/auth/authSlice'

interface Props {
    team: ITeam,
    closeForm: () => void;
}

function JoinToTeamForm(props: Props) {
    const { team, closeForm } = props;
    const dispatch = useAppDispatch();

    // Обработчик присоединения к команде команды
    const handleJoinFormSubmit = async (event: React.FormEvent<HTMLFormElement>, teamId: number) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const body = {
            teamId: teamId,
            password: data.get('teamPassword')
        };
        await axios.post<string>('api/team/jointheteam', body)
            .then(function (response) {
                dispatch(setTeamId(response.data));
                closeForm();
            })
            .catch(function (error) {
                console.log(error);
            });

        }
        return (
            <Box component="form" onSubmit={(event: React.FormEvent<HTMLFormElement>) => handleJoinFormSubmit(event, team.id)}>
                <DialogTitle>Присоедениться к команде {team.teamName} </DialogTitle>
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
                    <Button onClick={closeForm}>Отмена</Button>
                    <Button type="submit">Подтвердить</Button>
                </DialogActions>
            </Box>
        )
}

export default JoinToTeamForm
