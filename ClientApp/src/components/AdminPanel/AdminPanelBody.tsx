import React, { useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import QuizTable from './QuizTable';
import QuizUploadForm from './QuizUploadForm';
import IQuiz from '../../@types/AdminPanel/IQuiz';

function AdminPanelBody() {

    const [modal, setModal] = useState(false);
    const [quizList, setQuizList] = useState<IQuiz[]>([])

    console.log(quizList)
    const addQuiz = (quiz:IQuiz) => {
        console.log(quiz);
        setQuizList([...quizList, quiz]);

        setModal(false);
    }

    return (
        <div style={{display:'flex', flexDirection:'column',
            alignItems:'center'}}>
            <CustomButton style={{alignSelf:'flex-end'}} 
                onClick={() => setModal(true)}>
                Загрузить викторину
            </CustomButton>

            <CustomModal modal={modal} setModal={setModal}>
                <QuizUploadForm addQuiz={addQuiz} modal={modal}/>
            </CustomModal>

            <QuizTable quizList={quizList}/>


        </div>
    );
}

export default AdminPanelBody;