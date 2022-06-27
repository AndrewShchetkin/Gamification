import React, { useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';

function QuizUploadForm({addQuiz}:any) {

    const [quiz, setQuiz] = useState<IQuiz>({name:'', 
        dateBegin:'', dateEnd:''})
    
    const submitAdd = (e:any) => {
        e.preventDefault();
        addQuiz(quiz);

        setQuiz({name:'', dateBegin:'', dateEnd:''});
    }

    return (
        <form encType="multipart/form-data" action="" style={{display:'flex', 
            flexDirection:'column', alignItems:'center'}}>
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

            <CustomButton onClick={submitAdd}>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;