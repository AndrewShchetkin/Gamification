import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { IError } from '../../@types/IError';
import { ITeam } from '../../@types/ITeam';
import Chat from '../chat/Chat';
import ReusedList from '../shared/components/ReusedList';
import CreateTeamForm from './CreateTeamForm/CreateTeamForm';
import JoinToTeamForm from './JoinTeamForm/JoinToTeamForm';
import TeamItem from './TeamItem/TeamItem';
import classes from './LobbyWhenUserNotInTeam.module.scss'
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';
import { fetchTeams } from '../../store/reducers/teams/actionCreators';
import { useAppDispatch, useAppSelector } from '../../store/hooks';

function LobbyWhenUserNotInTeam() {
    const [selectedTeam, setSelectedTeam] = useState<ITeam>({ id: 1, teamName: '', users: [] })
    const [openCreateTeamForm, setOpenCreateTeamForm] = useState<boolean>(false);
    const [openJoinTeamForm, setOpenJoinTeamForm] = useState<boolean>(false);
    const [disableJoinButton, setDisableJoinButton] = useState<boolean>(true);
    //const [teams, setTeams] = useState<ITeam[]>([]);
    const [errors, setErrors] = useState<IError[]>([]); //вспомнить зачем вот это 
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const dispatch = useAppDispatch();
    const teams = useAppSelector(state => state.teamReduser.teams);

    

    const handleClickOpenCreateTeamForm = () => {
        setOpenCreateTeamForm(true);
    }

    const handleClickCloseCreateTeamForm = () => {
        setOpenCreateTeamForm(false);
    }

    const handleClickCloseJoinTeamForm = () => {
        setOpenJoinTeamForm(false);
    }

    const toggleVisibleJoinForm = (flag: boolean) => {
        setOpenJoinTeamForm(flag);
    }

    const toggleVisibleCreateForm = (flag: boolean) => {
        setOpenCreateTeamForm(flag);
    }

    // Обработчик нажатия по команде из списка
    const handleListIndexClick = (selectedTeamIndex: number) => {
        setOpenJoinTeamForm(true);
        const team = teams.find(t => t.id == selectedTeamIndex);
        if (team) {
            setSelectedTeam(team);
        }
        const numberOfUsers = team == undefined ? 0 : team.users.length;
        if (numberOfUsers > 4) {
            setDisableJoinButton(true);
        }
        else {
            setDisableJoinButton(false);
        }
    };

    return (
        <>
            {isLoading ?
                <div className={classes.loading}>
                    <h1>Идет загрузка</h1>
                </div>
                :
                <>
                    <div className={classes.chatBlock}>
                        <Chat group='generalGroup' />
                    </div>
                    <div className={classes.teamsBlock}>
                         <div className={classes.teamsContent}>
                            <div className={classes.teamsList}>
                                <div className={classes.createTeamBlock}>
                                    <div className={classes.addImage} onClick={handleClickOpenCreateTeamForm}></div>
                                    <CustomModal visible={openCreateTeamForm} setVisible={toggleVisibleCreateForm}>
                                        <CreateTeamForm closeForm={handleClickCloseCreateTeamForm} />
                                    </CustomModal>
                                </div>
                                {teams.map(team =>
                                    <div className={classes.teamItemWrapper} key={team.id}>
                                        <TeamItem
                                            selectedIndex={selectedTeam.id}
                                            onClickListItem={handleListIndexClick}
                                            team={team}
                                            users={team.users}
                                            key={team.id}
                                        />
                                    </div>
                                )}
                                <CustomModal visible={openJoinTeamForm} setVisible={toggleVisibleJoinForm} >
                                    <JoinToTeamForm team={selectedTeam} closeForm={handleClickCloseJoinTeamForm} />
                                </CustomModal>
                            </div>
                        </div> 
                    </div>
                </>
            }
        </>
    )
}

export default LobbyWhenUserNotInTeam
