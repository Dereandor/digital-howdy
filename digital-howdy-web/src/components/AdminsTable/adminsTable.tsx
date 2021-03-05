import React, { MouseEvent, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Admin } from '../../types/admin';
import { deleteAdmin } from '../../services/adminService';

import './adminsTable.css';

type Props = {
    admins: Admin[]
    onSortRequested: (key: string, reverse: boolean) => void
    onAdminDeleted: () => void
}

const AdminsTable = (props: Props) => {
    const { t } = useTranslation();

    const [previousColumnTarget, setPreviousColumnTarget] = useState<EventTarget & HTMLTableHeaderCellElement>();

    const onHeaderClick = (e: MouseEvent<HTMLTableHeaderCellElement>) => {
        const previousKey = previousColumnTarget?.getAttribute("value-key") ?? '';
        setPreviousColumnTarget(e.currentTarget);
        const key = e.currentTarget.getAttribute("value-key") ?? '';
        const valueSort = e.currentTarget.getAttribute("value-sort") ?? '';
        const reverse = (key === previousKey);
        deselectPreviousColumn();
        e.currentTarget.setAttribute("value-sort", valueSort === "up" ? "down" : "up");
        props.onSortRequested(key, reverse);
    }

    const deselectPreviousColumn = () => {
        previousColumnTarget?.setAttribute('value-sort', '');
    }

    const onClick = async (id: number) => {
        await deleteAdmin(id);
        props.onAdminDeleted();
    }

    return(
        <div>
            <table className="overview-table">
                <tr>
                    <th onClick={onHeaderClick} value-key="Username">
                        {t('admins.username2')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th value-key="id">
                        {t('admins.delete')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                </tr>
                {props.admins.map((admin, index) => {
                    return(
                        <tr key={index}>
                            <td>
                                {admin.username}
                            </td>
                            <td>
                                {admin.username !== localStorage.getItem('loggedIn') ?
                                <button type="submit" onClick={() => onClick(admin.id)}>{t('admins.delete')}</button>
                                : t('admins.this-is-you')}
                            </td>
                        </tr>
                    )
                })}
            </table>
        </div>
    )
}

export default AdminsTable;