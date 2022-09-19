import React, { useMemo } from 'react';
import IQuiz from '../../../../../@types/AdminPanel/IQuiz';
import classes from './Pagination.module.css';

export interface IPagination {
    list:IQuiz[],
    limit:number,
    currPage:number,
    setCurrPage:React.Dispatch<React.SetStateAction<number>>
}

function Pagination({list, limit, currPage, setCurrPage}:IPagination) {

    const pageNumbers = useMemo(() => {
        
        const arr = [];
        for (let i = 1; i <= Math.ceil(list.length / limit); ++i)
            arr.push(i);
        return arr;
    }, [list]);

    return (
        <ul className={classes.paginationList}>
            {pageNumbers.map(number => 
                <li
                className= {currPage === number ?
                        `${classes.paginationListItem} ${classes.extend}`
                    : classes.paginationListItem}
                key={number}
                id={`${number}`}
                onClick={() => setCurrPage(number)}
                >
                {number}
                </li>
            )}
        </ul>
    );
}

export default Pagination;