import { useEffect, useState } from 'react';
import {
    Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Paper,
    Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography, Autocomplete, Tooltip
} from '@mui/material';
import Grid from '@mui/material/Grid2';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import dayjs from 'dayjs';
import 'dayjs/locale/uk';
import { DateCalendar } from "@mui/x-date-pickers";
import ProductRecordForm, { DailyRecord } from "../../app/models/dailyRecord.ts";
import apiClient from "../../app/axios/apiClient.ts";
import InfoBoxComponent from "../../app/components/InfoBoxComponent.tsx";
import { ProductNutrition } from '../../app/models/productNutrition.ts';
import { toast } from 'react-toastify';

export default function Diary() {
    dayjs.locale('uk');
    const [selectedDate, setSelectedDate] = useState(dayjs());
    const [dailyRecord, setDailyRecord] = useState<DailyRecord | null>(null);
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    const [dialogMode, setDialogMode] = useState<'edit' | 'delete'>('edit');
    const [openDialog, setOpenDialog] = useState(false);
    const [editRecord, setEditRecord] = useState<ProductRecordForm | null>(null);

    const [openAddDialog, setOpenAddDialog] = useState(false);
    const [allProducts, setAllProducts] = useState<ProductNutrition[]>([]);
    const [selectedProduct, setSelectedProduct] = useState<ProductNutrition | null>(null);
    const [grams, setGrams] = useState('100');
    const [loadingProducts, setLoadingProducts] = useState(false);
    const [addingProduct, setAddingProduct] = useState(false);

    const isFutureDate = () => {
        return selectedDate.isAfter(dayjs(), 'day');
    };

    useEffect(() => {
        apiClient.Diary.getRecordByDate(selectedDate.format('YYYY-MM-DD'))
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
        setEditRecord({ productRecordId, consumedGrams });
        setOpenDialog(true);
    };

    const handleDeleteClick = (productRecordId: number) => {
        setDialogMode('delete');
        setEditRecord({ productRecordId, consumedGrams: 0 });
        setOpenDialog(true);
    };

    const handleClose = () => {
        setOpenDialog(false);
        setGrams('100');
    };


    const totals = dailyRecord?.productRecords.reduce(
        (acc, product) => {
            acc.calories += Math.round(product.calories);
            acc.protein += Math.round(product.protein);
            acc.fat += Math.round(product.fat);
            acc.carbohydrates += Math.round(product.carbohydrates);
            return acc;
        },
        { calories: 0, protein: 0, fat: 0, carbohydrates: 0 }
    );

    const getColor = (value: number, norm: number): 'success' | 'error' | 'warning' => {
        if (value > norm * 1.15){
            return 'warning';
        }
        if (value >= norm){
           return 'success';
        }
        else {
            return 'error';
        }
    };

    const handleOpenAddProductDialog = async () => {
        if (allProducts.length === 0) {
            setLoadingProducts(true);
            try {
                const products = await apiClient.ProductNutrition.listAll();
                setAllProducts(products);
            } catch (error) {
                console.log('Помилка завантаження продуктів', error);
            } finally {
                setLoadingProducts(false);
            }
        }
        setOpenAddDialog(true);
    };

    return (
        <Grid container spacing={2} p={2}>
            <Grid>
                <DateCalendar
                    value={selectedDate}
                    onChange={(newDate) => setSelectedDate(newDate)}
                />
            </Grid>
            <Grid sx={{ minWidth: '700px', maxWidth: '770px', width: '770px' }}>
                <Typography variant="h6" gutterBottom sx={{ textAlign: 'center' }}>
                    Запис щоденника за {selectedDate.format('D MMMM YYYY')}
                </Typography>

                <Grid container justifyContent="center" mb={2}>
                <Button
                        variant="contained"
                        color="primary"
                        onClick={handleOpenAddProductDialog}
                        disabled={isFutureDate()}
                        title={isFutureDate() ? "Неможливо додати продукт на майбутню дату" : ""}
                    >
                        Додати продукт
                    </Button>
                </Grid>
                {dailyRecord ? (
                    <>
                        <Grid container spacing={2} justifyContent="center" mb={2}>
                            <Grid><InfoBoxComponent label="Калорії" value={`${totals?.calories ?? 0}/${dailyRecord.dailyNutritions.dailyCalories ?? 0}`} color={getColor(totals?.calories ?? 0, dailyRecord.dailyNutritions.dailyCalories ?? 0)} /></Grid>
                            <Grid><InfoBoxComponent label="Білки" value={`${totals?.protein ?? 0}/${dailyRecord.dailyNutritions.dailyProtein ?? 0}`} color={getColor(totals?.protein ?? 0, dailyRecord.dailyNutritions.dailyProtein ?? 0)} /></Grid>
                            <Grid><InfoBoxComponent label="Жири" value={`${totals?.fat ?? 0}/${dailyRecord.dailyNutritions.dailyFat ?? 0}`} color={getColor(totals?.fat ?? 0, dailyRecord.dailyNutritions.dailyFat ?? 0)} /></Grid>
                            <Grid><InfoBoxComponent label="Вуглеводи" value={`${totals?.carbohydrates ?? 0}/${dailyRecord.dailyNutritions.dailyCarbohydrates ?? 0}`} color={getColor(totals?.carbohydrates ?? 0, dailyRecord.dailyNutritions.dailyCarbohydrates ?? 0)} /></Grid>
                        </Grid>
                        <Paper elevation={5} sx={{
                            maxHeight: 350,
                            overflow: 'auto',
                            boxShadow: 3,
                            borderRadius: 3,
                            width: '100%' }}>
                            <Table stickyHeader>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>№</TableCell>
                                        <TableCell>Назва продукту</TableCell>
                                        <TableCell>Ккал</TableCell>
                                        <TableCell>Білки</TableCell>
                                        <TableCell>Жири</TableCell>
                                        <TableCell>Вуглеводи</TableCell>
                                        <TableCell>Кількість(г)</TableCell>
                                        <TableCell align="center">Дії</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {dailyRecord.productRecords.map((product, index) => (
                                        <TableRow key={index}>
                                            <TableCell>{index + 1}</TableCell>
                                            <TableCell sx={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap', maxWidth: 200 }}>
                                                <Tooltip title={product.name}>
                                                    <span>{product.name}</span>
                                                </Tooltip>
                                            </TableCell>
                                            <TableCell>{product.calories}</TableCell>
                                            <TableCell>{product.protein}</TableCell>
                                            <TableCell>{product.fat}</TableCell>
                                            <TableCell>{product.carbohydrates}</TableCell>
                                            <TableCell>{product.grams}</TableCell>
                                            <TableCell align="center">
                                                <IconButton color="primary" onClick={() => handleEditClick(product.id, product.grams)}><EditIcon /></IconButton>
                                                <IconButton color="error" onClick={() => handleDeleteClick(product.id)}><DeleteIcon /></IconButton>
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </Paper>
                    </>
                ) : (
                    <Grid container justifyContent="center" alignItems="center" sx={{ height: '50vh', textAlign: 'center' }}>
                        <Typography variant="h6" sx={{ color: 'text.secondary' }}>
                            {isFutureDate() ? (
                                <>
                                    Це майбутня дата!<br />
                                    Неможливо додавати записи на майбутні дати.
                                </>
                            ) : (
                                <>
                                    На цю дату немає записів!<br />
                                    Додайте продукт, щоб створити запис.
                                </>
                            )}
                        </Typography>
                    </Grid>
                )}
            </Grid>

            <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
                <DialogTitle>{dialogMode === 'edit' ? 'Оновити кількість грамів' : 'Підтвердити видалення'}</DialogTitle>
                <DialogContent>
                    {dialogMode === 'edit' ? (
                        <TextField
                            type="number"
                            fullWidth
                            label="Грами"
                            margin="dense"
                            value={editRecord?.consumedGrams ?? ''}
                            onChange={(e) => {
                                const value = e.target.value;
                                if (value === '' || (/^\d+$/.test(value))) {
                                    setEditRecord(prev => prev ? { ...prev, consumedGrams: parseInt(value) } : null);
                                }
                            }}
                            error={!editRecord?.consumedGrams || editRecord.consumedGrams < 1 || editRecord.consumedGrams > 2000}
                            helperText={
                                !editRecord?.consumedGrams
                                    ? 'Поле не може бути порожнім'
                                    : editRecord.consumedGrams < 1
                                        ? 'Грами мають бути більше або дорівнювати 1'
                                        : editRecord.consumedGrams > 2000
                                            ? 'Максимальне значення - 2000 грамів'
                                            : ''
                            }
                        />
                    ) : (
                        <Typography>Ви впевнені, що хочете видалити цей продукт із запису?</Typography>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpenDialog(false)}>Скасувати</Button>
                    <Button
                        variant="contained"
                        color={dialogMode === 'edit' ? 'primary' : 'error'}
                        onClick={async () => {
                            if (!editRecord) return;
                            if (dialogMode === 'edit' && editRecord.consumedGrams >= 1 && editRecord.consumedGrams <= 2000) {
                                await apiClient.Diary.updateProductRecord(editRecord);
                                setOpenDialog(false);
                                setRefreshTrigger(prev => prev + 1);
                            } else if (dialogMode === 'delete') {
                                await apiClient.Diary.deleteProductRecord(editRecord.productRecordId);
                                setOpenDialog(false);
                                setRefreshTrigger(prev => prev + 1);
                            }
                        }}
                        disabled={dialogMode === 'edit' && (!editRecord?.consumedGrams || editRecord.consumedGrams < 1 || editRecord.consumedGrams > 2000)}
                    >
                        {dialogMode === 'edit' ? 'Зберегти' : 'Видалити'}
                    </Button>
                </DialogActions>
            </Dialog>
            <Dialog open={openAddDialog} onClose={handleClose} maxWidth="sm" fullWidth>
                <DialogTitle>Додати продукт до щоденника</DialogTitle>
                <DialogContent>
                    <Autocomplete
                        options={allProducts}
                        getOptionLabel={(option) => option.name}
                        loading={loadingProducts}
                        onChange={(_, value) => setSelectedProduct(value)}
                        renderInput={(params) => <TextField {...params} label="Продукт" margin="normal" />}
                        noOptionsText="Не знайдено продуктів"
                    />
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Грами"
                        type="number"
                        fullWidth
                        value={grams}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value === '' || (/^\d+$/.test(value))) {
                                setGrams(value);
                            }
                        }}
                        error={grams === '' || Number(grams) < 1 || Number(grams) > 2000}
                        helperText={
                            grams === ''
                                ? 'Поле не може бути порожнім'
                                : Number(grams) < 1
                                    ? 'Грами мають бути більше 0 або дорівнювати 1'
                                    : Number(grams) > 2000
                                        ? 'Максимальне значення - 2000 грамів'
                                        : ''
                        }
                    />

                    {selectedProduct && (
                        <div style={{ marginTop: '16px', textAlign: 'center' }}>
                            <img
                                src={selectedProduct.imageId
                                    ? selectedProduct.imageId
                                    : 'https://res.cloudinary.com/dzvxzhmfr/image/upload/v1747510946/product_without_pic_main.jpg'}
                                alt={selectedProduct.name}
                                style={{ maxWidth: '100%', maxHeight: '150px', objectFit: 'contain', background: '#f0f0f0', borderRadius: 8 }}
                            />
                            <Typography variant="subtitle1" mt={2}>
                                Попередні розрахунки:
                            </Typography>
                            <Typography variant="body2" mt={1}>
                                {(selectedProduct.caloriesPer100Grams * Number(grams) / 100).toFixed(0)} ккал ·
                                Б: {(selectedProduct.proteinPer100Grams * Number(grams) / 100).toFixed(0)}г ·
                                Ж: {(selectedProduct.fatPer100Grams * Number(grams) / 100).toFixed(0)}г ·
                                В: {(selectedProduct.carbohydratesPer100Grams * Number(grams) / 100).toFixed(0)}г
                            </Typography>
                        </div>
                    )}
                </DialogContent>

                <DialogActions>
                    <Button onClick={() => {
                        setOpenAddDialog(false);
                        setSelectedProduct(null);
                        setGrams('100');
                    }
                    }>Скасувати</Button>
                    <Button
                        variant="contained"
                        disabled={!selectedProduct || Number(grams) <= 0 || Number(grams) > 2000 || addingProduct}
                        onClick={async () => {
                            if (!selectedProduct || Number(grams) <= 0) return;
                            setAddingProduct(true);
                            try {
                                await apiClient.Diary.addProductRecord({
                                    productNutritionId: selectedProduct.id,
                                    consumedGrams: Number(grams),
                                    date: selectedDate.format("YYYY-MM-DD"),
                                });
                                toast.success('Продукт успішно додано!');
                                setOpenAddDialog(false);
                                setSelectedProduct(null);
                                setGrams('100');
                                setRefreshTrigger(prev => prev + 1);
                            } catch (error) {
                                toast.error('Помилка додавання продукту!');
                                console.error(error);
                            } finally {
                                setAddingProduct(false);
                            }
                        }}
                    >
                        {addingProduct ? 'Додається...' : 'Додати'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid>
    );
}
