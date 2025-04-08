import { Typography, Grid, Paper, Box, Button } from "@mui/material";
import {FieldValues, useForm} from "react-hook-form";
import AppTextInput from "../../app/components/AppTextInput";
import {yupResolver} from "@hookform/resolvers/yup";
import {useAppDispatch} from "../../app/store/store.ts";
import {fetchProductAsync, setProduct} from "./productNutritionSlice.ts";
import {LoadingButton} from "@mui/lab";
import {ProductNutrition} from "../../app/models/productNutrition.ts";
import {validationSchema} from "./productNutritionValidation.ts";
import useProductsNutrition from "../../app/hooks/useProductsNutrition.tsx";
import AppSelectList from "../../app/components/AppSelectList.tsx";
import api from "../../app/api/api.ts";
import apiClient from "../../app/axios/apiClient.ts";
import {useEffect, useState} from "react";
import {toast} from "react-toastify";
import {useDropzone} from "react-dropzone";

interface FormProperties {
    productNutrition?: ProductNutrition;
    cancelCreate: () => void;
}

export default function ProductNutritionForm({productNutrition, cancelCreate}: FormProperties) {
    const { control, handleSubmit, reset, formState: { isSubmitting, isDirty} } = useForm({
        resolver: yupResolver<any>(validationSchema),
        mode: 'onBlur',
        reValidateMode: 'onBlur'
    });
    const dispatch = useAppDispatch();
    const {categories} = useProductsNutrition();
    const [selectedImage, setSelectedImage] = useState<File | null>(null);
    const [preview, setPreview] = useState<string | null>(null);


    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        accept: { 'image/*': [] },
        multiple: false,
        onDrop: (acceptedFiles) => {
            const file = acceptedFiles[0];
            if (file){
                setSelectedImage(file);
                setPreview(URL.createObjectURL(file));
            }
        }
    });

    useEffect(() => {
        if (productNutrition && !isDirty) {
            reset(productNutrition);
            if (!selectedImage) {
                setPreview(productNutrition.imageId || null);
            }
        }
        return () => {
            if (preview && preview.startsWith("blob:")) {
                URL.revokeObjectURL(preview);
            }
        };
    }, [productNutrition, reset, isDirty, selectedImage, preview]);

    async function handleSubmitData(data: FieldValues) {
        try {
            const formData = new FormData();

            if (productNutrition) {
                formData.append("id", productNutrition.id.toString());
            }

            formData.append("name", data.name);
            formData.append("caloriesPer100Grams", data.caloriesPer100Grams);
            formData.append("proteinPer100Grams", data.proteinPer100Grams);
            formData.append("fatPer100Grams", data.fatPer100Grams);
            formData.append("carbohydratesPer100Grams", data.carbohydratesPer100Grams);
            formData.append("categoryName", data.categoryName);

            if (selectedImage) {
                formData.append("file", selectedImage);
            }

            let response: ProductNutrition;

            if (productNutrition) {
                response = await apiClient.Admin.updateProductNutrition(formData);
                toast.success('Успішно оновлений продукт!');
            } else {
                response = await apiClient.Admin.createNewProductNutrition(formData);
                toast.success('Успішно додано новий продукт!');
            }

            dispatch(setProduct(response));
            cancelCreate();
            if (productNutrition) {
                dispatch(fetchProductAsync(response.id));
            }

        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Box component={Paper} sx={{p: 4}}>
            <Typography variant="h5" fontStyle={'inherit'} display={'flex'} justifyContent={'center'} sx={{mb: 4}}>
                Додавання нового/ Оновлення існуючого продукту харчування
            </Typography>
            <Box {...getRootProps()} sx={{
                border: '2px dashed gray',
                padding: 2,
                textAlign: 'center',
                borderRadius: 2,
                mt: 2,
                mb: 4,
                backgroundColor: isDragActive ? '#f0f0f0' : 'inherit',
                cursor: 'pointer'
            }}>
                <input {...getInputProps()} />
                {preview ? (
                    <img src={preview} alt="preview" style={{ maxHeight: 200, borderRadius: 8 }} />
                ) : (
                    <Typography variant="body2" color="textSecondary">
                        Перетягни або клікни, щоб завантажити зображення
                    </Typography>
                )}
            </Box>

            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput control={control} name='name' label='Назва продукту'/>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput control={control} name='caloriesPer100Grams' label='Калорійність на 100г'/>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput control={control} name='proteinPer100Grams' label='Білки'/>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput control={control} name='fatPer100Grams' label='Жири'/>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput control={control} name='carbohydratesPer100Grams' label='Вуглеводи'/>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppSelectList control={control} items={categories.map(c => c.name)} name='categoryName'
                                       label='Категорія'/>
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-around' sx={{mt: 3}}>
                    <Button onClick={cancelCreate} variant='contained' color='inherit'>Скасувати</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained'
                                   color='success'>Додати</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}