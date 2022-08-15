import { useAppSelector } from "../../store/hooks";
import classes from './Header.module.scss';
import Logo from '../../img/Logo.svg';
import Name from '../../img/Name.svg';
import { Link, Redirect } from "react-router-dom";
import { CustomButton } from "../shared/components/UI/CustomButton/CustomButton";
import { useState } from "react";

interface IHeaderProps {
    isSignIn: boolean | undefined;
}

export default function SignInSignUpHeader(props: IHeaderProps) {

    const { isSignIn } = props;
    const [redirectToSignUp, setRedirectToSignUp] = useState<boolean>(false)
    const [redirectToSignIn, setRedirectToSignIn] = useState<boolean>(false)

    if (redirectToSignUp === true) {
        return <Redirect to='/signup' />
    }

    if (redirectToSignIn === true) {
        return <Redirect to='/signin' />
    }

    return (
        <div className={classes.navBar} >
            <div className={classes.logo} >
                <Link to={"/"} >
                    <Logo />
                </Link>
                <Name />
            </div>
            <div className={classes.menu}>
                {isSignIn ?
                    <div>
                        <CustomButton
                            onClick={() => setRedirectToSignUp(true)}
                            style={{
                                backgroundColor: "rgba(190, 209, 232, 0.7)",
                                height: '40px',
                                width: '120px',
                                fontSize: "15px"
                            }}>Регистрация</CustomButton>
                    </div>
                    :
                    <div>
                        <CustomButton 
                        onClick={() => setRedirectToSignIn(true)}
                        style={{
                            backgroundColor: "rgba(190, 209, 232, 0.7)",
                            height: '40px',
                            width: '100px',
                            fontSize: "15px"
                        }}>Вход</CustomButton>
                    </div>}
            </div>
        </div>)

}