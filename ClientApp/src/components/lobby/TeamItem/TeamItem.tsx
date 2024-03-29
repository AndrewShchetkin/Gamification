import React, { FC, useState } from 'react';
import ReusedList from '../../shared/components/ReusedList';
import UserItem from '../UserItem/UserItem';
import { IUser } from '../../../@types/IUser';
import { ITeam } from '../../../@types/ITeam';
import classes from './TeamItem.module.scss'

interface ITeamProps {
    team: ITeam;
    users: IUser[];
    selectedIndex: number;
    onClickListItem: (selectedTeam: number) => void;
}

const TeamItem: FC<ITeamProps> = ({ team, users, selectedIndex, onClickListItem }) => {
    const [open, setOpen] = useState(false)
    const maxNumbesOfUsers = 5;

    const onArrowClick = (event: React.MouseEvent) => {
        event.stopPropagation();
        setOpen(!open);
    }

    return (
            <div className={classes.teamItem} onClick={() => onClickListItem(team.id)}>
                <div className={classes.teamItemInfo}>
                    <div className={classes.userCount}>{team.users.length}/{maxNumbesOfUsers}</div>
                    <div>{team.teamName}</div>
                </div>
            </div>
    )
}

export default TeamItem;
