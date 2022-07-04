import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { IError } from '../../@types/IError';
import { ITeam } from '../../@types/ITeam';
import Chat from '../chat/Chat';
import ReusedList from '../shared/components/ReusedList';
import CreateTeamForm from './CreateTeamForm';
import JoinToTeamForm from './JoinToTeamForm';
import TeamItem from './TeamItem';
import classes from './Lobby.module.scss'
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import CustomModal from '../shared/components/UI/CustomModal/CustomModal';

function TeamsInfoWhenUserNotInTeam() {
    const [selectedTeam, setSelectedTeam] = useState<ITeam>({ id: 0, teamName: '', users: [] })
    const [openCreateTeamForm, setOpenCreateTeamForm] = useState<boolean>(false);
    const [openJoinTeamForm, setOpenJoinTeamForm] = useState<boolean>(false);
    const [disableJoinButton, setDisableJoinButton] = useState<boolean>(true);
    const [teams, setTeams] = useState<ITeam[]>([]);
    const [errors, setErrors] = useState<IError[]>([]); //вспомнить зачем вот это 
    const [isLoading, setIsLoading] = useState<boolean>(true);

    async function fetchTeams() {
        try {
            const response = await axios.get<ITeam[]>('api/team/getallteams')
            // const response: ITeam[] = [
            //     {id: 123141 , teamName: "Team1", users: [ { id: 'sdf', userName: 'User1', teamId: "1"}, { id: 'sdfa', userName: 'User2', teamId: "1"}]},
            //     {id: 2141241 , teamName: "Team2", users: [ { id: 'sdf', userName: 'User1', teamId: "2"}]}
            // ]
            setTeams(response.data);
            //setTeams(response)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        fetchTeams();
    }, [])

    const handleClickOpenCreateTeamForm = () => {
        setOpenCreateTeamForm(true);
    }

    const handleClickCloseCreateTeamForm = () => {
        setOpenCreateTeamForm(false);
    }

    const handleClickOpenJoinTeamForm = () => {
        debugger;
        setOpenJoinTeamForm(true);
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
        debugger;
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
                        <Chat chatRoom='generalRoom' />
                    </div>
                    <div className={classes.teamsBlock}>
                        <div>Команды, доступные для выбора:</div>
                        <div className={classes.teamsContent}>
                            <div className={classes.teamsList}>
                            <ReusedList items={teams} renderItem={(team: ITeam) =>
                                    <TeamItem
                                        selectedIndex={selectedTeam.id}
                                        onClickListItem={handleListIndexClick}
                                        team={team}
                                        users={team.users}
                                        key={team.id}
                                    />
                                }
                            />
                            </div>
                            <div className={classes.teamsFooter}>
                                <CustomButton 
                                    onClick={handleClickOpenJoinTeamForm}
                                >Присоедениться к команде</CustomButton>
                                <CustomModal visible={openJoinTeamForm} setVisible={toggleVisibleJoinForm} >
                                     <JoinToTeamForm team={selectedTeam} closeForm={handleClickCloseJoinTeamForm} /> 
                                </CustomModal>
                                <CustomButton
                                    onClick={handleClickOpenCreateTeamForm}
                                >Создать команду</CustomButton>
                                <CustomModal visible={openCreateTeamForm} setVisible={toggleVisibleCreateForm}>
                                    <CreateTeamForm closeForm={handleClickCloseCreateTeamForm} />
                                </CustomModal>
                            </div>
                        </div>
                    </div>
                </>
                // ? <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                //     <CircularProgress />
                // </Box>
                // :
                // <>
                //     <Box className="chatBlock"
                //         sx={{
                //             flex: '0 0 30%',
                //             maxWidth: "30%"
                //         }}>
                //         <Chat chatRoom='generalRoom' />
                //     </Box>
                //     <Box className="teamsBlock"
                //         sx={{
                //             bgcolor: '#fff',
                //             display: 'flex',
                //             flex: '1 1 auto',
                //             flexDirection: 'column'
                //         }}>
                //         <Box sx={{
                //             flex: '0 0 10%',
                //             bgcolor: '#c1c7b7'
                //         }}>Информация о командах</Box>
                //         <Box sx={{ flexGrow: 10, display: 'flex', flexDirection: 'column' }}>
                //             <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
                //                 <ReusedList items={teams} renderItem={(team: ITeam) =>
                //                     <TeamItem
                //                         selectedIndex={selectedTeam.id}
                //                         onClickListItem={handleListIndexClick}
                //                         team={team}
                //                         users={team.users}
                //                         key={team.id}
                //                     />
                //                 }
                //                 />
                //             </Box>
                //             <Box sx={{
                //                 flexGrow: 1, bgcolor: '#fff', display: 'flex', alignItems: 'center', justifyContent: 'flex-end'
                //             }}>
                //                 <Button
                //                     variant="outlined"
                //                     sx={{ mr: 1, ml: 1 }}
                //                     onClick={handleClickOpenJoinTeamForm}
                //                     disabled={disableJoinButton}
                //                 >Присоедениться</Button>
                //                 <Dialog open={openJoinTeamForm} onClose={handleClickCloseJoinTeamForm}>
                //                     <JoinToTeamForm team={selectedTeam} closeForm={handleClickCloseJoinTeamForm} />
                //                 </Dialog>
                //                 <Button variant="outlined" sx={{ mr: 1, ml: 1 }} onClick={handleClickOpenCreateTeamForm}>Создать</Button>
                //                 <Dialog open={openCreateTeamForm} onClose={handleClickCloseCreateTeamForm}>
                //                     <CreateTeamForm closeForm={handleClickCloseCreateTeamForm} />
                //                 </Dialog>
                //             </Box>
                //         </Box>
                //     </Box>
                // </>

            }
        </>





        // <Box sx={{ flexGrow: 10, display: 'flex', flexDirection: 'column'}}>
        //     <Box sx={{ flexGrow: 10, bgcolor: '#fff' }}>
        //         <ReusedList items={teams} renderItem={(team: ITeam) =>
        //             <TeamItem
        //                 selectedIndex={selectedTeam.id}
        //                 onClickListItem={handleListIndexClick}
        //                 team={team}
        //                 users={team.users}
        //                 key={team.id}
        //             />
        //         }
        //         />
        //     </Box>
        //     <Box sx={{
        //         flexGrow: 1, bgcolor: '#fff', display: 'flex', alignItems: 'center', justifyContent: 'flex-end'
        //     }}>
        //         <Button
        //             variant="outlined"
        //             sx={{ mr: 1, ml: 1 }}
        //             onClick={handleClickOpenJoinTeamForm}
        //             disabled={disableJoinButton}
        //         >Присоедениться</Button>
        //         <Dialog open={openJoinTeamForm} onClose={handleClickCloseJoinTeamForm}>
        //             <JoinToTeamForm team={selectedTeam} closeForm={handleClickCloseJoinTeamForm}/> 
        //         </Dialog>
        //         <Button variant="outlined" sx={{ mr: 1, ml: 1 }} onClick={handleClickOpenCreateTeamForm}>Создать</Button>
        //         <Dialog open={openCreateTeamForm} onClose={handleClickCloseCreateTeamForm}>
        //             <CreateTeamForm closeForm={handleClickCloseCreateTeamForm} />
        //         </Dialog>
        //     </Box>
        // </Box>

    )
}

export default TeamsInfoWhenUserNotInTeam
