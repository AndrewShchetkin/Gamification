import { AppBar, bottomNavigationActionClasses, Divider, Drawer, IconButton, List, ListItem, ListItemText, Menu, MenuItem, Toolbar, Typography } from "@mui/material";
import { Box } from "@mui/system";
import MenuIcon from '@mui/icons-material/Menu';
import HomeIcon from '@mui/icons-material/Home';
import QuizIcon from '@mui/icons-material/Quiz';
import SettingsIcon from '@mui/icons-material/Settings';
import LogoutIcon from '@mui/icons-material/Logout';
import HelpCenterIcon from '@mui/icons-material/HelpCenter';
import React from "react";
import { useState } from "react";
import { Link } from "react-router-dom";


export default function Navigation() {
    const [openDrawer, setOpenDrawer] = useState(false);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null); // для привязки меню к кнопке


    // Привязка меню настроек к текущему элементу 
    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    // Отвязка меню настроек от элемента 
    const handleClose = () => {
        setAnchorEl(null);
    };

    //Отображение навигационного меню сбоку
    const toggleDrawer = (isOpen: boolean) => {
        setOpenDrawer(isOpen)
    }

    return (
        <Box sx={{ flexGrow: 1,
                    marginBottom: '10px'
                }}>
            <AppBar position="static">
                <Toolbar>
                    <IconButton
                        size="large"
                        edge="start"
                        color="inherit"
                        aria-label="menu"
                        sx={{ mr: 2 }}
                        onClick={() => toggleDrawer(true)}>
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        The Game
                    </Typography>
                    {
                        // Пробуем меню
                    }
                    <div>
                        <IconButton
                            size="large"
                            color="inherit"
                            aria-controls="menu-appbar"
                            onClick={handleMenu}
                        >
                            <SettingsIcon />
                        </IconButton>
                        <Menu
                            id="menu-appbar"
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={handleClose}
                        >
                            <MenuItem component={Link} to="/help" onClick={handleClose} >
                                <HelpCenterIcon sx={{ mr: 2 }} />
                                Справка
                            </MenuItem>
                            <Divider />
                            <MenuItem  onClick={handleClose} >
                                <LogoutIcon sx={{ mr: 2 }} />
                                Выйти
                            </MenuItem>
                        </Menu>
                    </div>
                </Toolbar>
            </AppBar>
            { 
        //Не закрывается при нажатии на лист
    }
            <Drawer
    
                anchor="left"
                open={openDrawer}
                onClose={() => toggleDrawer(false)}>
                <List>
                    <ListItem component={Link} to="/" onClick={() => toggleDrawer(false)} >
                        <HomeIcon />
                        <ListItemText primary="Home" />
                    </ListItem>
                    <ListItem component={Link} to="/quiz" onClick={() => toggleDrawer(false)} >
                        <QuizIcon />
                        <ListItemText primary="Quiz" />
                    </ListItem>
                    <ListItem component={Link} to="/lobby" onClick={() => toggleDrawer(false)} >
                        <QuizIcon />
                        <ListItemText primary="Lobby" />
                    </ListItem>
                    <ListItem component={Link} to="/adminpanel" onClick={() => toggleDrawer(false)} >
                        <QuizIcon />
                        <ListItemText primary="Admin Panel" />
                    </ListItem>
                </List>
            </Drawer>
        </Box>
    )
}