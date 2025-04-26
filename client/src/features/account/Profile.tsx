import {useEffect, useState} from 'react';
import {
    Box,
    Button,
    Card,
    CardContent,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    MenuItem,
    Stack,
    TextField,
    Typography
} from '@mui/material';
import Grid from "@mui/material/Grid2"
import {axisClasses, LineChart, lineElementClasses, LinePlot} from '@mui/x-charts';
import apiClient from "../../app/axios/apiClient.ts";
import {ActivityLevel, GoalType, ProfileFormData, UserCharacteristics} from "../../app/models/profileHelpers.ts";
import {toast} from "react-toastify";
import LoadingComponent from "../../app/components/LoadingComponent.tsx";

interface WeightRecord {
    weight: number;
    date: string;
}

export default function Profile() {
    const [goals, setGoals] = useState<GoalType[]>([]);
    const [activities, setActivities] = useState<ActivityLevel[]>([]);
    const [userData, setUserData] = useState<UserCharacteristics | null>(null);
    const [openWeightDialog, setOpenWeightDialog] = useState(false);
    const [openEditDialog, setOpenEditDialog] = useState(false);
    const [newWeight, setNewWeight] = useState<WeightRecord>({weight: 0, date: ''});
    const [dataUpdated, setDataUpdated] = useState(false);
    const [editedData, setEditedData] = useState<ProfileFormData>({
        gender: '',
        height: 0,
        currentActivityLevel: '',
        currentGoalType: ''
    });

    useEffect(() => {
        apiClient.Account.getUserProfile()
            .then(setUserData)
            .catch(error => console.log(error));
        apiClient.Account.getGoalTypes().then(setGoals).catch(error => console.log(error));
        apiClient.Account.getActivityLevels().then(setActivities).catch(error => console.log(error));
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

    const handleAddWeightClick = () => setOpenWeightDialog(true);
    const handleEditProfileClick = () => setOpenEditDialog(true);
    const handleCloseWeightDialog = () => setOpenWeightDialog(false);
    const handleCloseEditDialog = () => setOpenEditDialog(false);

    const handleSaveWeight = () => {
        if (!isNaN(newWeight.weight) && newWeight.weight > 0) {
            apiClient.Account.addNewWeightRecord({weight: newWeight.weight})
                .then(() => {
                    toast.success("Успішно додано новий запис ваги!");
                    setDataUpdated(prev => !prev);
                })
                .catch(error => console.log(error));
        } else {
            toast.error("Введіть коректне число для ваги.");
        }
        setOpenWeightDialog(false);
        setNewWeight({weight: 0, date: ''});
    };

    const handleSaveProfile = () => {
        apiClient.Account.updateUserProfile(editedData)
            .then(() => {
                toast.success("Характеристики користувача успішно оновлено!");
                setDataUpdated(prev => !prev);
            })
            .catch(error => console.log(error));
        setOpenEditDialog(false);
    };

    if (!userData) {
        return <LoadingComponent message="Завантаження профілю..." />;
    }

    const translatedGender =
        userData.gender === "Male" ? "Чоловік" :
            userData.gender === "Female" ? "Жінка" :
                "Невідомо";

    const latestWeight = userData.weightRecords.length > 0
        ? `${userData.weightRecords[0].weight} кг — ${userData.weightRecords[0].date}`
        : 'Немає записів';

    const weightData = [...userData.weightRecords].reverse();

    return (
        <Box sx={{maxWidth: 1200, mx: 'auto', my: 4, px: 2}}>
            <Grid container spacing={4}>
                <Grid size={{xs: 12, md: 6}}>
                    <Card sx={{height: '100%', boxShadow: 3, borderRadius: 3}}>
                        <CardContent>
                            <Typography variant="h6" gutterBottom>Характеристики користувача</Typography>
                            <Stack spacing={1} mb={3}>
                                <Typography><strong>Стать:</strong> {translatedGender}</Typography>
                                <Typography><strong>Дата
                                    народження:</strong> {userData.dateOfBirth} ({userData.age} років)</Typography>
                                <Typography><strong>Зріст:</strong> {userData.height} см</Typography>
                                <Typography><strong>Ціль:</strong> {userData.currentGoalType}</Typography>
                                <Typography><strong>Активність:</strong> {userData.currentActivityLevel}</Typography>
                                <Button variant="contained" onClick={handleEditProfileClick}>Змінити
                                    характеристики</Button>
                            </Stack>

                            <Divider sx={{mb: 3}}/>

                            <Typography variant="h6" gutterBottom>Добова норма КБЖВ</Typography>
                            <Stack spacing={1}>
                                <Typography>Калорії: {userData.dailyNutritions.dailyCalories}</Typography>
                                <Typography>Білки: {userData.dailyNutritions.dailyProtein} г</Typography>
                                <Typography>Жири: {userData.dailyNutritions.dailyFat} г</Typography>
                                <Typography>Вуглеводи: {userData.dailyNutritions.dailyCarbohydrates} г</Typography>
                            </Stack>
                        </CardContent>
                    </Card>
                </Grid>
                <Grid size={{xs: 12, md: 6}}>
                    <Card sx={{height: '100%', boxShadow: 3, borderRadius: 3}}>
                        <CardContent>
                            <Typography variant="h6" gutterBottom>Аналітика ваги</Typography>
                            <Typography mb={1}><strong>Останній запис:</strong> {latestWeight}</Typography>
                            <Divider sx={{my: 2}}/>

                            <Typography variant="h6" gutterBottom>Графік змін ваги</Typography>
                            <Box sx={{height: 250}}>
                                {weightData.length > 0 ? (
                                    <LineChart
                                        xAxis={[{scaleType: 'point', data: weightData.map(w => w.date)}]}
                                        series={[{data: weightData.map(w => w.weight), label: 'Вага (кг)', type: 'line'}]}
                                        height={250}
                                        sx={{
                                            [`& .${axisClasses.left} .MuiTypography-root`]: {fontSize: 12},
                                            [`& .${axisClasses.bottom} .MuiTypography-root`]: {fontSize: 12},
                                            [`& .${lineElementClasses.root}`]: {
                                                strokeWidth: 2,
                                            },
                                        }}
                                    >
                                        <LinePlot/>
                                    </LineChart>
                                    ) : (
                                    <Typography color="textSecondary">Немає даних для побудови графіка</Typography>
                                    )}
                            </Box>

                            <Button variant="contained" color="secondary" sx={{mt: 2}} onClick={handleAddWeightClick}>
                                Додати новий запис ваги
                            </Button>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Dialog open={openWeightDialog} onClose={handleCloseWeightDialog}>
                <DialogTitle>Додати новий запис ваги</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Вага (кг)"
                        type="number"
                        fullWidth
                        value={newWeight.weight}
                        onChange={(e) => setNewWeight({weight: parseFloat(e.target.value), date: ''})}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseWeightDialog}>Закрити</Button>
                    <Button onClick={handleSaveWeight} variant="contained">Зберегти</Button>
                </DialogActions>
            </Dialog>
            <Dialog open={openEditDialog} onClose={handleCloseEditDialog}>
                <DialogTitle>Редагувати характеристики</DialogTitle>
                <DialogContent>
                    <TextField
                        select
                        label="Стать"
                        fullWidth
                        value={editedData.gender}
                        onChange={(e) => setEditedData({...editedData, gender: e.target.value})}
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
                        onChange={(e) => setEditedData({...editedData, height: parseFloat(e.target.value)})}
                        margin="dense"
                    />

                    <TextField
                        select
                        label="Ціль"
                        fullWidth
                        value={editedData.currentGoalType}
                        onChange={(e) => setEditedData({...editedData, currentGoalType: e.target.value})}
                        margin="dense"
                    >
                        {goals.map(goal => (
                            <MenuItem key={goal.id} value={goal.name}>{goal.name}</MenuItem>
                        ))}
                    </TextField>

                    <TextField
                        select
                        label="Активність"
                        fullWidth
                        value={editedData.currentActivityLevel}
                        onChange={(e) => setEditedData({...editedData, currentActivityLevel: e.target.value})}
                        margin="dense"
                    >
                        {activities.map(activity => (
                            <MenuItem key={activity.id} value={activity.name}>{activity.name}</MenuItem>
                        ))}
                    </TextField>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseEditDialog}>Закрити</Button>
                    <Button onClick={handleSaveProfile} variant="contained">Зберегти</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}
