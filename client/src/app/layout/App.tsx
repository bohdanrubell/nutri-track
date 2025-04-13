import {Container, CssBaseline, ThemeProvider, createTheme, GlobalStyles} from '@mui/material';
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

                <GlobalStyles styles={{
                    body: {
                        margin: 0,
                        padding: 0,
                        minHeight: '100vh',
                        background: 'linear-gradient(135deg, #e0f7fa 0%, #c8e6c9 100%)',
                        backgroundAttachment: 'fixed',
                        backgroundSize: 'cover',
                        fontFamily: 'Roboto, sans-serif',
                    }
                }} />
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
