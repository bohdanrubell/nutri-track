import { Navigate, Outlet, useLocation } from "react-router-dom";
import { toast } from 'react-toastify';
import { useAppSelector } from "../store/store";
import { useEffect } from 'react';

interface Props {
    roles: string[];
}

export default function AuthOnly({ roles }: Props) {
    const { user } = useAppSelector(state => state.account);
    const location = useLocation();

    useEffect(() => {
        if (!user) {
            toast.warning('Ви повинні бути авторизованим!');
        } else if (roles && user.roles && !roles.some(r => user.roles!.includes(r))) {
            toast.error('У вас немає доступу до цієї сторінки!');
        }
    }, [location.pathname]);

    if (!user) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    if (roles && user.roles && !roles.some(r => user.roles!.includes(r))) {
        return <Navigate to="/not-found" replace />;
    }

    return <Outlet />;
}
