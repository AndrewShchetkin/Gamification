import axios from 'axios';
import React from 'react'
import { ActionType } from '../../../@types/ReduxTypes/ActionTypes';
import { useAppDispatch } from '../../../store/hooks';
import { setTeamId } from '../../../store/reducers/auth/authSlice';
import { CustomButton } from '../../shared/components/UI/CustomButton/CustomButton';
import { CustomInput } from '../../shared/components/UI/CustomInput/CustomInput';
import classes from "./CreateTeamForm.module.scss"

interface Props {
    closeForm: () => void;
}

function CreateTeamForm(props: Props) {
    const { closeForm } = props
    const dispatch = useAppDispatch();

    // Обработчик создания команды
    const handleCreateFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const body = {
            TeamName: data.get('teamName'),
            Password: data.get('teamPassword')
        };
        await axios.post<string>('api/team/teamregister', body)
            .then(function (response) {
                dispatch(setTeamId(response.data));
                dispatch({ type: ActionType.UpdateTeams, payload: response.data });
                closeForm();
            })
            .catch(function (error) {
                //const errors: IError[] = error.response.data.errors;
                //setErrors(errors); 
            });
    }

    return (
        <div className={classes.wrapper}>
            <div
                className={classes.closeForm}
                onClick={closeForm}>
            </div>
            <label className={classes.header}>Заполните поля для создания команды</label>
            <form className={classes.formWrapper} onSubmit={handleCreateFormSubmit}>
                <div className={classes.formContent}>
                    <div className={classes.textBlock}>
                        <div>Наименование команды</div>
                        <div style={{ marginTop: '25px' }}>Пароль</div>
                    </div>
                    <div className={classes.inputBlock}>
                        <CustomInput
                            style={{ height: '35px' }}
                            name="teamName"
                        />
                        <CustomInput
                            style={{ marginTop: '15px', height: '35px' }}
                            type="password"
                            name="teamPassword"
                        />
                    </div>
                </div>
                <button className={classes.confirmBtn} type="submit"/>
            </form>
        </div>
    )
}

export default CreateTeamForm
