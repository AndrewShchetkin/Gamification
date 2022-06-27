import React, { useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';

function QuizUploadForm({addQuiz}:any) {

    const [quiz, setQuiz] = useState<IQuiz>({name:'', 
        dateBegin:'', dateEnd:'', xlsxPath:''})
    

    const submitAdd = async (e:any) => {
        e.preventDefault();
        
        // file work
        const response = await fetch('api/quiz', {
            method: 'POST',
            body: new FormData(e.target)
        });
        const result = await response.json();
        // если ответ - ок, то добавляем, иначе - нет
        console.log('Успех:', JSON.stringify(result));
        //

        addQuiz(quiz);

        setQuiz({name:'', dateBegin:'', dateEnd:'', xlsxPath:''});
    }

    return (
        <form encType="multipart/form-data" action="" style={{display:'flex', 
            flexDirection:'column', alignItems:'center'}} onSubmit={submitAdd} >
            <CustomTextInput
            type="text"
            placeholder='Название'
            value={quiz.name}
            onChange={(e:any) => setQuiz({...quiz, name: e.target.value})}
            /> {/* TODO: custom text input
            + custom file input + fill quiz */}
            <CustomTextInput
            type="text"
            placeholder='Дата начала'
            value={quiz.dateBegin}
            onChange={(e:any) => setQuiz({...quiz,
                dateBegin: e.target.value})}
            />
            <CustomTextInput
            type="text"
            placeholder='Дата окончания'
            value={quiz.dateEnd}
            onChange={(e:any) => setQuiz({...quiz,
                 dateEnd: e.target.value})}
              />
            <input id='file' type="file" value={quiz.xlsxPath} 
               onChange={(e:any) => setQuiz({...quiz,
                xlsxPath: e.target.value})} />
            <CustomButton type='submit'>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;