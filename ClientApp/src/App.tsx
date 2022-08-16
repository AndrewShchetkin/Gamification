import { useEffect, useState } from "react";
import SignIn from "./components/auth/SignIn";
import SignUp from "./components/auth/SignUp";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "./components/PrivateRoute";
import { Temp } from "./components/temp";
import { Temp2 } from "./components/temp2";
import { useAppDispatch, useAppSelector } from "./store/hooks";
import { fetchUser } from "./store/reducers/auth/actionCreators";
import Quiz from "./components/quiz/Quiz";
import Lobby from "./components/lobby/Lobby";
import Home from "./components/Home";
import Help from "./components/Help";
import Map from "./components/Map";
import './app.module.scss';


const App = () => {
    // const history = createBrowserHistory();
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(fetchUser());
    }, [])
    
    return (<>

        <BrowserRouter>
            <Switch>
                <Route exact path="/" >
                    <Home />
                </Route>
                <Route path="/help" >
                    <Help/>
                    </Route>
                <Route path='/signin' >
                    <SignIn/>
                </Route>
                <Route path='/signup' component={SignUp} />
                <Route path='/lobby' component={Lobby} />
                <Route path='/quiz' component={Quiz} />
                <PrivateRoute path='/game' component={Temp} />
                <PrivateRoute path='/game2' component={Temp2} />
                <Route exact path="/map">
                    <Map />
                </Route>
            </Switch>
        </BrowserRouter>

    </>
    );
}

export default App;