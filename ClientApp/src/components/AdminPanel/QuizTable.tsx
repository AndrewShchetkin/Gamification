import React from 'react';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import classes from '../../styles/AdminPanel/QuizTable/QuizTable.module.css';

function QuizTable() {
    return (
        
        <table className={classes.customTable}>
            <tr>
                <th className={classes.column}>Название викторины</th>
                <th className={classes.column}>Дата начала</th>
                <th style={{border:'1px solid'}} className={classes.column}>Дата окончания</th>
            </tr>
            <tr>
                <td className={classes.column}>Столбец 1</td>
                <td className={classes.column}>Столбец 2</td>
                <td className={classes.column}>Столбец 3</td>
                <td className={classes.column}>
                    <CustomButton>Delete</CustomButton>
                </td>
            </tr>
            <tr>
                <td className={classes.column}>Столбец 1</td>
                <td className={classes.column}>Столбец 2</td>
                <td className={classes.column}>Столбец 3</td>
                <td className={classes.column}>
                    <CustomButton>Delete</CustomButton>
                </td>
            </tr>
        </table>
    );
}

export default QuizTable;