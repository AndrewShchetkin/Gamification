import axios, { AxiosError } from 'axios';
import * as React from 'react';
import { Redirect } from 'react-router-dom';
import { CustomButton } from '../shared/components/UI/CustomButton/CustomButton';
import { CustomInput } from '../shared/components/UI/CustomInput/CustomInput';
import classes from './signup.module.scss';



function SignUp() {
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
    <div className={classes.wrapper}>
      <div className={classes.logoBlockWrapper}>
        <div className={classes.signUpIMG}></div>
        <div className={classes.signUpHeader}>Регистрация</div>
      </div>
      <form
        className={classes.formContent}
        onSubmit={handleSubmit}>
        <p>Имя пользователя</p>
        <CustomInput
          style={{ marginBottom: '20px', height: '55px', fontSize: '28px' }}
          name="email" />
        <p>Пароль</p>
        <CustomInput
          style={{ marginBottom: '20px', height: '55px', fontSize: '28px'  }}
          type="password"
          name="password" />
        <div className={classes.btnBlock}>
          <CustomButton
            style={{
              width: '70%',
              height: '60px',
              fontSize: '28px'
            }}
            type="submit"
          >Зарегистрироваться</CustomButton>
        </div>

      </form>
    </div>
  )
}

export default SignUp
