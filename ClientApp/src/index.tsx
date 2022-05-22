import React from "react";
import ReactDOM from "react-dom";
import register from "./registerServiceWorker";
import App from "./App";
import { store } from "./app/store";
import { Provider } from "react-redux";
import "./web.config"


ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </React.StrictMode>,
  document.getElementById("root")
);

register();