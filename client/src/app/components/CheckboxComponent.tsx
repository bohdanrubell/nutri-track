import { FormGroup, FormControlLabel, Checkbox } from "@mui/material";
import { useEffect, useState } from "react";
import { ProductNutritionCategory } from "../models/productNutrition.ts";
import SimpleBar from "simplebar-react";
import Typography from "@mui/material/Typography";

interface CheckboxComponentProps {
    availableItems: ProductNutritionCategory[];
    selectedItems?: ProductNutritionCategory[];
    onChangeSelection: (selected: ProductNutritionCategory[]) => void;
}

export default function CheckboxComponent({ availableItems, selectedItems = [], onChangeSelection }: CheckboxComponentProps) {
    const [checkedItems, setCheckedItems] = useState<ProductNutritionCategory[]>(selectedItems);

    useEffect(() => {
        setCheckedItems(selectedItems);
    }, [selectedItems]);

    function handleToggle(item: ProductNutritionCategory) {
        const isChecked = checkedItems.some(selected => selected.name === item.name);
        const updatedItems = isChecked
            ? checkedItems.filter(selected => selected.name !== item.name)
            : [...checkedItems, item];

        setCheckedItems(updatedItems);
        onChangeSelection(updatedItems);
    }

    return (
        <SimpleBar style={{ maxHeight: 170 }}>
            <Typography variant="subtitle2" fontWeight="bold" mb={1}>
                Вибір категорії
            </Typography>
            <FormGroup>
            {availableItems.map(item => (
                <FormControlLabel
                    key={item.name}
                    control={
                        <Checkbox
                            checked={checkedItems.some(selected => selected.name === item.name)}
                            onChange={() => handleToggle(item)}
                        />
                    }
                    label={item.name}
                />
            ))}
        </FormGroup>
        </SimpleBar>
    );
}
