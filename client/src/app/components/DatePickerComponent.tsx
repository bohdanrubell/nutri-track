import { FormControl } from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers";
import { useController, UseControllerProps, FieldValues } from "react-hook-form";
import dayjs from 'dayjs';

interface Properties<TFieldValues extends FieldValues> extends UseControllerProps<TFieldValues> {
    label: string;
}

export default function DatePickerComponent<TFieldValues extends FieldValues>(properties: Properties<TFieldValues>) {
    const { field, fieldState } = useController<TFieldValues>({ ...properties });

    const handleChange = (date: dayjs.Dayjs | null) => {
        const formattedDate = date ? date.format('YYYY-MM-DD') : '';
        field.onChange(formattedDate);
    };

    return (
        <FormControl fullWidth margin="normal" error={!!fieldState.error}>
            <DatePicker
                label={properties.label}
                value={field.value ? dayjs(field.value) : null} // з рядка назад у dayjs для відображення
                onChange={handleChange}
                slotProps={{
                    textField: {
                        error: !!fieldState.error,
                        helperText: fieldState.error?.message
                    }
                }}
            />
        </FormControl>
    );
}
