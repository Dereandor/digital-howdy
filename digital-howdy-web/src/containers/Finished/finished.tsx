import React from 'react';
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import './finished.css';

import check from '../../assets/black-check.png';

const Finished = () => {
    const history = useHistory();

    const { t, i18n } = useTranslation();

    const changeLanguage = (lng: string) => {
        i18n.changeLanguage(lng);
    }

    const goToWelcome = (lng: string) => {
        changeLanguage(lng);
        history.push('/');
    }

    return (
        <div className="content center">
            <div className="container">
                <p className="heading secondary">{t('finished.done')}</p>
                <img className="check-image" src={check} alt="Done"></img>
                <p className="information secondary">{t('finished.disclaimer')}</p>
                <button type="button" onClick={() => goToWelcome('en')}>{t('finished.button')}</button>
            </div>
        </div>
    )
}

export default Finished;