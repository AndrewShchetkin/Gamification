import React, { useEffect, useMemo, useState } from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import classes from './styles/QuizTable.module.css';
import IQuiz from '../../@types/AdminPanel/IQuiz';
import axios, { AxiosError } from 'axios';

const QUIZZES_PER_PAGE = 3;

function QuizTable({quizList, deleteQuiz}:{quizList:IQuiz[], deleteQuiz: () => void}) {

    const [currPage, setCurrPage] = useState<number>(1);
    
    const pageNumbers = useMemo(() => {
        
        const arr = [];
        for (let i = 1; i <= Math.ceil(quizList.length / QUIZZES_PER_PAGE); ++i) {
            arr.push(i);
        }
        return arr;
    }, [quizList]);

    const nextPageIndex = currPage * QUIZZES_PER_PAGE;
    const prevPageIndex = nextPageIndex - QUIZZES_PER_PAGE;

    const pageQuizzes = useMemo(() => {
        return quizList.slice(prevPageIndex, nextPageIndex);
    }, [currPage, quizList]);


    useEffect(() => { // если удаляем последний элемент на странице - переходим на пред. страницу
        if (!pageQuizzes.length)
            setCurrPage(currPage-1);
    }, [pageQuizzes])

    const switchPage = (e: any) => {
        setCurrPage(e.target.id);
    }

    const onDelete = async (quiz: IQuiz) => {

        if (!confirm("Вы точно хотите удалить?"))
            return;
        
        try {
            await axios.delete(`api/quiz?quizName=${quiz.name}`);
            await deleteQuiz();

            
        }
        catch(err) {
            const error: AxiosError = err as AxiosError;
            if (error.response?.status === 400)
                alert((err as AxiosError).response?.data);
            else
                alert(error.message);
        }

    

    }

    return (
        
        <div>
            <ul className={classes.paginationList}>
                {pageNumbers.map(number => {
                    return (
                    <li
                    className= {classes.paginationListItem}
                    key={number}
                    id={`${number}`}
                    onClick={switchPage}
                    >
                    {number}
                    </li>
                )})}
            </ul>

            <table className={classes.customTable}>
                <thead>
                    <tr>
                        <th className={classes.column}>Название викторины</th>
                        <th className={classes.column}>Дата начала</th>
                        <th style={{border:'1px solid'}} className={classes.column}>Дата окончания</th>
                    </tr>
                </thead>
                <tbody>
                    {pageQuizzes.map((quiz) => 
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
                </tbody>
            
            
            </table>

        </div>

        
    );
}

export default QuizTable;