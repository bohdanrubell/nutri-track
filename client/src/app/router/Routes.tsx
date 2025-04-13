import {createBrowserRouter, Navigate} from 'react-router-dom';
import App from '../layout/App';
import Login from "../../features/account/Login.tsx";
import Register from "../../features/account/Register.tsx";
import Profile from "../../features/account/Profile.tsx";
import ProductNutrition from "../../features/productNutrition/ProductNutrition.tsx";
import Diary from "../../features/diary/Diary.tsx";
import ProductNutritionDetails from "../../features/productNutrition/ProductNutritionDetails.tsx";
import PeriodStatisticsView from "../../features/statistics/PeriodStatisticsView.tsx";

export const router = createBrowserRouter(([
    {
        path: '/',
        element: <App/>,
        children: [
            {path: '/login', element: <Login/>},
            {path: '/productNutrition', element: <ProductNutrition/>},
            {path: '/productNutrition/:id', element: <ProductNutritionDetails/>},
            {path: '/statistics', element: <PeriodStatisticsView/>},
            {path: '/register', element: <Register/>},
            {path: '/profile', element: < Profile/>},
            {path: '/diary', element: < Diary/>},
            {path: '*', element: <Navigate replace to='/not-found'/>}
        ]
    }
]))