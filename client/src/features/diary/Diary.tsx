import {useEffect, useState} from 'react';
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TextField,
    Typography
} from '@mui/material';
import Grid from '@mui/material/Grid2';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import dayjs from 'dayjs';
import {DateCalendar} from "@mui/x-date-pickers";
import ProductRecordForm, {DailyRecord} from "../../app/models/dailyRecord.ts";
import api from "../../app/api/api.ts";
import InfoBox from "./InfoBox.tsx";

export default function Diary() {
    const [selectedDate, setSelectedDate] = useState(dayjs());
    const [dailyRecord, setDailyRecord] = useState<DailyRecord | null>(null);
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    const [dialogMode, setDialogMode] = useState<'edit' | 'delete'>('edit');
    const [openDialog, setOpenDialog] = useState(false);
    const [editRecord, setEditRecord] = useState<ProductRecordForm | null>(null);

    useEffect(() => {
        api.Diary.getRecordByDate(selectedDate.format('YYYY-MM-DD'))
            .then(setDailyRecord)
            .catch(error => {
                if (error?.status === 404) {
                    setDailyRecord(null);
                } else {
                    console.log(error);
                }
            });
    }, [selectedDate, refreshTrigger]);

    const handleEditClick = (productRecordId: number, consumedGrams: number) => {
        setDialogMode('edit');
        setEditRecord({productRecordId, consumedGrams});
        setOpenDialog(true);
    };

    const handleDeleteClick = (productRecordId: number) => {
        setDialogMode('delete');
        setEditRecord({productRecordId, consumedGrams: 0});
        setOpenDialog(true);
    };

    const totals = dailyRecord?.productRecords.reduce(
        (acc, product) => {
            acc.calories += Math.round(product.calories);
            acc.protein += Math.round(product.protein);
            acc.fat += Math.round(product.fat);
            acc.carbohydrates += Math.round(product.carbohydrates);
            return acc;
        },
        {calories: 0, protein: 0, fat: 0, carbohydrates: 0}
    );

    const getColor = (value: number, norm: number): 'success' | 'error' | 'warning' => {
        if (value < norm) return 'error';
        if (value <= norm * 1.1) return 'success';
        return 'warning';
    };

    return (
        <Grid container spacing={2} p={2}>
            <Grid>
                <DateCalendar
                    value={selectedDate}
                    onChange={(newDate) => setSelectedDate(newDate)}
                />
            </Grid>
            <Grid>
                <Typography variant="h6" gutterBottom sx={{textAlign: 'center'}}>
                    Запис щоденника за {selectedDate.format('DD.MM.YYYY')}
                </Typography>
                {dailyRecord ? (
                    <>
                        <Grid container spacing={2} justifyContent="center" mb={2}>
                            <Grid>
                                <InfoBox
                                    label="Калорії"
                                    value={`${totals?.calories ?? 0}/${dailyRecord.dailyNutritions.dailyCalories ?? 0}`}
                                    color={getColor(totals?.calories ?? 0, dailyRecord.dailyNutritions.dailyCalories ?? 0)}
                                />
                            </Grid>
                            <Grid>
                                <InfoBox
                                    label="Білки"
                                    value={`${totals?.protein ?? 0}/${dailyRecord.dailyNutritions.dailyProtein ?? 0}`}
                                    color={getColor(totals?.protein ?? 0, dailyRecord.dailyNutritions.dailyProtein ?? 0)}
                                />
                            </Grid>
                            <Grid>
                                <InfoBox
                                    label="Жири"
                                    value={`${totals?.fat ?? 0}/${dailyRecord.dailyNutritions.dailyFat ?? 0}`}
                                    color={getColor(totals?.fat ?? 0, dailyRecord.dailyNutritions.dailyFat ?? 0)}
                                />
                            </Grid>
                            <Grid>
                                <InfoBox
                                    label="Вуглеводи"
                                    value={`${totals?.carbohydrates ?? 0}/${dailyRecord.dailyNutritions.dailyCarbohydrates ?? 0}`}
                                    color={getColor(totals?.carbohydrates ?? 0, dailyRecord.dailyNutritions.dailyCarbohydrates ?? 0)}
                                />
                            </Grid>
                        </Grid>
                        <Paper elevation={5}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>№</TableCell>
                                        <TableCell>Назва продукту</TableCell>
                                        <TableCell>Ккал</TableCell>
                                        <TableCell>Білки</TableCell>
                                        <TableCell>Жири</TableCell>
                                        <TableCell>Вуглеводи</TableCell>
                                        <TableCell>Кількість</TableCell>
                                        <TableCell align="center">Дії</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {dailyRecord.productRecords.map((product, index) => (
                                        <TableRow key={index}>
                                            <TableCell>{index + 1}</TableCell>
                                            <TableCell>{product.name}</TableCell>
                                            <TableCell>{product.calories}</TableCell>
                                            <TableCell>{product.protein}</TableCell>
                                            <TableCell>{product.fat}</TableCell>
                                            <TableCell>{product.carbohydrates}</TableCell>
                                            <TableCell>{product.grams}</TableCell>
                                            <TableCell align="center">
                                                <IconButton
                                                    color="primary"
                                                    onClick={() => handleEditClick(product.id, product.grams)}
                                                >
                                                    <EditIcon/>
                                                </IconButton>
                                                <IconButton
                                                    color="error"
                                                    onClick={() => handleDeleteClick(product.id)}
                                                >
                                                    <DeleteIcon/>
                                                </IconButton>
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </Paper>
                    </>
                ) : (
                    <Grid container justifyContent="center" alignItems="center" sx={{height: '60vh'}}>
                        <Typography variant="h6" sx={{color: 'text.secondary'}}>
                            На цю дату немає записів!
                        </Typography>
                    </Grid>
                )}
            </Grid>
            <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
                <DialogTitle>
                    {dialogMode === 'edit' ? 'Оновити кількість грамів' : 'Підтвердити видалення'}
                </DialogTitle>
                <DialogContent>
                    {dialogMode === 'edit' ? (
                        <TextField
                            type="number"
                            fullWidth
                            label="Грами"
                            margin="dense"
                            value={editRecord?.consumedGrams ?? ''}
                            onChange={(e) =>
                                setEditRecord(prev =>
                                    prev ? {...prev, consumedGrams: parseInt(e.target.value)} : null
                                )
                            }
                        />
                    ) : (
                        <Typography>
                            Ви впевнені, що хочете видалити цей продукт із запису?
                        </Typography>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpenDialog(false)}>Скасувати</Button>
                    <Button
                        variant="contained"
                        color={dialogMode === 'edit' ? 'primary' : 'error'}
                        onClick={async () => {
                            if (!editRecord) return;
                            if (dialogMode === 'edit') {
                                await api.Diary.updateProductRecord(editRecord);
                            } else {
                                await api.Diary.deleteProductRecord(editRecord.productRecordId);
                            }
                            setOpenDialog(false);
                            setRefreshTrigger(prev => prev + 1);
                        }}
                    >
                        {dialogMode === 'edit' ? 'Зберегти' : 'Видалити'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid>
    );
}
