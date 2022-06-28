import React, { useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import CustomTextInput from '../shared/components/UI/CustomTextInput/CustomTextInput';

function QuizUploadForm({addQuiz, modal}:{addQuiz:any, modal:boolean}) {

    const [quiz, setQuiz] = useState<IQuiz>({name:'', 
        dateBegin:'', dateEnd:'', xlsxPath:''})
    
    useEffect(() => { // при скрытии мод. окна поля стираются
        if (!modal)
            setQuiz({name:'', dateBegin:'', dateEnd:'', xlsxPath:''});
    }, [modal])

    const submitAdd = async (e:any) => {
        e.preventDefault();
        
        // file work

        const response = await fetch('api/quiz', { // -> axios
            method: 'POST',
            body: new FormData(e.target)
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
            onChange={(e:any) => setQuiz({...quiz, name: e.target.value})}
            /> 

            <input
            name='db'
            type="datetime-local"
            value={quiz.dateBegin}
            onChange={(e:any) => setQuiz({...quiz,
                dateBegin: e.target.value})}
            />

            <input
            name='de'
            type="datetime-local"
            value={quiz.dateEnd}
            onChange={(e:any) => setQuiz({...quiz,
                 dateEnd: e.target.value})}
              />

            <input type='file' value={quiz.xlsxPath} 
               onChange={(e:any) => setQuiz({...quiz,
                xlsxPath: e.target.value})} name='file' />
                
            <CustomButton type='submit'>Добавить</CustomButton>
         </form>
    );
}

export default QuizUploadForm;