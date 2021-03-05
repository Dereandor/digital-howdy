import React from 'react';
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import './welcome.css';

import logo from '../../assets/logo.png';
import qrcode from '../../assets/qr-code.png';
import denmark from '../../assets/flags/denmark.png';
import finland from '../../assets/flags/finland.png';
import france from '../../assets/flags/france.png';
import germany from '../../assets/flags/germany.png';
import norway from '../../assets/flags/norway.png';
import spain from '../../assets/flags/spain.png';
import sweden from '../../assets/flags/sweden.png';
import uk from '../../assets/flags/uk.png';

const Welcome = () => {
    const history = useHistory();

    const { t, i18n } = useTranslation();

    const changeLanguage = (lng: string) => {
        i18n.changeLanguage(lng);
    }

    const goToRegistration = (lng: string) => {
        changeLanguage(lng);
        history.push('/registration');
    }

    return (
        <div className="content center">
            <div className="title">
                <h1>{t('welcome.welcome')}</h1>
                <img className="logo-home" src={logo} alt="TietoEVRY logo"/>
            </div>
            <div className="qr-code">
                <h2>{t('welcome.scan')}</h2>
                <img className="qr-image" src={qrcode} alt="qr-code" />
            </div>
            <div className="flag-container">
                <img src={norway} className="flag-icon" alt="Norsk" onClick={() => goToRegistration('no')}/>
                <img src={uk} className="flag-icon" alt="English" onClick={() => goToRegistration('en')}/>
                <img src={sweden} className="flag-icon" alt="Svensk" onClick={() => goToRegistration('sv')}/>
                <img src={denmark} className="flag-icon" alt="Dansk" onClick={() => goToRegistration('da')}/>
                <img src={finland} className="flag-icon" alt="Suomi" onClick={() => goToRegistration('fi')}/>
                <img src={germany} className="flag-icon" alt="Deutsch" onClick={() => goToRegistration('de')}/>
                <img src={france} className="flag-icon" alt="Français" onClick={() => goToRegistration('fr')}/>
                <img src={spain} className="flag-icon" alt="Español" onClick={() => goToRegistration('es')}/>
            </div>
        </div>
    )
}

export default Welcome;