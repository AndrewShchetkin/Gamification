import React, { useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import QuizTable from './QuizTable';
import QuizUploadForm from './QuizUploadForm';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import axios, { AxiosError } from 'axios';
import classes from './styles/Body.module.css';

function AdminPanelBody() {

    const [modal, setModal] = useState<boolean>(false);
    const [quizList, setQuizList] = useState<IQuiz[]>([])

    const getAllQuizzes = async () => {
        try {
            const response = await axios.get<IQuiz[]>('api/quiz');
            await setQuizList(response.data);
        }
        catch(error) {
            alert((error as AxiosError).message);
        }
    }

    const addQuiz = async () => {
        await getAllQuizzes();
        setModal(false);
    }

    const deleteQuiz = async () => {
        await getAllQuizzes();
    }

    useEffect(() => {
        console.log('quizAdded');
        getAllQuizzes();

    }, [])

    return (
        <div className={classes.content}>
            <CustomButton style={{alignSelf:'flex-end'}} 
                onClick={() => setModal(true)}>
                Загрузить викторину
            </CustomButton>

            <CustomModal modal={modal} setModal={setModal}>
                <QuizUploadForm addQuiz={addQuiz} modal={modal}/>
            </CustomModal>

            {quizList.length ? <QuizTable quizList={quizList} deleteQuiz={deleteQuiz} />
                : <div>Викторины не найдены!</div>}
            


        </div>
    );
}

export default AdminPanelBody;