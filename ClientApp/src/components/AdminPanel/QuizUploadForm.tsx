import React, { useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';

function QuizUploadForm({addQuiz}:any) {

    const [quiz, setQuiz] = useState<IQuiz>({name:'', 
        dateBegin:'', dateEnd:''})

    const submitAdd = (e:any) => {
        e.preventDefault();
        addQuiz(quiz);

        /* TODO: setQuiz({name:'', 
        dateBegin:'', dateEnd:'', file:''}) */
    }

    return (
        <form action="" style={{display:'flex', 
            flexDirection:'column'}}>
            <input type="text" placeholder='Название'/> {/* TODO: custom text input
            + custom file input + fill quiz */}
            <input type="text" placeholder='Дата начала'/>
            <input type="text" placeholder='Дата окончания'/>
            <CustomButton onClick={submitAdd}>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;