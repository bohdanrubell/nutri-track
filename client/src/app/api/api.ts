import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from 'react-toastify';
import { PaginatedResponse } from '../models/pagination.ts';
import {store} from "../store/store.ts";
import {RegisterFormData} from "../models/registerFormData.ts";
import LoginFormData from "../models/loginFormData.ts";
import {ProfileFormData} from "../models/profileHelpers.ts";
import ProductRecordForm from "../models/dailyRecord.ts";
import ProductRecordNew from "../models/productNutrition.ts";

axios.defaults.baseURL = import.meta.env.VITE_API_URL;
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response.data;

axios.interceptors.request.use(config => {
    const token = store.getState().account.user?.token;
    if (token) config.headers.Authorization = `Bearer ${token}`
    return config;
})

axios.interceptors.response.use(async response => {
    const pagination = response.headers['pagination'];
    if (pagination) {
        response.data = new PaginatedResponse(response.data, JSON.parse(pagination));
        return response;
    }
    return response
}, (error: AxiosError) => {
    const {data, status} = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if (data.errors) {
                const modelStateErrors: string[] = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modelStateErrors.push(data.errors[key])
                    }
                }
                throw modelStateErrors.flat();
            }
            toast.error(data.title);
            break;
        case 401:
            toast.error("Невдалий вхід. Будь ласка, переіерте введені дані.");
            break;
        case 403:
            toast.error('Заборонена дія!');
            break;
        default:
            break;
    }
    return Promise.reject(error.response);
})

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, {params}).then(responseBody),
    put: (url: string, body: object) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
    post: (url: string, body: object) => axios.post(url, body).then(responseBody)
}

const Account = {
    login: (values: LoginFormData) => requests.post('account/login',values),
    register: (values: RegisterFormData) => requests.post('account/register',values),
    currentUser: () => requests.get('account/currentUser'),
    getUserProfile: () => requests.get('account/profile'),
    updateUserProfile: (values: ProfileFormData) => requests.put('account/profile', values),
    getActivityLevels: () => requests.get('account/activityLevels'),
    getGoalTypes: () => requests.get('account/goalTypes'),
    addNewWeightRecord: (values: any) => requests.post('account/addWeightRecord', values)
}

const ProductNutrition = {
    list: (params: URLSearchParams) => requests.get('productNutrition', params),
    getProductById: (id: number) => requests.get(`productNutrition/${id}`),
    fetchCategories: () => requests.get('productNutrition/categories'),
}

const Diary = {
    addProductRecord: (value: ProductRecordNew) => requests.post('diary/addNewProductRecord',value),
    getRecordByDate: (date: string) => requests.get(`diary/getRecordByDate/${date}`),
    updateProductRecord: (value: ProductRecordForm) => requests.put('diary/updateProductRecord', value),
    deleteProductRecord: (id: number) => requests.delete(`diary/deleteProductRecord/${id}`),
    getStatisticsByPeriod: (period: string) => requests.get(`diary/getStatisticsByPeriod/${period}`),
}

const api = {
    Account,
    ProductNutrition,
    Diary
}

export default api;