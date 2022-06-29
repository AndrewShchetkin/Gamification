import React, { ChangeEvent, FC, FormEvent, useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';

export interface IUploadForm {
    addQuiz: (quiz:IQuiz) => void,
    modal: boolean
}

const QuizUploadForm:FC<IUploadForm> = ({addQuiz, modal}) => {

    const [quiz, setQuiz] = useState<IQuiz>({name:'', 
        dateBegin:'', dateEnd:'', xlsxPath:''})

    console.log(quiz);

    useEffect(() => { // при скрытии мод. окна поля стираются
        if (!modal)
            setQuiz({name:'', dateBegin:'', dateEnd:'', xlsxPath:''});
    }, [modal])

    const submitAdd = async (e:FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        
        // file work
        const response = await fetch('api/quiz', { // -> axios
            method: 'POST',
            body: new FormData(e.currentTarget)
        });
        const result = await response.json();
        
        if (response.ok) {

            console.log('Успех:', result);
            addQuiz(quiz);
            setQuiz({name:'', dateBegin:'', dateEnd:'', xlsxPath:''});
        }
        else {
            console.log('Error', result);
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
            value={quiz.xlsxPath} 
            onChange={(e:ChangeEvent<HTMLInputElement>) => setQuiz({...quiz,
                xlsxPath: e.currentTarget.value})}
            name='file'
            style={{marginTop:'15px'}}
            />
                
            <CustomButton type='submit' style={{marginTop:'15px'}}>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;