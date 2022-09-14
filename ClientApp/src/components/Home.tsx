import React from "react";
import { Redirect } from "react-router-dom";
import classes from './Home.module.scss'
import { CustomButton } from "./shared/components/UI/CustomButton/CustomButton";
export default function Home() {

    const [redirectToSignUp, setRedirectToSignUp] = React.useState(false)
    const [redirectToSignIn, setRedirectToSignIn] = React.useState(false)

    if (redirectToSignUp === true) {
        return <Redirect to='/signup' />
    }

    if (redirectToSignIn === true) {
        return <Redirect to='/signin' />
    }

    const redirectToSignUpPage = () => {
        setRedirectToSignUp(true);
    }

    const redirectToSignInPage = () => {
        setRedirectToSignIn(true);
    }


    return (
        <div>
            <div className={classes.btnsBlock}>
                <CustomButton
                    onClick={redirectToSignUpPage}
                    style={{
                        width: '160px',
                        backgroundColor: 'rgba(190, 209, 232, 0.7)',
                        height: '50px',
                        fontSize: '20px'
                    }}>Регистрация</CustomButton>
                <CustomButton
                    onClick={redirectToSignInPage} 
                    style={{
                        width: '120px',
                        fontSize: '20px'
                }}>Вход</CustomButton>
            </div>
            <div className={classes.mainBlock}>
                <div className={classes.logo}></div>
                <div className={classes.name}></div>
            </div>
        </div>
    )
}