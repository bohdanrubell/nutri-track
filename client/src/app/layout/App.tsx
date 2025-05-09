import { Container, CssBaseline, ThemeProvider, createTheme, GlobalStyles } from '@mui/material';
import Header from './Header';
import { useCallback, useEffect, useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
import LoadingComponent from '../components/LoadingComponent.tsx';
import { useAppDispatch } from '../store/store.ts';
import { fetchCurrentUser } from "../../features/account/accountSlice.tsx";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import HomePage from "../../features/home/HomePage.tsx";

function App() {
    const locate = useLocation();
    const dispatch = useAppDispatch();
    const [loading, setLoading] = useState(true);
    const [darkMode, setDarkMode] = useState<boolean>(() => {
        const storedMode = localStorage.getItem('themeMode');
        return storedMode ? storedMode === 'dark' : false;
    });

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

    const toggleTheme = () => {
        setDarkMode(prev => {
            const newMode = !prev;
            localStorage.setItem('themeMode', newMode ? 'dark' : 'light');
            return newMode;
        });
    };

    const lightTheme = createTheme({
        palette: {
            mode: 'light',
            primary: { main: "#2e7d32" },
            secondary: { main: "#81c784" },
            background: {
                default: '#f5f5f5'
            }
        }
    });

    const darkTheme = createTheme({
        palette: {
            mode: 'dark',
            primary: { main: "#81c784" },
            secondary: { main: "#2e7d32" },
            background: {
                default: '#121212'
            }
        }
    });

    if (loading) return <LoadingComponent message="Запускаємо застосунок..." />;

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="uk">
            <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
                <CssBaseline />

                <GlobalStyles styles={{
                    body: {
                        margin: 0,
                        padding: 0,
                        minHeight: '100vh',
                        background: darkMode
                            ? 'linear-gradient(135deg, #263238 0%, #1b5e20 100%)'
                            : 'linear-gradient(135deg, #e0f7fa 0%, #c8e6c9 100%)',
                        backgroundAttachment: 'fixed',
                        backgroundSize: 'cover',
                        fontFamily: 'Roboto, sans-serif',
                    }
                }} />

                {locate.pathname === '/' ? (
                    <HomePage />
                ) : (
                    <>
                        <Header toggleTheme={toggleTheme} darkMode={darkMode} />
                        <Container>
                            <Outlet />
                        </Container>
                    </>
                )}
            </ThemeProvider>
        </LocalizationProvider>
    );
}

export default App;
