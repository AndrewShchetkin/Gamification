import React from "react";
import SignIn from "./auth/SignIn";
import SignUp from "./auth/SignUp";
import { createBrowserHistory } from 'history';
import { Route, Router } from "react-router";
import PrivateRoute from "./components/PrivateRoute";
import { Temp } from "./components/temp";
import { Temp2 } from "./components/temp2";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import { userOkFetch } from "./auth/authSlice";
import Quiz from "./quiz/Quiz";
import Lobby from "./components/lobby/Lobby";
import Home from "./components/Home";
import Navigation from "./components/Navigation";
import Help from "./components/Help";
import Map from "./components/Map";

const App = () => {
    const history = createBrowserHistory();
    // const isAuthenticated = useAppSelctor(state => state.auth.isAuthenticated)
    // if (!isAuthenticated) {
        const dispatch = useAppDispatch();
        dispatch(userOkFetch());
    // }
    return (<>
    <Router history={history}>
        <Navigation/>
        
            <Route exact path="/" component={Home} />
            <Route  path="/nav" component={Navigation} />
            <Route  path="/help" component={Help} />
            <Route path='/signin' component={SignIn} />
            <Route path='/signup' component={SignUp} />
            <Route path='/lobby' component={Lobby} />
            <PrivateRoute path='/quiz' component={Quiz} />
            <PrivateRoute path='/game' component={Temp} />
            <PrivateRoute path='/game2' component={Temp2} />
            <Route path='/map' component={Map} />
        </Router>
        </>
    );
}

export default App;