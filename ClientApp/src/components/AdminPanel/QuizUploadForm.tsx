import React, { ChangeEvent, FC, FormEvent, useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';
import axios from 'axios';

export interface IUploadForm {
    setQuizAdded:React.Dispatch<React.SetStateAction<boolean>>,
    modal: boolean
}

const initialQuiz ={name:'', dateBegin:'', dateEnd:''};

const QuizUploadForm:FC<IUploadForm> = ({setQuizAdded, modal}) => {

    const [quiz, setQuiz] = useState<IQuiz>(initialQuiz)

    useEffect(() => { // при скрытии мод. окна поля стираются
        if (!modal)
            setQuiz(initialQuiz);
    }, [modal])

    const submitAdd = async (e:FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        
        // file work
        const response = await axios.post('api/quiz', new FormData(e.currentTarget));

        if (response.status === 200) {

            console.log('Успех:', response.data);
            setQuizAdded(true);
            setQuiz(initialQuiz);
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

            <input
            type='file'
            name='file'
            style={{marginTop:'15px'}}
            />
                
            <CustomButton type='submit' style={{marginTop:'15px'}}>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;