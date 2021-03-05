import * as React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import './header.css';

import logo from '../../assets/logo.png';
import globe from '../../assets/globe.png';
import { useEffect } from 'react';


const Header = () => {
    const { t, i18n } = useTranslation();
    const location = useLocation();

    const [username, setUsername] = React.useState<string | null>(null);

    useEffect(() => {
        setUsername(localStorage.getItem('loggedIn'));
    }, [location]);

    const showUser = (): boolean => {
        return location.pathname.includes('/dashboard/');
    }

    const changeLanguage = (lng: string) => {
        i18n.changeLanguage(lng);
    }

    return(
        <div className="header">
            <Link to="/" >
                <img className="logo-header" src={logo} alt="TietoEVRY"/>
            </Link>

            <div style={{display: "inline-block"}}>
                {showUser() ?
                <div className="user-display">
                    <p>{username}</p>
                    <i className="fas fa-user"></i>
                </div>
                : null}
                <div className="dropdown">
                    <a href="# " className="lang">{t('header.lang')}</a>
                    <img className="globe" src={globe} alt="globe"  />
                    <div className="dropdown-content" style={{right: 0}}>
                        <a href="#NO" onClick={() => changeLanguage('no')}>Norsk</a>
                        <a href="#EN" onClick={() => changeLanguage('en')}>English</a>
                        <a href="#SV" onClick={() => changeLanguage('sv')}>Svensk</a>
                        <a href="#DA" onClick={() => changeLanguage('da')}>Dansk</a>
                        <a href="#FI" onClick={() => changeLanguage('fi')}>Suomi</a>
                        <a href="#DE" onClick={() => changeLanguage('de')}>Deutsch</a>
                        <a href="#FR" onClick={() => changeLanguage('fr')}>Français</a>
                        <a href="#ES" onClick={() => changeLanguage('es')}>Español</a>
                    </div>
                </div>
            </div>
            
        </div>

    )
}

export default Header;