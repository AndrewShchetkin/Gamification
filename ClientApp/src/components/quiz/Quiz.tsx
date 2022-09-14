import axios from 'axios';
import { useEffect, useState } from 'react';
import { ITeam } from '../../@types/ITeam';
import QuestionBox from './QuestionsBox'
import { useAppSelector } from '../../store/hooks';
import classes from './Quiz.module.scss'

export default function Quiz() {

    const userName = useAppSelector(state => state.authReduser.userName)
    const teamId = useAppSelector(state => state.authReduser.teamId);

    const [usersTeam, setUsersTeam] = useState<ITeam>();
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() => {
        if (teamId) {
            fetchUserTeam();
        }
    }, [teamId])

    const fetchUserTeam = async () => {
        try {
            const response = await axios.get('api/team/getTeamByID', { params: { teamID: teamId } });
            setUsersTeam(response.data);
            setIsLoading(false);
        }
        catch (e) {
            console.log(e);
        }
    }

    return (
        <div>
            <div className={classes.header}>
                <div className={classes.logo}></div>
                <div className={classes.gameName}></div>
            </div>

            <div className={classes.wrapper}>
                <div className={classes.quizName}>Название викторины</div>
                <div className={classes.container}>
                    <div className={classes.teamInfo}>
                        <div className={classes.teamName}>
                            {usersTeam?.teamName}
                        </div>
                        <div className={classes.usersList}>
                            {usersTeam?.users.map(user =>
                                <div className={classes.userItem}>
                    <QuestionBox/>
                                    {user.userName}
                                </div>)}
                        </div>
                        <div className={classes.timer}></div>
                    </div>
                    <div className={classes.quizContent}>
                        <div className={classes.questionsNumbers}></div>
                        <div className={classes.currentQuestion}>
                            <div className={classes.questionWording}></div>
                            <div className={classes.answersOptions}>
                                <div className={classes.firstAnswersPair}>
                                    <div className={classes.answer}></div>
                                    <div className={classes.answer}></div>
                                </div>
                                <div className={classes.secondAnswersPair}>
                                    <div className={classes.answer}></div>
                                    <div className={classes.answer}></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
