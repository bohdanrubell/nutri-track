import { useEffect, useState } from 'react';
import { Button, Card, CardContent, Typography, Dialog, DialogActions, DialogContent, DialogTitle, TextField, MenuItem } from '@mui/material';
import api from "../../app/api/api.ts";
import {ActivityLevel, GoalType, ProfileFormData, UserCharacteristics} from "../../app/models/profileHelpers.ts";
import { toast } from "react-toastify";

interface WeightRecord {
    weight: number;
}

export default function Profile() {
    const [goals, setGoals] = useState<GoalType[]>([]);
    const [activities, setActivities] = useState<ActivityLevel[]>([]);
    const [userData, setUserData] = useState<UserCharacteristics | null>(null);
    const [openWeightDialog, setOpenWeightDialog] = useState(false);
    const [openEditDialog, setOpenEditDialog] = useState(false);
    const [newWeight, setNewWeight] = useState<WeightRecord>({ weight: 0 });
    const [dataUpdated, setDataUpdated] = useState(false);
    const [editedData, setEditedData] = useState<ProfileFormData>({gender: '', height: 0, currentActivityLevel: '', currentGoalType: ''});

    useEffect(() => {
        api.Account.getUserProfile()
            .then(setUserData)
            .catch(error => console.log(error));
        api.Account.getGoalTypes().then(setGoals).catch(error => console.log(error));
        api.Account.getActivityLevels().then(setActivities).catch(error => console.log(error));
    }, [dataUpdated]);

    useEffect(() => {
        if (userData) {
            setEditedData({
                gender: userData.gender,
                height: userData.height,
                currentActivityLevel: userData.currentActivityLevel,
                currentGoalType: userData.currentGoalType
            });
        }
    }, [userData]);

    const handleAddWeightClick = () => {
        setOpenWeightDialog(true);
    };

    const handleEditProfileClick = () => {
        setOpenEditDialog(true);
    };

    const handleCloseWeightDialog = () => {
        setOpenWeightDialog(false);
    };

    const handleCloseEditDialog = () => {
        setOpenEditDialog(false);
    };

    const handleSaveWeight = () => {
        if (!isNaN(newWeight.weight) && newWeight.weight > 0) {
            api.Account.addNewWeightRecord(newWeight)
                .then(() => {
                    toast.success("Успішно додано новий запис ваги!");
                    setDataUpdated(prev => !prev);
                })
                .catch(error => console.log(error));
        } else {
            toast.error("Введіть коректне число для ваги.");
        }
        setOpenWeightDialog(false);
        setNewWeight({ weight: 0});
    };

    const handleSaveProfile = () => {
        api.Account.updateUserProfile(editedData)
            .then(() => {
                toast.success("Характеристики користувача успішно оновлено!");
                setDataUpdated(prev => !prev);
            })
            .catch(error => console.log(error));
        console.log(editedData)
        setOpenEditDialog(false);
    };

    if (!userData) {
        return <Typography>Завантаження...</Typography>;
    }

    return (
        <Card style={{ maxWidth: 600, margin: 'auto', padding: 16 }}>
            <CardContent>
                <Typography variant="h6">Поточна характеристика користувача</Typography>
                <Typography>Стать:
                    {userData.gender === "Male" ? "Чоловік" : userData.gender === "Female" ? "Жінка" : "Невідомо"}
                </Typography>
                <Typography>Дата народження: {userData.dateOfBirth} ({userData.age} років)</Typography>
                <Typography>Зріст: {userData.height} см</Typography>
                <Typography>Поточна ціль: {userData.currentGoalType}</Typography>
                <Typography>Поточна активність: {userData.currentActivityLevel}</Typography>

                <Button variant="contained" color="primary" onClick={handleEditProfileClick}>
                    Змінити
                </Button>

                <Typography variant="h6" style={{ marginTop: 20 }}>Поточне добове КБЖУ:</Typography>
                <Typography>Калорії: {userData.dailyNutritions.dailyCalories}</Typography>
                <Typography>Білки: {userData.dailyNutritions.dailyProtein}</Typography>
                <Typography>Жири: {userData.dailyNutritions.dailyFat}</Typography>
                <Typography>Вуглеводи: {userData.dailyNutritions.dailyCarbohydrates}</Typography>

                <Typography variant="h6" style={{ marginTop: 20 }}>Аналітика ваги</Typography>
                <Typography>Ваш останній запис ваги: - кг</Typography>

                <Typography>Минулі записи ваги:</Typography>
                <ul>
                    {userData.weightRecords.map((entry, index) => (
                        <li key={index}>
                            {entry.weight} кг - {entry.date}
                        </li>
                    ))}
                </ul>

                <Button variant="contained" color="secondary" onClick={handleAddWeightClick}>
                    Додати новий запис
                </Button>
            </CardContent>

            {/* Діалог для додавання ваги */}
            <Dialog open={openWeightDialog} onClose={handleCloseWeightDialog}>
                <DialogTitle>Додати новий запис ваги</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Новий вага (кг)"
                        type="number"
                        fullWidth
                        variant="outlined"
                        value={newWeight.weight}
                        onChange={(e) => setNewWeight({ weight: parseFloat(e.target.value) })}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseWeightDialog} color="primary">
                        Закрити
                    </Button>
                    <Button onClick={handleSaveWeight} color="primary">
                        Зберегти
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Діалог для редагування профілю */}
            <Dialog open={openEditDialog} onClose={handleCloseEditDialog}>
                <DialogTitle>Редагувати характеристики</DialogTitle>
                <DialogContent>
                    <TextField
                        select
                        label="Стать"
                        fullWidth
                        value={editedData.gender}
                        onChange={(e) => setEditedData({ ...editedData, gender: e.target.value })}
                        margin="dense"
                    >
                        <MenuItem value="Male">Чоловік</MenuItem>
                        <MenuItem value="Female">Жінка</MenuItem>
                    </TextField>
                    <TextField
                        label="Зріст (см)"
                        type="number"
                        fullWidth
                        value={editedData.height}
                        onChange={(e) => setEditedData({ ...editedData, height: parseFloat(e.target.value) })}
                        margin="dense"
                    />
                    <TextField
                        select
                        label="Поточна ціль"
                        fullWidth
                        value={editedData.currentGoalType}
                        onChange={(e) => setEditedData({ ...editedData, currentGoalType: e.target.value })}
                        margin="dense"
                    >
                        {goals.map(goal => (
                            <MenuItem key={goal.id} value={goal.name}>
                                {goal.name}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        select
                        label="Поточна активність"
                        fullWidth
                        value={editedData.currentActivityLevel}
                        onChange={(e) => setEditedData({ ...editedData, currentActivityLevel: e.target.value })}
                        margin="dense"
                    >
                        {activities.map(activity => (
                            <MenuItem key={activity.id} value={activity.name}>
                                {activity.name}
                            </MenuItem>
                        ))}
                    </TextField>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseEditDialog} color="primary">
                        Закрити
                    </Button>
                    <Button onClick={handleSaveProfile} color="primary">
                        Зберегти
                    </Button>
                </DialogActions>
            </Dialog>
        </Card>
    );
}
