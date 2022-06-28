import React from 'react';
import { useAppDispatch } from '../../store/hooks';
import { signInComplete } from '../../store/reducers/auth/authSlice';
import { Redirect, Link } from 'react-router-dom';
import { LoginResponse } from '../../@types/loginResponse';
import axios, { AxiosError } from 'axios';
import "./signin.scss"




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
    <div className='form'>
      <h1>
        Войдите в свой аккаунт
      </h1>
      <form onSubmit={handleSubmit} >

        <label>
          Имя пользователя
          <input
            required
            id="email"
            name="email"
            autoComplete="email"
            autoFocus
            placeholder='username'
          />

        </label>
        <label>
          Пароль
          <input
            required
            name="password"
            type="password"
            id="password"
            autoComplete="current-password"
          />
        </label>

        <button type="submit" >
          Войти
        </button>

        <Link to='/signup' >
          Регистрация
        </Link>
      </form>
    </div>
  );
}
