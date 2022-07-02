import React from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import classes from '../../styles/AdminPanel/QuizTable/QuizTable.module.css';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import axios from 'axios';

function QuizTable({quizList, deleteQuiz}:{quizList:IQuiz[], deleteQuiz: () => void}) {

    const onDelete = async (quiz: IQuiz) => {
        const response = await axios.delete(`api/quiz?quizName=${quiz.name}`)
        
        if (response.status === 200) {
            deleteQuiz()
        }

        console.log(response.data);
    }

    return (
        
        <table className={classes.customTable}>
            <thead>
                <tr>
                    <th className={classes.column}>Название викторины</th>
                    <th className={classes.column}>Дата начала</th>
                    <th style={{border:'1px solid'}} className={classes.column}>Дата окончания</th>
                </tr>
            </thead>
            {quizList.length ?
            <tbody>
                {quizList.map((quiz) => 
                    <tr key={quiz.name}>
                        <td className={classes.column}> 
                            {quiz.name}
                        </td>
                        <td className={classes.column}> 
                            {quiz.dateBegin.split('T').join(' ')}
                        </td>
                        <td className={classes.column}> 
                            {quiz.dateEnd.split('T').join(' ')}
                        </td>
                        <td className={classes.column}>
                            <CustomButton onClick={() => onDelete(quiz)}>Удалить</CustomButton>
                        </td>
                    </tr>
                )}
                   
            </tbody> :
            <tbody>
                <tr>
                    <td colSpan={3} style={{textAlign:'center',
                        fontSize:'20px'}}>Ожидаю викторину...</td>
                </tr>
            </tbody>
                 }
            
            
        </table>
    );
}

export default QuizTable;