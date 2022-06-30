import React, { useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import QuizTable from './QuizTable';
import QuizUploadForm from './QuizUploadForm';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import axios from 'axios';

function AdminPanelBody() {

    const [modal, setModal] = useState<boolean>(false);
    const [quizList, setQuizList] = useState<IQuiz[]>([])

    const getAllQuizzes = async () => {
        const response = await axios.get<IQuiz[]>('api/quiz');
        if (response.status === 200) 
            setQuizList(response.data);
        else
            console.log('error', response.statusText);
    }

    const addQuiz = async () => {
        getAllQuizzes();
        setModal(false);
    }

    const deleteQuiz = async () => {
        getAllQuizzes();
    }

    useEffect(() => {
        console.log('quizAdded');
        getAllQuizzes();

    }, [])

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

            <QuizTable quizList={quizList} deleteQuiz={deleteQuiz}/>


        </div>
    );
}

export default AdminPanelBody;