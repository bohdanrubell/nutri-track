import {TextField} from "@mui/material";
import {useController, UseControllerProps} from "react-hook-form";

interface Properties extends UseControllerProps {
    label: string;
    type?: string;
}

export default function InputComponent(properties: Properties){
    const {fieldState,field} = useController({...properties, defaultValue: ''})
    return (
        <TextField
            fullWidth
            variant="filled"
            {...properties}
            {...field}
            type={properties.type}
            helperText={fieldState.error?.message}
            error={!!fieldState.error}
        />
    )
}