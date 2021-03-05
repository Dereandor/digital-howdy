import React from 'react';
import { NavLink } from 'react-router-dom';

import './navbar.css';

const Navbar = () => {
    const onLogout = () => {
        localStorage.setItem('token', '');
        localStorage.setItem('loggedIn', '');
    }

    return(
        <nav className="navbar">
            <ul className="navbar-nav">
                <li className="nav-item">
                    <NavLink to="/dashboard/checkout" activeClassName="nav-link-active" className="nav-link">
                        <i className="fas fa-sign-out-alt fa-2x"></i>
                        <span className="link-text">Checkout</span>
                    </NavLink>
                </li>
                <li className="nav-item">
                    <NavLink to="/dashboard/visitorlog" activeClassName="nav-link-active" className="nav-link">
                        <i className="fas fa-receipt fa-2x"></i>
                        <span className="link-text">Visitor Log</span>
                    </NavLink>
                </li>
                <li className="nav-item">
                    <NavLink to="/dashboard/eventlog" activeClassName="nav-link-active" className="nav-link">
                        <i className="fas fa-history fa-2x"></i>
                        <span className="link-text">Event Log</span>
                    </NavLink>
                </li>
                <li className="nav-item">
                    <NavLink to="/dashboard/admins" activeClassName="nav-link-active" className="nav-link">
                        <i className="fas fa-user fa-2x"></i>
                        <span className="link-text">Admins</span>
                    </NavLink>
                </li>
                <li className="nav-item">
                    <NavLink onClick={onLogout} to="/login" activeClassName="nav-link-active" className="nav-link">
                        <i className="fas fa-user-circle fa-2x"></i>
                        <span className="link-text">Logout</span>
                    </NavLink>
                </li>
            </ul>
        </nav>
    );
}

export default Navbar;