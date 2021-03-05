import axios from 'axios';
import * as config from '../config/config.json';
import { AdminLogin, AdminRegister, LoginResponse, RegisterResponse, AdminResult } from '../types/admin';

const url = config.apiBaseUrl;

export const loginAdmin = async (adminLogin: AdminLogin): Promise<LoginResponse> => {
    try {
        const result = await axios.post(`${url}/admins/login`, adminLogin);
        const response: LoginResponse = {
            data: result.data,
            error: ''
        };

        const token = response.data?.token ?? '';

        localStorage.setItem('token', token);
        localStorage.setItem('loggedIn', adminLogin.username);

        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response: LoginResponse = {
            data: null,
            error: error
        }
        return response;
    }
}

export const validateToken = async (): Promise<boolean> => {
    try {
        await axios.post(`${url}/admins/validate`, {});

        return true;
    } catch (e) {
        return false;
    }
}

export const registerAdmin = async (data: AdminRegister): Promise<RegisterResponse> => {
    try {
        const response = await axios.post(`${url}/admins`, data);

        return {
            data: response.data,
            error: ''
        }
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response: RegisterResponse = {
            data: null,
            error: error
        }
        return response;
    }
}

export const getAllAdmins = async (): Promise<AdminResult> => {
    try {
        const result = await axios.get(`${url}/admins`);
        const response = {
        data: result.data,
        error: ''
        }
        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response = {
            data: null,
            error: error
        }
        return response;
    }
}

export const deleteAdmin = async (Id: number) => {
    try {
        console.log('yo');
        const result = await axios.delete(`${url}/admins/${Id}`);
        const data = result.data;
        return data;
    } catch (e) {
        return null;
    }
}