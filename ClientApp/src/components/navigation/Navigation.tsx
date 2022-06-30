
import { Link } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";
import classes from './Navigation.module.scss'
import Logo from '../../img/Logo.svg';

export default function Navigation() {

    const isAuthenticated = useAppSelector(state => state.authReduser.isAuthenticated);
    const userName = useAppSelector(state => state.authReduser.userName);

    return (
        <div className={classes.navBar} >
            <div className={classes.logo} >

                <Link to={"/"} >
                    <Logo />
                </Link>

                <h2>The Game</h2>
            </div>
            <div className={classes.menu}>
                <ul>
                    <li><Link to={"/quiz"} >Quize</Link></li>
                    <li><Link to={"/Lobby"}>Lobby</Link></li>
                    <li><Link to={"/Help"}>Help</Link></li>
                </ul>

                {isAuthenticated ?
                    <div>{userName}</div> :
                    <div>
                        <Link to={"/signin"}
                            className={classes.active}>
                            SignIn
                        </Link>
                    </div>}
            </div>
        </div>)
}