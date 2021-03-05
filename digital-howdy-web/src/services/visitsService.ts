import axios from 'axios';
import Visit, { VisitsResponse, VisitUpdateResponse } from '../types/visit';
import VisitUpdate from '../types/visitUpdate';
import FormData from '../types/formData';
import { RegistrationResponse } from '../types/registration';

import * as config from '../config/config.json';

const url = config.apiBaseUrl;

export const getAllVisits = async (query: string, currentlyIn: boolean, exact: boolean = false): Promise<VisitsResponse> => {
    try {
        const result = await axios.get(url + `/visits?query=${query}&exact=${exact}&currentlyIn=${currentlyIn}`);
        const response = {
            data: result.data,
            error: ''
        }
        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.!';

        const response = {
            data: null,
            error: error
        }
        return response;
    }
}

export const GetVisitById = async (Id: number) => {
    try {
        const result = await axios.get(`${url}/visits?Id=${Id}`);
        const data = result.data;
        return data;
    } catch (e) {
        return null;
    }
}

export const InsertVisit = async (newVisit: Visit) => {
    try {
        const result = await axios.post(`${url}/visits`, newVisit);
        const data = result.data;
        return data;
    } catch (e) {
        return null;
    }
}

export const registerVisit = async (formData: FormData): Promise<RegistrationResponse> => {
    try {
        const result = await axios.post(`${url}/visits`, formData);
        const response = {
            data: result.data,
            error: ''
        }
        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.!';

        const response = {
            data: null,
            error: error
        }
        return response;
    }
}

export const updateEndDateVisit = async (visitUpdate: VisitUpdate): Promise<VisitUpdateResponse> => {
    try {
        await axios.put(`${url}/visits`, visitUpdate);
        return { error: '' };
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.!';

        const response = {
            error: error
        }
        return response;
    }
}

export const DeleteVisit = async (Id: number) => {
    try {
        const result = await axios.delete(`${url}/visits?Id=${Id}`);
        const data = result.data;
        return data;
    } catch (e) {
        return null;
    }
}