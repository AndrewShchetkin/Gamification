import axios from 'axios'
import React from 'react'
import { ITeam } from '../../@types/ITeam'
import { useAppDispatch } from '../../store/hooks'
import { setTeamId } from '../../store/reducers/auth/authSlice'
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton'
import { CustomInput } from '../shared/components/UI/CustomInput/CustomInput'
import classes from "./JoinTeamForm.module.scss"

interface Props {
    team: ITeam,
    closeForm: () => void;
}

function JoinToTeamForm(props: Props) {
    const { team, closeForm } = props;
    const dispatch = useAppDispatch();

    // Обработчик присоединения к команде команды
    const handleJoinFormSubmit = async (event: React.FormEvent<HTMLFormElement>, teamId: number) => {
        debugger;
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
            <form
                onSubmit={(event: React.FormEvent<HTMLFormElement>) => handleJoinFormSubmit(event, team.id)}>
                <div className={classes.formContent}>
                    <p>Присоедениться к команде</p>
                    <CustomInput
                    type="password"
                    name="teamPassword"/>
                </div>
                <div className={classes.formBtnGroup}>
                    <CustomButton type="submit">Подтвердить</CustomButton>
                    <CustomButton onClick={closeForm} >Отмена</CustomButton>
                </div>
            </form>
        )
}

export default JoinToTeamForm
