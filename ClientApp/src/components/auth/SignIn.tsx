import React from 'react';
import { useAppDispatch } from '../../store/hooks';
import { signInComplete } from '../../store/reducers/auth/authSlice';
import { Redirect, Link } from 'react-router-dom';
import { LoginResponse } from '../../@types/loginResponse';
import axios, { AxiosError } from 'axios';
import classes from "./signin.module.scss"
import { CustomInput } from '../shared/components/UI/CustomInput/CustomInput';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import SignInSignUpHeader from '../header/SignInSignUpHeader';


export default function SignIn() {
  const dispatch = useAppDispatch();

  const [redirectToReferrer, setRedirectToReferrer] = React.useState(false)

  if (redirectToReferrer === true) {
    return <Redirect to='/lobby' />
  }

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {

    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const body = {
      UserName: data.get('email'),
      Password: data.get('password'),
    };

    await axios.post<LoginResponse>("api/auth/login", body)
      .then((response) => {
        dispatch(signInComplete(response.data));
        setRedirectToReferrer(true);
      })
      .catch((error: AxiosError) => {
        alert(error.response?.data.message);
      });
  };

  return (
    <>
    <SignInSignUpHeader isSignIn={true}/>
    <div className={classes.wrapper}>
      <div className={classes.logoBlockWrapper}>
        <div className={classes.signInIMG}></div>
        <div className={classes.signInHeader}>Введите данные аккаунта</div>
      </div>
      <form
        className={classes.formContent}
        onSubmit={handleSubmit}>
        <p>Имя пользователя</p>
        <CustomInput
          style={{ marginBottom: '20px', height: '35px', fontSize: '18px' }}
          name="email"
          autoComplete='off' />
        <p>Пароль</p>
        <CustomInput
          style={{ marginBottom: '20px', height: '35px', fontSize: '18px'  }}
          type="password"
          name="password" />
        <div className={classes.btnBlock}>
          <CustomButton
            style={{
              width: '40%',
              height: '40px',
              fontSize: '20px'
            }}
            type="submit"
          >Войти</CustomButton>
        </div>

      </form>
    </div>
    </>
  );
}
