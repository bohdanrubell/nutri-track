import { useAppDispatch, useAppSelector } from "../store/store";
import { useState, MouseEvent } from "react";
import {Avatar, Button, Menu, MenuItem, Tooltip, Typography} from "@mui/material";
import { Link } from "react-router-dom";
import { signOut } from "../../features/account/accountSlice";
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';

export default function UserMenu() {
    const dispatch = useAppDispatch();
    const { user } = useAppSelector((state) => state.account);
    const [anchor, setAnchor] = useState<null | HTMLElement>(null);
    const open = Boolean(anchor);
    const isAdmin = user?.roles?.includes('Admin');

    const handleClick = (event: MouseEvent<HTMLButtonElement>) => {
        setAnchor(event.currentTarget);
    };

    const handleClose = () => {
        setAnchor(null);
    };

    const handleLogout = () => {
        handleClose();
        dispatch(signOut());
    };

    return (
        <>
            <Button
                color="inherit"
                onClick={handleClick}
                sx={{
                    typography: "h6",
                    textTransform: 'none',
                    display: 'flex',
                    alignItems: 'center',
                    gap: 1
                }}
                aria-controls={open ? "user-menu" : undefined}
                aria-haspopup="true"
                aria-expanded={open ? "true" : undefined}
            >
                {isAdmin ? (
                    <>
                        <Typography
                            variant="h5"
                            fontWeight="bold"
                            color="error"
                        >
                            АДМІНІСТРАТОР
                        </Typography>
                        <Tooltip title="Адміністратор">
                            <AdminPanelSettingsIcon color="secondary" />
                        </Tooltip>
                    </>
                ) : (
                    <>
                        <Avatar sx={{ width: 32, height: 32 }}>
                            {user?.username?.charAt(0).toUpperCase() || "U"}
                        </Avatar>
                        {user?.username}
                    </>
                )}
            </Button>
            <Menu
                id="user-menu"
                anchorEl={anchor}
                open={open}
                onClose={handleClose}
            >
                {user!.roles!.includes('User') && <MenuItem component={Link} to="/profile" onClick={handleClose}>
                    Профіль користувача
                </MenuItem>}

                <MenuItem onClick={handleLogout}>Вийти з аккаунту</MenuItem>
            </Menu>
        </>
    );
}