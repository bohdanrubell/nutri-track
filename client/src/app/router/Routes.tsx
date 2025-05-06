import {createBrowserRouter, Navigate} from 'react-router-dom';
import App from '../layout/App';
import Login from "../../features/account/Login.tsx";
import Register from "../../features/account/Register.tsx";
import Profile from "../../features/account/Profile.tsx";
import ForgotPassword from "../../features/account/ForgotPassword.tsx";
import ResetPassword from "../../features/account/ResetPassword.tsx";
import ProductNutrition from "../../features/productNutrition/ProductNutrition.tsx";
import Diary from "../../features/diary/Diary.tsx";
import ProductNutritionDetails from "../../features/productNutrition/ProductNutritionDetails.tsx";
import PeriodStatisticsView from "../../features/statistics/PeriodStatisticsView.tsx";
import NotFoundComponent from "../components/NotFoundComponent.tsx";
import AuthOnly from "./AuthOnly.tsx";

export const router = createBrowserRouter(([
    {
        path: '/',
        element: <App/>,
        children: [
            {
                element: <AuthOnly roles={['User']}/>, children: [
                    {path: '/statistics', element: <PeriodStatisticsView/>},
                    {path: '/profile', element: < Profile/>},
                    {path: '/diary', element: < Diary/>}
                ]
            },
            {path: '/login', element: <Login/>},
            {path: '/forgot-password', element: <ForgotPassword/>},
            {path: '/reset-password', element: <ResetPassword/>},
            {path: '/productNutrition', element: <ProductNutrition/>},
            {path: '/productNutrition/:id', element: <ProductNutritionDetails/>},
            {path: '/register', element: <Register/>},
            { path: '/not-found', element: <NotFoundComponent /> },
            {path: '*', element: <Navigate replace to='/not-found'/>}
        ]
    }
]))