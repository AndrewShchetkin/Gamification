import React, { ChangeEvent, FC, FormEvent, MutableRefObject, useEffect, useRef, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';
import {Validator} from '../../services/ValidationService';
import IQuizErrorMsg from '../../@types/AdminPanel/IQuizErrorMsg';
import axios, { AxiosError } from 'axios';
import classes from './styles/UploadForm.module.css';

export interface IUploadForm {
    addQuiz:() => void,
    modal: boolean
}

const INITIAL_QUIZ ={name:'', dateBegin:'', dateEnd:''};
const INITIAL_QUIZ_ERROR_MSG ={name:'', dateBegin:'', dateEnd:'', filePath:'', serverError:''};

const QuizUploadForm:FC<IUploadForm> = ({addQuiz, modal}) => {

    const [quiz, setQuiz] = useState<IQuiz>(INITIAL_QUIZ);
    const [quizErrorMsgs, setQuizErrorMsgs] = useState<IQuizErrorMsg>(INITIAL_QUIZ_ERROR_MSG);
    const [filePath, setFilePath] = useState<string>('');

    useEffect(() => { // при скрытии мод. окна поля стираются
        if (!modal) {
            setQuiz(INITIAL_QUIZ);
            setFilePath('');
            setQuizErrorMsgs(INITIAL_QUIZ_ERROR_MSG);
        }
    }, [modal])

    const submitAdd = async (e:FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        
        if (!Validator.uploadFormValidate(quiz,filePath,setQuizErrorMsgs, quizErrorMsgs))
            return;

        try {
            const response = await axios.post('api/quiz', new FormData(e.currentTarget));

            console.log('Успех:', response.data);
            await addQuiz();
            setQuiz(INITIAL_QUIZ);
            setFilePath('');
            setQuizErrorMsgs(INITIAL_QUIZ_ERROR_MSG);
        }
        
        catch(err) {

            const error: AxiosError = (err as AxiosError);
            if (error.message === 'Network Error') // изменили файл
            {
                setFilePath('');
                setQuizErrorMsgs({...INITIAL_QUIZ_ERROR_MSG, filePath:'Файл был изменен. Загрузите файл еще раз'});
            }
            else if (error.response?.status === 400)
                setQuizErrorMsgs({...INITIAL_QUIZ_ERROR_MSG, serverError: error.response?.data.error})
            else 
                setQuizErrorMsgs({...INITIAL_QUIZ_ERROR_MSG, serverError: 'Непредвиденная ошибка на сервере'})
        }
        
       
        

       
    }

    return (
        <form encType="multipart/form-data" action="" className={classes.uploadForm} onSubmit={submitAdd} >

            <CustomTextInput
            name='name'
            type="text"
            placeholder='Название'
            value={quiz.name}
            onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz, name: e.currentTarget.value})}
            /> 
            <div className={classes.errorInput}>{quizErrorMsgs.name}</div>

            <div className={classes.dateContainer}>
                <div className={classes.dateText}>Дата начала: </div>
                <input
                name='db'
                type="datetime-local"
                value={quiz.dateBegin}
                onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz,
                    dateBegin: e.currentTarget.value})}
                />
            </div>
            <div className={classes.errorInput}>{quizErrorMsgs.dateBegin}</div>

            <div className={classes.dateContainer}>
                <div className={classes.dateText}>Дата окончания: </div>
                <input
                name='de'
                type="datetime-local"
                value={quiz.dateEnd}
                onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz,
                    dateEnd: e.currentTarget.value})}
                />
            </div>
            <div className={classes.errorInput}>{quizErrorMsgs.dateEnd}</div>
            
            <input
            value={filePath}
            onChange={(e:ChangeEvent<HTMLInputElement>) => setFilePath(e.currentTarget.value)}
            type='file'
            name='file'
            style={{marginTop:'15px'}}
            />
            <div className={classes.errorInput}>{quizErrorMsgs.filePath}</div>
                
            <CustomButton type='submit' style={{marginTop:'15px'}}>Добавить</CustomButton>
            <div className={classes.errorServer}>{quizErrorMsgs.serverError}</div>
         </form>
    );
}

export default QuizUploadForm;