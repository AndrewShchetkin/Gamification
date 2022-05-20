import { Collapse, IconButton, ListItem, ListItemButton, Typography } from '@mui/material';
import React, { FC, useState } from 'react';
import ReusedList from '../shared/components/ReusedList';
import UserItem from './UserItem';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import ArrowDropUpIcon from '@mui/icons-material/ArrowDropUp';
import ArrowDropDown from '@mui/icons-material/ArrowDropDown';
import { Label } from '@mui/icons-material';
import Box from '@mui/system/Box';
import { IUser } from '../../@types/IUser';
import { ITeam } from '../../@types/ITeam';

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
        <>
            <ListItemButton sx={{ display: 'flex', justifyContent: 'space-between' }}
                selected={team.id === selectedIndex}
                onClick={() => onClickListItem(team.id)}
            >
                {team.teamName}
                <Box sx={{display: 'flex' ,justifyContent: 'space-between' }}>
                    <Typography padding={1}>{team.users.length}/{maxNumbesOfUsers}</Typography>
                    <IconButton onClick={onArrowClick}>
                        {open ? <ArrowDropUpIcon /> : <ArrowDropDown />}
                    </IconButton>
                </Box>
            </ListItemButton>
            <Collapse in={open} timeout="auto" unmountOnExit>
                <ReusedList items={users} renderItem={(user: IUser) =>
                    <UserItem user={user} key={user.id} />}
                />
            </Collapse>
        </>
    )
}

export default TeamItem;