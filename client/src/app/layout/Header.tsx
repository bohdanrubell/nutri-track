import { AppBar, Box, List, ListItem, Toolbar, Typography } from "@mui/material";
import { NavLink } from 'react-router-dom';
import { useAppSelector } from '../store/store.ts';
import UserMenu from "./UserMenu.tsx";
import AnalyticsIcon from '@mui/icons-material/Analytics';
import MenuBookIcon from '@mui/icons-material/MenuBook';
import StorageIcon from '@mui/icons-material/Storage';

const rightLinks = [
    { title: 'Авторизація', path: '/login' },
    { title: 'Регістрація', path: '/register' },
]

const navLinkStyles = {
    color: 'inherit',
    textDecoration: 'none',
    whiteSpace: 'nowrap',
    typography: 'h6',
    '&:hover': {
        color: 'grey.400'
    },
    '&.active': {
        color: 'text.secondary'
    }
}

export default function Header() {
    const {user} = useAppSelector(state => state.account);

    return (
        <AppBar position='static' sx={{ mb: 4 }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Box display='flex' alignItems='center' gap={1}>
                    <img
                        src='/images/icon.png'
                        alt='logo'
                        style={{width: 32, height: 32}}
                    />
                    <Typography
                        variant='h6'
                        component={NavLink}
                        to='/'
                        sx={navLinkStyles}
                    >
                        NutriTrack
                    </Typography>
                </Box>

                <List sx={{display: 'flex'}}>
                    <ListItem
                        component={NavLink}
                        to={'/productNutrition'}
                        sx={navLinkStyles}
                    >
                        <StorageIcon/>
                        База продуктів
                    </ListItem>
                    {user && user!.roles!.includes('User') &&
                        <>
                            <ListItem
                                component={NavLink}
                                to={'/diary'}
                                sx={navLinkStyles}
                            >
                                <MenuBookIcon/>
                                Щоденник
                            </ListItem>
                            <ListItem
                                component={NavLink}
                                to={'/statistics'}
                                sx={navLinkStyles}
                            >
                                <AnalyticsIcon/>
                                Відомість
                            </ListItem>
                        </>
                    }
                </List>

                <Box display='flex' alignItems='center'>
                    {user ? (
                        <UserMenu />
                    ) : (
                        <List sx={{ display: 'flex' }}>
                            {rightLinks.map(({ title, path }) => (
                                <ListItem
                                    component={NavLink}
                                    to={path}
                                    key={path}
                                    sx={navLinkStyles}
                                >
                                    {title.toUpperCase()}
                                </ListItem>
                            ))}
                        </List>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    )
}