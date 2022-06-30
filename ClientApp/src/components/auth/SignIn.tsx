import React from 'react';
import { useAppDispatch } from '../../store/hooks';
import { signInComplete } from '../../store/reducers/auth/authSlice';
import { Redirect, Link } from 'react-router-dom';
import { LoginResponse } from '../../@types/loginResponse';
import axios, { AxiosError } from 'axios';
import classes from "./signin.module.scss"
import Input from '../shared/components/UI/input/input';




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
        alert(error.message);
      });
  };

  return (
    <div className={classes.formClass}>
      <h2>
        Войдите в свой аккаунт
      </h2>
      <form onSubmit={handleSubmit} >

        <Input
          name='email'
          placeholder='username'
          label='Имя пользователя'
          type='email' />
        <Input
          name='password'
          placeholder='password'
          label='Имя пользователя'
          type='password' />


        <button type="submit" >
          Войти
        </button>
      </form>

      <Link to='/signup' className={classes.signup}>
        Регистрация
      </Link>
    </div>
  );
}
