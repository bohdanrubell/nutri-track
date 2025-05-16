import { Typography, Paper, Box, Button } from "@mui/material";
import Grid from "@mui/material/Grid2";
import {FieldValues, useForm} from "react-hook-form";
import InputComponent from "../../app/components/InputComponent.tsx";
import {yupResolver} from "@hookform/resolvers/yup";
import {useAppDispatch} from "../../app/store/store.ts";
import {fetchCategories, fetchProductAsync, setProduct} from "./productNutritionSlice.ts";
import {LoadingButton} from "@mui/lab";
import {ProductNutrition} from "../../app/models/productNutrition.ts";
import {validationSchema} from "./productNutritionValidation.ts";
import useProductsNutrition from "./useProductsNutrition.tsx";
import ListComponent from "../../app/components/ListComponent.tsx";
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

    const isEditMode = Boolean(productNutrition);

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
    }, [productNutrition, reset, isDirty]);


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
            dispatch(fetchProductAsync(productNutrition!.id));
            dispatch(fetchCategories())
            cancelCreate();
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <Box component={Paper} sx={{p: 4}}>
            <Typography variant="h5" fontStyle={'inherit'} display={'flex'} justifyContent={'center'} sx={{ mb: 4 }}>
                {isEditMode
                    ? `Оновлення продукту: ${productNutrition?.name}`
                    : "Створення нового продукту"}
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
                    <Grid size={{xs: 12, sm:6}}>
                        <InputComponent control={control} name='name' label='Назва продукту'/>
                    </Grid>
                    <Grid size={{xs: 12, sm:6}}>
                        <InputComponent control={control} name='caloriesPer100Grams' label='Калорійність на 100г'/>
                    </Grid>
                    <Grid size={{xs: 12, sm:6}}>
                        <InputComponent control={control} name='proteinPer100Grams' label='Білки'/>
                    </Grid>
                    <Grid size={{xs: 12, sm:6}}>
                        <InputComponent control={control} name='fatPer100Grams' label='Жири'/>
                    </Grid>
                    <Grid size={{xs: 12, sm:6}}>
                        <InputComponent control={control} name='carbohydratesPer100Grams' label='Вуглеводи'/>
                    </Grid>
                    <Grid size={{xs: 12, sm:6}}>
                        <ListComponent control={control} items={categories.map(c => c.name)} name='categoryName'
                                       label='Категорія'/>
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-around' sx={{mt: 3}}>
                    <Button onClick={cancelCreate} variant='contained' color='inherit'>Скасувати</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained'
                                   color='success'>
                        {isEditMode ? "Оновити" : "Додати"}
                    </LoadingButton>
                </Box>
            </form>
        </Box>
    )
}