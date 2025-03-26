import { FormGroup, FormControlLabel, Checkbox } from "@mui/material";
import { useState } from "react";
import { ProductNutritionCategory } from "../models/productNutrition.ts";

interface CheckboxProperties {
    items: ProductNutritionCategory[];
    checked?: ProductNutritionCategory[];
    onChange: (items: ProductNutritionCategory[]) => void;
}

export default function CheckboxButton({ items, checked, onChange }: CheckboxProperties) {
    const [checkedItems, setCheckedItems] = useState(checked || []);

    function handleChecked(value: string) {
        const currentIndex = checkedItems.findIndex(item => item.name === value);
        let newChecked: ProductNutritionCategory[];
        if (currentIndex === -1) {
            newChecked = [...checkedItems, items.find(item => item.name === value)!];
        } else {
            newChecked = checkedItems.filter(i => i.name !== value);
        }
        setCheckedItems(newChecked);
        onChange(newChecked);
    }

    return (
        <FormGroup>
            {items.map(item => (
                <FormControlLabel
                    key={item.name}
                    control={<Checkbox
                        checked={checkedItems.some(checkedItem => checkedItem.name === item.name)}
                        onClick={() => handleChecked(item.name)}
                    />}
                    label={item.name} />
            ))}
        </FormGroup>
    );
}
