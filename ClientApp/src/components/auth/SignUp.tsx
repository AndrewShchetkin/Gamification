import * as React from 'react';
import { Redirect } from 'react-router-dom';
import axios, { AxiosError } from 'axios';
import classes from './auth.module.scss';
import Input from '../shared/components/UI/input/input';


export default function SignUp() {
  const [redirectToReferrer, setRedirectToReferrer] = React.useState(false)

  if (redirectToReferrer === true) {
    return <Redirect to='/signin' />
  }

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const body = {
      UserName: data.get('email'),
      Password: data.get('password'),
    };

    await axios.post("api/auth/register", body)
      .then((response) => {
        if (response.status === 201) {
          setRedirectToReferrer(true);
        }
      })
      .catch((error: AxiosError) => {
        console.log(error.message);
      });
  };

  return (
  <div className={classes.formClass}>
    <h2>
      Зарегистрируйтесь
    </h2>
    <form onSubmit={handleSubmit} >

      <Input
        name='email'
        placeholder='username'
        label='Имя пользователя'
        // type='email' 
        />
      <Input
        name='password'
        placeholder='password'
        label='Имя пользователя'
        type='password' />


      <button type="submit" >
      Подтвердить
      </button>
    </form>
  </div>
  );
}