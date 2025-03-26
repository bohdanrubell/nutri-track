import { useAppDispatch, useAppSelector } from "../store/store";
import { useState, MouseEvent } from "react";
import { Avatar, Button, Menu, MenuItem } from "@mui/material";
import { Link } from "react-router-dom";
import { signOut } from "../../features/account/accountSlice";

export default function UserMenu() {
    const dispatch = useAppDispatch();
    const { user } = useAppSelector((state) => state.account);
    const [anchor, setAnchor] = useState<null | HTMLElement>(null);
    const open = Boolean(anchor);

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
                sx={{ typography: "h6", textTransform: 'none', display: 'flex', alignItems: 'center', gap: 1 }}
                aria-controls={open ? "user-menu" : undefined}
                aria-haspopup="true"
                aria-expanded={open ? "true" : undefined}
            >
                <Avatar sx={{ width: 32, height: 32 }}>
                    {user?.username?.charAt(0).toUpperCase() || "U"}
                </Avatar>
                {user?.username}
            </Button>
            <Menu
                id="user-menu"
                anchorEl={anchor}
                open={open}
                onClose={handleClose}
            >
                <MenuItem component={Link} to="/profile" onClick={handleClose}>
                    Профіль користувача
                </MenuItem>
                <MenuItem onClick={handleLogout}>Вийти з аккаунту</MenuItem>
            </Menu>
        </>
    );
}
