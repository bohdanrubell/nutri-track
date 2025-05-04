import {Navigate, Outlet, useLocation, useNavigate} from "react-router-dom";
import { toast } from 'react-toastify';
import { useAppSelector } from "../store/store.ts";
import { useEffect, useRef } from 'react';

interface Props {
    roles?: string[];
}

export default function AuthOnly({roles}: Props) {
    const {user} = useAppSelector(state => state.account);
    const location = useLocation();
    const isInitialMount = useRef(true);
    const navigate = useNavigate();
    
    // Only show unauthorized toast on actual navigation attempts, not on initial mount or logout
    useEffect(() => {
        if (isInitialMount.current) {
            isInitialMount.current = false;
            return;
        }
        
        if (!user) {
            toast.warning('Ви повинні бути авторизованим!');
        } else if (roles && !roles?.some(r => user.roles?.includes(r))) {
            toast.error('Заборонена дія!');
            navigate(-1);
            return;
        }
    }, [location.pathname]);
    
    if (!user) {
        return <Navigate to='/login' state={{from: location}} />;
    }


    if (roles && !roles?.some(r => user.roles?.includes(r))) {
        toast.error('Заборонена дія!');
        navigate(-1);
        return null;
    }

    return <Outlet />;
}