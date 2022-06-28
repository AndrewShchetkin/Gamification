import React from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import classes from '../../styles/AdminPanel/QuizTable/QuizTable.module.css';
import IQuiz from '../../@types/AdminPanel/IQuiz';

function QuizTable({quizList}:{quizList:IQuiz[]}) {

    

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
                            <CustomButton>Удалить</CustomButton>
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