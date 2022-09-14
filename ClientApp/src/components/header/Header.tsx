import { useAppSelector } from "../../store/hooks";
import classes from './Header.module.scss';
import Logo from '../../img/Logo.svg';
import Name from '../../img/Name.svg';
import { Link, Redirect } from "react-router-dom";
import { CustomButton } from "../shared/components/UI/CustomButton/CustomButton";

export default function Header() {

    const userName = useAppSelector(state => state.authReduser.userName);

    return (
        <div className={classes.navBar} >
            <div className={classes.logo} >
                <Link to={"/"} >
                    <Logo />
                </Link>
                <Name />
            </div>
            <div className={classes.menu}>
                <div style={{ display: 'flex', alignItems: "center" }}>
                    <div className={classes.personIcon}></div>
                    <div className={classes.userName}>{userName}</div>
                    <CustomButton
                        style={{
                            backgroundColor: "rgba(190, 209, 232, 0.7)",
                            height: '40px',
                            width: '145px',
                            fontSize: "18px"
                        }}>Выход</CustomButton>
                </div>
            </div>
        </div>)

}

