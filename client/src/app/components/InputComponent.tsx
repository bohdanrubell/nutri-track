import { TextField } from "@mui/material";
import { useController, UseControllerProps, FieldValues } from "react-hook-form";

interface Properties<TFieldValues extends FieldValues> extends UseControllerProps<TFieldValues> {
    label: string;
    type?: string;
    helperText?: string;
    error?: boolean;
}

export default function InputComponent<TFieldValues extends FieldValues>(properties: Properties<TFieldValues>) {
    const { fieldState, field } = useController<TFieldValues>({...properties});

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = properties.type === 'number' ? Number(event.target.value) : event.target.value;
        field.onChange(value);
    };

    return (
        <TextField
            margin="normal"
            fullWidth
            {...field}
            {...properties}
            onChange={handleChange}
            onBlur={field.onBlur}
            type={properties.type}
            error={properties.error ?? !!fieldState.error}
            helperText={properties.helperText ?? fieldState.error?.message}
        />
    );
}
