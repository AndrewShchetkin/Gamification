import { ListItem } from '@mui/material';
import React, {FC} from 'react';
import { IUser } from '../../@types/IUser';

interface IUserProps{
    user: IUser
}

const UserItem: FC<IUserProps> = ({user}) => {
    return(
        <ListItem sx={{ pl: 4 }}>
            {user.userName}
            
        </ListItem>
    )
}

export default UserItem;