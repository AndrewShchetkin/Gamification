import React, { useEffect } from "react";
import SignIn from "./components/auth/SignIn";
import SignUp from "./components/auth/SignUp";
import { createBrowserHistory } from 'history';
import { Route, Router } from "react-router";
import PrivateRoute from "./components/PrivateRoute";
import { Temp } from "./components/temp";
import { Temp2 } from "./components/temp2";
import { useAppDispatch, useAppSelector } from "./store/hooks";
import { fetchUser } from "./store/reducers/auth/actionCreators";
import Quiz from "./components/quiz/Quiz";
import Lobby from "./components/lobby/Lobby";
import Home from "./components/Home";
import Navigation from "./components/Navigation";
import Help from "./components/Help";
import Map from "./components/Map";
import AdminPanel from "./components/AdminPanel/AdminPanel";

const history = createBrowserHistory();
// перемещено из компонента
const App = () => {
    const userRole = useAppSelector(state => state.authReduser.role); // добавлено для отсл. роли юзера
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(fetchUser());
    }, [])
    return (<>
        <Router history={history}>
            <Navigation />
            <Route exact path="/" component={Home} />
            <Route path="/nav" component={Navigation} />
            <Route path="/help" component={Help} />
            <Route path='/signin' component={SignIn} />
            <Route path='/signup' component={SignUp} />
            <Route path='/lobby' component={Lobby} />
            <PrivateRoute path='/quiz' component={Quiz} />
            <PrivateRoute path='/game' component={Temp} />
            <PrivateRoute path='/game2' component={Temp2} />
            {userRole === 'admin' && <PrivateRoute path='/adminpanel' component={AdminPanel} /> }
            <Route path='/map' component={Map} />
        </Router>
    </>
    );
}

export default App;