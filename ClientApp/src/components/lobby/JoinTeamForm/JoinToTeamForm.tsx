import axios from 'axios'
import React from 'react'
import { ITeam } from '../../../@types/ITeam'
import { ActionType } from '../../../@types/ReduxTypes/ActionTypes'
import { useAppDispatch } from '../../../store/hooks'
import { setTeamId } from '../../../store/reducers/auth/authSlice'
import { CustomInput } from '../../shared/components/UI/CustomInput/CustomInput'
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
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const body = {
            teamId: teamId,
            password: data.get('teamPassword')
        };
        await axios.post<string>('api/team/jointheteam', body)
            .then(function (response) {
                dispatch(setTeamId(response.data));
                dispatch({ type: ActionType.UpdateTeams, payload: response.data });
                closeForm();
            })
            .catch(function (error) {
                console.log(error);
            });
    }
    return (
        <div className={classes.wrapper}>
            <div
                className={classes.closeForm}
                onClick={closeForm}>
            </div>
            <div className={classes.teamInfo}>
                <p style={{ margin: "0px" }}>{team.teamName}</p>
                <ol>
                    {team.users.map(user =>
                        <li className={classes.userItem}>{user.userName}</li>)}
                </ol>
            </div>
            <form className={classes.formContent}
                onSubmit={(event: React.FormEvent<HTMLFormElement>) => handleJoinFormSubmit(event, team.id)}>
                <p>Введите пароль для присоединения к команде</p>
                <CustomInput
                    style={{ width: '80%', height: '35px' }}
                    type="password"
                    name="teamPassword" />
                <button className={classes.confirmBtn} type="submit" />
            </form>
        </div>
    )
}

export default JoinToTeamForm
