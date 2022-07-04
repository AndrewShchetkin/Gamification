import React, {FC} from 'react';
import { IUser } from '../../@types/IUser';
import classes from './UserItem.module.scss'

interface IUserProps{
    user: IUser
}

const UserItem: FC<IUserProps> = ({user}) => {
    return(
        <div className={classes.userItem}>
            {user.userName}
        </div>
    )
}

export default UserItem;