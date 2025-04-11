import { Container, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import Header from './Header';
import {useCallback, useEffect, useState} from 'react';
import {Outlet, useLocation} from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
import LoadingComponent from '../components/LoadingComponent.tsx';
import { useAppDispatch } from '../store/store.ts';
import {fetchCurrentUser} from "../../features/account/accountSlice.tsx";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import HomePage from "../../features/home/HomePage.tsx";

function App() {
    const locate = useLocation();
    const dispatch = useAppDispatch();
    const [loading, setLoading] = useState(true);

    const initApp = useCallback(async () => {
        try {
            await dispatch(fetchCurrentUser());
        } catch (error) {
            console.log(error);
        }
    }, [dispatch]);

    useEffect(() => {
        initApp().then(() => setLoading(false));
    }, [initApp]);

    const theme = createTheme({
        palette: {
            primary: { main: "#2e7d32" },
            secondary: { main: "#81c784" }
        },
    });

    if (loading) return <LoadingComponent message='Запускаємо застосунок...' />;

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <ThemeProvider theme={theme}>
                <CssBaseline />

                {locate.pathname === '/' ? (
                    <HomePage />
                ) : (
                    <>
                        <Header />
                        <Container>
                            <Outlet />
                        </Container>
                    </>
                )}
            </ThemeProvider>
        </LocalizationProvider>
    );
}

export default App
