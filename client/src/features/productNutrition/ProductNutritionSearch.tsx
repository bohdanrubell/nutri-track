import { TextField } from "@mui/material";
import { useAppDispatch, useAppSelector } from '../../app/store/store.ts';
import { useState } from 'react';
import { unstable_debounce } from '@mui/utils';
import {setProductParams} from "./productNutritionSlice.ts";

export default function ProductNutritionSearch() {
    const { productParams } = useAppSelector(state => state.productNutrition);
    const [searchTerm, setSearchTerm] = useState(productParams.searchTerm);
    const dispatch = useAppDispatch();

    const debouncedSearch = unstable_debounce((event: any) => {
        dispatch(setProductParams({ searchTerm: event.target.value }))
    }, 1000);

    return (
        <TextField
            label='Введіть назву продукту'
            variant='outlined'
            fullWidth
            value={searchTerm || ''}
            onChange={(event: any) => {
                setSearchTerm(event.target.value);
                debouncedSearch(event);
            }}
        />
    )
}