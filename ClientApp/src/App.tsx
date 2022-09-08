import { Suspense, useEffect, useState } from "react";

import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "./components/PrivateRoute";
import { useAppDispatch } from "./store/hooks";
import { fetchUser } from "./store/reducers/auth/actionCreators";

import AdminPanel from "./components/AdminPanel/AdminPanel";
import './app.module.scss';
import React from "react";
import { LoadAnimation } from "./components/shared/components/UI/LoadAnimation";

const Map = React.lazy(() => import("./components/Map"));
const Lobby = React.lazy(() => import("./components/lobby/Lobby"));
const Quiz = React.lazy(() => import("./components/quiz/Quiz"));
const Home = React.lazy(() => import("./components/Home"));
const Help = React.lazy(() => import("./components/Help"));
const SignIn = React.lazy(() => import("./components/auth/SignIn"));
const SignUp = React.lazy(() => import("./components/auth/SignUp"));


//const history = createBrowserHistory();
// перемещено из компонента
const App = () => {
    const userRole = useAppSelector(state => state.authReduser.role); // добавлено для отсл. роли юзера
    const dispatch = useAppDispatch();
    useEffect(() => {
        dispatch(fetchUser());
    }, [])

    return (<>

        <BrowserRouter>
            <Switch>
                <Route exact path="/" >

                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <Home />
                        </section>
                    </Suspense>
                </Route>
                <Route path="/help" >
                    <Suspense fallback={<LoadAnimation />}>
                        <section>

                            <Help />
                        </section>
                    </Suspense>
                </Route>
                <Route path='/signin' >

                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <SignIn />
                        </section>
                    </Suspense>
                </Route>
                <Route path='/signup' >
                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <SignUp />
                        </section>
                    </Suspense>
                </Route>
                <Route path='/lobby'>
                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <Lobby />
                        </section>
                    </Suspense>
                </Route>
                <Route path='/quiz'>
                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <Quiz />
                        </section>
                    </Suspense>
                </Route>
                {/* <PrivateRoute path='/game' component={Temp} /> */}
                {/* <PrivateRoute path='/game2' component={Temp2} /> */}
                <Route exact path="/map">
                    <Suspense fallback={<LoadAnimation />}>
                        <section>
                            <Map />
                        </section>
                    </Suspense>
                </Route>
            </Switch>
        </BrowserRouter>

    </>
    );
}

export default App;