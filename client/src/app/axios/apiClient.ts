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
    return response;
}, (error: AxiosError) => {

    const {data, status} = error.response!;

    interface ErrorData {
        title?: string;
        errors?: string[] | Record<string, string[]>;
        type?: string;
        status?: number;
        detail?: string;
    }

    const errorData = data as ErrorData;

    switch (status) {
        case 400:
            if (errorData.errors && Array.isArray(errorData.errors)) {
                errorData.errors.forEach(error => {
                    toast.error(error);
                });
            }
            else if (errorData.errors && typeof errorData.errors === 'object') {
                const modelStateErrors: string[] = [];
                for (const key in errorData.errors) {
                    if (errorData.errors[key]) {
                        const keyErrors = errorData.errors[key];
                        if (Array.isArray(keyErrors)) {
                            modelStateErrors.push(keyErrors.join(', '));
                        }
                    }
                }
                modelStateErrors.forEach(error => {
                    toast.error(error);
                });
            }
            else {
                toast.error(errorData.title || 'Помилка запиту');
            }
            break;
        case 401:
            toast.error(errorData.title || "Потрібна авторизація. Будь ласка, увійдіть.");
            break;
        case 403:
            toast.error(errorData.title || 'Доступ заборонено!');
            break;
        case 404:
            if (!error.config?.url?.includes('diary/getRecordByDate')) {
                toast.error(errorData.title || 'Ресурс не знайдено');
            }
            break;
        case 500:
            toast.error(errorData.title || 'Серверна помилка');
            break;
        default:
            toast.error(errorData.title || 'Щось пішло не так');
            break;
    }

    return Promise.reject(error.response);
});

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, {params}).then(responseBody),
    put: (url: string, body: object) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
    post: (url: string, body: object) =>
        axios.post(url, body instanceof FormData ? body : body, {
            headers: body instanceof FormData ? {'Content-Type': 'multipart/form-data'} : undefined
        }).then(responseBody),
}

const Account = {
    login: (values: LoginFormData) => requests.post('account/login',values),
    register: (values: RegisterFormData) => requests.post('account/register',values),
    currentUser: () => requests.get('account/currentUser'),
    getUserProfile: () => requests.get('account/profile'),
    updateUserProfile: (values: ProfileFormData) => requests.put('account/profile', values),
    getActivityLevels: () => requests.get('account/activityLevels'),
    getGoalTypes: () => requests.get('account/goalTypes'),
    addNewWeightRecord: (values: any) => requests.post('account/addWeightRecord', values),
    forgotPassword: (email: string) => requests.post('account/forgot-password', { email }),
    resetPassword: (email: string, token: string, newPassword: string) => 
        requests.post('account/reset-password', { 
            email,
            token,
            newPassword
        }),
    checkAvailability: (email: string, username: string) =>
        requests.get('account/check-availability', new URLSearchParams({ email, username }))
}

const ProductNutrition = {
    list: (params: URLSearchParams) => requests.get('productNutrition', params),
    getProductById: (id: number) => requests.get(`productNutrition/${id}`),
    fetchCategories: () => requests.get('productNutrition/categories'),
    listAll: () => requests.get('productNutrition/all'),
}

const Diary = {
    addProductRecord: (value: ProductRecordNew) => requests.post('diary/addNewProductRecord',value),
    getRecordByDate: (date: string) => requests.get(`diary/getRecordByDate/${date}`),
    updateProductRecord: (value: ProductRecordForm) => requests.put('diary/updateProductRecord', value),
    deleteProductRecord: (id: number) => requests.delete(`diary/deleteProductRecord/${id}`),
    getStatisticsByPeriod: (period: string) => requests.get(`diary/getStatisticsByPeriod/${period}`),
}

const Admin = {
    createNewProductNutrition: (value: any) => requests.post('productNutrition/create', value),
    deleteProductNutrition: (id: number) => requests.delete(`productNutrition/${id}`),
    updateProductNutrition: (value: any) => requests.put(`productNutrition/update`, value),
    addProductCategory: (value: any) => requests.post('productNutrition/category/add', value),
    deleteProductCategory: (id: number) => requests.delete(`productNutrition/category/${id}`)
}

const apiClient = {
    Account,
    ProductNutrition,
    Diary,
    Admin
}

export default apiClient;