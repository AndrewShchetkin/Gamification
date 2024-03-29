import React, { useEffect, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import QuizTable from './QuizTable';
import QuizUploadForm from './QuizUploadForm';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import axios, { AxiosError } from 'axios';
import classes from './styles/Body.module.css';
import { DatesService } from '../../services/DatesService';



function AdminPanelBody() {

    const [visibleQuizUploadFromModal, setVisibleQuizUploadFromModal] = useState<boolean>(false);
    const [quizList, setQuizList] = useState<IQuiz[]>([])

    const getAllQuizzes = async () => {
        try {
            const response = await axios.get<IQuiz[]>('api/quiz');
            await setQuizList(response.data.map((quiz) => {
                const newDateBegin = DatesService.convertUTCDateToLocalDate(new Date(quiz.dateBegin)).toLocaleString();
                const newDateEnd = DatesService.convertUTCDateToLocalDate(new Date(quiz.dateEnd)).toLocaleString();
                return {
                    ...quiz,
                    dateBegin: newDateBegin,
                    dateEnd: newDateEnd
                }
            }));
        }
        catch (error) {
            alert((error as AxiosError).message);
        }
    }

    const addQuiz = async () => {
        await getAllQuizzes();
        setVisibleQuizUploadFromModal(false);
    }

    const deleteQuiz = async () => {
        await getAllQuizzes();
    }

    useEffect(() => {
        console.log('quizAdded');
        getAllQuizzes();
    }, [])

    const toggleVisibleQuizUploadFromModal = (flag: boolean) => {
        setVisibleQuizUploadFromModal(flag);
    }

    return (
        <div className={classes.content}>
            <CustomButton
                style={{ alignSelf: 'flex-end' }}
                onClick={() => setVisibleQuizUploadFromModal(true)}> Загрузить викторину
            </CustomButton>
            <CustomModal visible={visibleQuizUploadFromModal} setVisible={toggleVisibleQuizUploadFromModal}>
                <QuizUploadForm addQuiz={addQuiz} modal={visibleQuizUploadFromModal} />
            </CustomModal>

            {quizList.length 
            ? <QuizTable quizList={quizList} deleteQuiz={deleteQuiz} />
            : <div>Викторины не найдены!</div>}
        </div>
    );
}

export default AdminPanelBody;