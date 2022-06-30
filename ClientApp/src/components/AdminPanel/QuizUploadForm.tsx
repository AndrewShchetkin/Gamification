import React, { ChangeEvent, FC, FormEvent, MutableRefObject, useEffect, useRef, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';
import {Validator} from '../../services/ValidationService';
import IQuizErrorMsg from '../../@types/AdminPanel/IQuizErrorMsg';
import axios from 'axios';

export interface IUploadForm {
    addQuiz:() => void,
    modal: boolean
}

const initialQuiz ={name:'', dateBegin:'', dateEnd:''};
const initialQuizErrorMsg ={name:'', dateBegin:'', dateEnd:'', filePath:''};

const QuizUploadForm:FC<IUploadForm> = ({addQuiz, modal}) => {

    const [quiz, setQuiz] = useState<IQuiz>(initialQuiz);
    const [quizErrorMsgs, setQuizErrorMsgs] = useState<IQuizErrorMsg>(initialQuizErrorMsg);
    const [filePath, setFilePath] = useState<string>('');

    useEffect(() => { // при скрытии мод. окна поля стираются
        if (!modal) {
            setQuiz(initialQuiz);
            setFilePath('');
            setQuizErrorMsgs(initialQuizErrorMsg);
        }
    }, [modal])

    const submitAdd = async (e:FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        
        const validator = new Validator();
        if (!validator.uploadFormValidate(quiz,filePath,setQuizErrorMsgs))
            return;

        // file work

        const response = await axios.post('api/quiz', new FormData(e.currentTarget));

        if (response.status === 200) {

            console.log('Успех:', response.data);
            await addQuiz();
            setQuiz(initialQuiz);
            setFilePath('');
            setQuizErrorMsgs(initialQuizErrorMsg);
        }
        else {
            console.log('Error', response.data);
        }

       
    }

    return (
        <form encType="multipart/form-data" action="" style={{display:'flex', 
            flexDirection:'column', alignItems:'center'}} onSubmit={submitAdd} >

            <CustomTextInput
            name='name'
            type="text"
            placeholder='Название'
            value={quiz.name}
            onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz, name: e.currentTarget.value})}
            /> 
            <div style={{marginTop:'5px', color:'red'}}>{quizErrorMsgs.name}</div>

            <div style={{display:'flex', justifyContent:'center',alignItems:'center', marginTop:'15px'}}>
                <div style={{width:'100px' }}>Дата начала: </div>
                <input
                name='db'
                type="datetime-local"
                value={quiz.dateBegin}
                onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz,
                    dateBegin: e.currentTarget.value})}
                />
            </div>
            <div style={{marginTop:'5px', color:'red'}}>{quizErrorMsgs.dateBegin}</div>

            <div style={{display:'flex', justifyContent:'center', alignItems:'center', marginTop:'15px'}}>
                <div style={{width:'100px'}}>Дата окончания: </div>
                <input
                name='de'
                type="datetime-local"
                value={quiz.dateEnd}
                onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz,
                    dateEnd: e.currentTarget.value})}
                style={{height:'fit-content'}}
                />
            </div>
            <div style={{marginTop:'5px', color:'red'}}>{quizErrorMsgs.dateEnd}</div>
            
            <input
            value={filePath}
            onChange={(e:ChangeEvent<HTMLInputElement>) => setFilePath(e.currentTarget.value)}
            type='file'
            name='file'
            style={{marginTop:'15px'}}
            />
            <div style={{marginTop:'5px', color:'red'}}>{quizErrorMsgs.filePath}</div>
                
            <CustomButton type='submit' style={{marginTop:'15px'}}>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;