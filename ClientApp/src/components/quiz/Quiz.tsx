import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import { getQuestions, nextQuestion } from '../../store/reducers/quize/quizeSlice';
import QuestionElement from './question';

export default function Quiz() {
    const userName = useAppSelector(s => s.auth.name);
    const questionsIsLoaded = useAppSelector(q => q.quize.questionsIsLoaded);
    
    const questions = useAppSelector(q => q.quize.questions);
    const currentQuestionIndex = useAppSelector(q => q.quize.currentQuestionIndex);
    const currentQuestion = questions[currentQuestionIndex];
    const dispatch = useAppDispatch();
    if(!questionsIsLoaded)
        dispatch(getQuestions())
    
    return (
        <Box
            sx={{
                width: '100%',
                height: '100%',
                color: '#fff',
                '& > .MuiBox-root > .MuiBox-root': {
                    p: 1,
                    borderRadius: 2,
                },
                alignItems: 'center'
            }}
        >
            <Box
                sx={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(4, 1fr)',
                    gridAutoRows: '40px',
                    gap: 5,
                    gridTemplateRows: 'auto',
                    gridTemplateAreas: `
                    "header header header header"
                    "sidebar main main main"
                    "footer footer footer footer"`,

                }}
            >
                <Box sx={{ gridArea: 'header', bgcolor: 'primary.main', gridRow: 'span 2' }}>
                    Header + инфа с таймерами
                    <Link to='/game'>game</Link>
                    <Link to='/game2'>game2</Link>
                </Box>
                <Box sx={{ gridArea: 'main', bgcolor: 'secondary.main', textAlign: 'center', gridRow: '3/12' }}>
                    {/* TODO: if questions are not loaded show loader */}
                    {questionsIsLoaded ? <div>
                        {currentQuestionIndex}
                        <QuestionElement text={currentQuestion.text} />
                        
                        <Button variant="contained" onClick={()=>dispatch(nextQuestion())}>Next question</Button>
                    </div>
                    : <CircularProgress color="inherit" />}
                </Box>
                <Box sx={{ gridArea: 'sidebar', bgcolor: 'info.main', textAlign: 'center', gridRow: '3/12' }}>
                    <ul>
                        <li>тут разметить пользователя 😎</li>
                        <li>тут разметить пользователя 😴</li>
                        <li>тут разметить пользователя 🐵</li>
                        <li>тут разметить пользователя 💩 {userName}</li>
                        <li>тут разметить пользователя 🐱‍👤</li>
                    </ul>
                </Box>
                <Box sx={{
                    gridArea: 'footer',
                    bgcolor: 'warning.main',
                    bottom: 0,
                    gridRow: '12/14'
                }}>Тут ссылка на пожертвования 🤑</Box>
            </Box>
        </Box>
    );
}
