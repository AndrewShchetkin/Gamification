import axios from 'axios';
import React from 'react'
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
                closeForm();
            })
            .catch(function (error) {
                //const errors: IError[] = error.response.data.errors;
                //setErrors(errors); 
            });
    }

    return (
        <form
            onSubmit={handleCreateFormSubmit}>
            <div className={classes.formContent}>
                <p>Создать команду</p>
                <CustomInput 
                    name="teamName"
                />
                <CustomInput
                    type="password"
                    name="teamPassword"
                />
            </div>
            <div className={classes.formBtnGroup}>
                <CustomButton type="submit">Подтвердить</CustomButton>
                <CustomButton onClick={closeForm} >Отмена</CustomButton>
            </div>
        </form>
        // <Box component="form" onSubmit={handleCreateFormSubmit}>
        //     <DialogTitle>Создать команду</DialogTitle>
        //     <DialogContent>
        //         <TextField
        //             autoFocus
        //             autoComplete='off'
        //             name="teamName"
        //             margin="normal"
        //             id="teamName"
        //             label="Введите название команды"
        //             type="text"
        //             fullWidth
        //             variant="outlined"
        //         />
        //         <TextField
        //             autoComplete='off'
        //             name="teamPassword"
        //             margin="normal"
        //             id="teamName"
        //             label="Введите пароль"
        //             type="password"
        //             fullWidth
        //             variant="outlined"
        //         />
        //     </DialogContent>
        //     <DialogActions>
        //         <Button onClick={closeForm}>Отмена</Button>
        //         <Button type="submit">Подтвердить</Button>
        //     </DialogActions>
        // </Box>
    )
}

export default CreateTeamForm
