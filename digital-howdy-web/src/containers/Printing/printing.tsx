import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import './printing.css';

import loading from '../../assets/loading_4.svg';

const Printing = () => {
    const history = useHistory();

    const { t } = useTranslation();

    // TODO: Obviously set this to rather be triggered by a printing callback (if that's even possible)
    useEffect(() => {
        setTimeout(() => {
            goToFinished();
        }
        , 5000);
    })

    const goToFinished = () => {
        history.push('/finished');
    }

    return (
        <div className="content center">
            <div className="container">
                <p className="heading">{t('printing.please')}</p>
                <img className="loading" src={loading} alt="Loading" />
            </div>
        </div>
    )
}

export default Printing;