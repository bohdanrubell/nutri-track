import { AppBar, Box, List, ListItem, Toolbar, Typography } from "@mui/material";
import { NavLink } from 'react-router-dom';
import { useAppSelector } from '../store/store.ts';
import UserMenu from "./UserMenu.tsx";

const midLinks = [
    { title: 'Продукти', path: '/productNutrition' },
    { title: 'Про нас', path: '/about' },
]

const rightLinks = [
    { title: 'Авторизація', path: '/login' },
    { title: 'Регістрація', path: '/register' },
]

const navLinkStyles = {
    color: 'inherit',
    textDecoration: 'none',
    typography: 'h6',
    '&:hover': {
        color: 'grey.500'
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
                <Box display='flex' alignItems='center'>
                    <Typography
                        variant='h6'
                        component={NavLink}
                        to='/'
                        sx={navLinkStyles}
                    >
                        NutriTrack
                    </Typography>
                </Box>

                <List sx={{ display: 'flex' }}>
                    {midLinks.map(({ title, path }) => (
                        <ListItem
                            component={NavLink}
                            to={path}
                            key={path}
                            sx={navLinkStyles}
                        >
                            {title.toUpperCase()}
                        </ListItem>
                    ))}
                    {user  &&
                        <ListItem
                            component={NavLink}
                            to={'/diary'}
                            sx={navLinkStyles}
                        >
                            Щоденник
                        </ListItem>
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