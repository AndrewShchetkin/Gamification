import { Redirect, Route } from "react-router-dom";
import React from "react";
import { useAppSelector } from "../store/hooks";
import { LoadAnimation } from "./shared/components/UI/LoadAnimation";

const PrivateRoute: React.ComponentType<any> = ({
  component: Component,
  ...rest
}) => {
  const isAuthenticated = useAppSelector(state => state.authReduser.isAuthenticated) // получаем текущее состояние 
  const requestProcess = useAppSelector(state => state.authReduser.requestSended)

  return (
    <Route
      {...rest} //какое то наследование  
      render={props => 
        isAuthenticated ? (
          <Component {...props} />
        ) : 
        requestProcess ? (<LoadAnimation/>) : // меняется глобальный стейт почему то
        (
          <Redirect push to='/signin' />
        )
      }
    />
  );
};

export default PrivateRoute