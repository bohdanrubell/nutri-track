import {FormControl, FormHelperText, InputLabel, MenuItem, Select} from "@mui/material";
import {useController, UseControllerProps, FieldValues} from "react-hook-form";

interface Properties<TFieldValues extends FieldValues> extends UseControllerProps<TFieldValues> {
    label: string;
    items: string[];
}

export default function ListComponent<TFieldValues extends FieldValues>(props: Properties<TFieldValues>) {
    const { fieldState, field } = useController<TFieldValues>({...props});

    const handleChange = (event: any) => {
        const value = event.target.value ?? '';
        field.onChange(value);
    };

    return (
        <FormControl fullWidth margin="normal" error={!!fieldState.error}>
            <InputLabel>{props.label}</InputLabel>
            <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={field.value ?? ''}
                onBlur={field.onBlur}
                label={props.label}
                onChange={handleChange} // !!! тут тепер наша обробка
            >
                <MenuItem value="">
                    <em>Не обрано</em>
                </MenuItem>
                {props.items.map((item, index) => (
                    <MenuItem key={index} value={item}>
                        {item}
                    </MenuItem>
                ))}
            </Select>
            <FormHelperText>{fieldState.error?.message}</FormHelperText>
        </FormControl>
    );
}

