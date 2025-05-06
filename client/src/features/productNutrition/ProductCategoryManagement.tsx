import React, { useState } from 'react';
import { 
    Typography, 
    Paper, 
    TextField, 
    Button, 
    List, 
    ListItem, 
    ListItemText, 
    ListItemSecondaryAction, 
    IconButton, 
    Divider,
    Box,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogContentText,
    DialogActions
} from '@mui/material';
import { Delete as DeleteIcon, Add as AddIcon } from '@mui/icons-material';
import { toast } from 'react-toastify';
import useProductsNutrition from './useProductsNutrition';
import { useAppDispatch } from '../../app/store/store';
import { fetchCategories } from './productNutritionSlice';
import apiClient from '../../app/axios/apiClient';

interface ProductCategoryManagementProps {
    onClose: () => void;
}

export default function ProductCategoryManagement({ onClose }: ProductCategoryManagementProps) {
    const { categories } = useProductsNutrition();
    const [newCategoryName, setNewCategoryName] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [categoryToDelete, setCategoryToDelete] = useState<{id: number, name: string} | null>(null);
    const dispatch = useAppDispatch();

    const handleAddCategory = async () => {
        if (!newCategoryName.trim()) {
            toast.error('Назва категорії не може бути порожньою');
            return;
        }

        setIsSubmitting(true);
        try {
            await apiClient.Admin.addProductCategory({categoryName: newCategoryName});
            toast.success(`Категорію "${newCategoryName}" успішно додано`);
            setNewCategoryName('');
            dispatch(fetchCategories());
        } catch (error: any) {
            toast.error(`Помилка при додаванні категорії: ${error.data?.title || 'Невідома помилка'}`);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleDeleteCategory = async () => {
        if (!categoryToDelete) return;
        
        setIsSubmitting(true);
        try {
            await apiClient.Admin.deleteProductCategory(categoryToDelete.id);
            toast.success(`Категорію "${categoryToDelete.name}" успішно видалено`);
            setCategoryToDelete(null);
            dispatch(fetchCategories());
        } catch (error: any) {
            toast.error(`Помилка при видаленні категорії: ${error.data?.title || 'Невідома помилка'}`);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Paper elevation={3} sx={{ p: 3, maxWidth: 600, mx: 'auto', my: 4 }}>
            <Typography variant="h5" gutterBottom align="center">
                Управління категоріями продуктів
            </Typography>
            <Divider sx={{ mb: 3 }} />
            
            {/* Add new category form */}
            <Box sx={{ display: 'flex', mb: 4, gap: 2 }}>
                <TextField
                    fullWidth
                    variant="outlined"
                    label="Нова категорія"
                    value={newCategoryName}
                    onChange={(e) => setNewCategoryName(e.target.value)}
                    disabled={isSubmitting}
                />
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={handleAddCategory}
                    disabled={isSubmitting}
                >
                    Додати
                </Button>
            </Box>
            
            {/* Categories list */}
            <Typography variant="h6" gutterBottom>
                Існуючі категорії
            </Typography>
            
            <List sx={{ bgcolor: 'background.paper' }}>
                {categories.map((category, index) => (
                    <React.Fragment key={index}>
                        <ListItem>
                            <ListItemText primary={category.name} />
                            <ListItemSecondaryAction>
                                <IconButton 
                                    edge="end" 
                                    aria-label="delete"
                                    onClick={() => setCategoryToDelete({ id: index + 1, name: category.name })}
                                >
                                    <DeleteIcon />
                                </IconButton>
                            </ListItemSecondaryAction>
                        </ListItem>
                        {index < categories.length - 1 && <Divider />}
                    </React.Fragment>
                ))}
                {categories.length === 0 && (
                    <ListItem>
                        <ListItemText primary="Немає доступних категорій" />
                    </ListItem>
                )}
            </List>
            
            <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
                <Button variant="outlined" onClick={onClose}>
                    Закрити
                </Button>
            </Box>
            
            {/* Confirmation Dialog */}
            <Dialog
                open={!!categoryToDelete}
                onClose={() => setCategoryToDelete(null)}
            >
                <DialogTitle>Підтвердження видалення</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Ви впевнені, що хочете видалити категорію "{categoryToDelete?.name}"?
                        Це може вплинути на продукти, пов'язані з цією категорією.
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setCategoryToDelete(null)} color="primary">
                        Скасувати
                    </Button>
                    <Button onClick={handleDeleteCategory} color="error" disabled={isSubmitting}>
                        Видалити
                    </Button>
                </DialogActions>
            </Dialog>
        </Paper>
    );
} 