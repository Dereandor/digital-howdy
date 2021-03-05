import React from "react";
import "./App.css";
import axios from 'axios';
import { Route, Switch, withRouter } from "react-router-dom";
import { Helmet } from "react-helmet";
import Welcome from "./containers/Welcome/welcome";
import Registration from "./containers/Registration/registration";
import Printing from "./containers/Printing/printing";
import Finished from "./containers/Finished/finished";
import { TransitionGroup, CSSTransition } from "react-transition-group";
import Header from "./components/Header/header";
import VisitorLog from "./containers/VisitorLog/visitorLog";
import Checkout from "./containers/Checkout/checkout";
import Login from "./containers/Login/login";
import PrivateRoute from "./privateRoute";
import Admins from "./containers/Admins/admins";
import EventLog from "./containers/EventLog/eventLog";

function App({ location } : {location:any}) {

  axios.defaults.headers.common['Authorization'] = 'bearer ' + localStorage.getItem('token')

  return (
    <div className="App">
      <Helmet>
        <title>Digital Howdy</title>
        <script src="https://kit.fontawesome.com/6c7477558c.js" crossOrigin="anonymous"></script>
      </Helmet>
      <Header/>
      <TransitionGroup className="pager">
        <CSSTransition
          key={location.key}
          timeout={{ enter: 800, exit: 800 }}
          classNames={"slide"}
        >
          <section className="route-section">
          <Switch location={location}>
            route, as each page is dependent on some state from the previous
            page
            <Route exact path="/" component={Welcome} />
            <Route exact path="/registration" component={Registration} />
            <Route exact path="/printing" component={Printing} />
            <Route exact path="/finished" component={Finished} />
            <PrivateRoute exact path="/dashboard/checkout" component={Checkout} />
            <PrivateRoute exact path="/dashboard/visitorlog" component={VisitorLog} />
            <PrivateRoute exact path="/dashboard/admins" component={Admins} />
            <PrivateRoute exact path="/dashboard/eventlog" component={EventLog} />
            <Route exact path="/login" component={Login} />
            <Route component={Welcome}/>
          </Switch>
          </section>
        </CSSTransition>
      </TransitionGroup>
    </div>
  );
}

export default withRouter(App);
