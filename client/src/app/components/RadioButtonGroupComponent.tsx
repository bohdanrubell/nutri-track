import { FormControl, RadioGroup, FormControlLabel, Radio } from "@mui/material";

interface RadioButtonProperties {
    options: any[];
    onChange: (event: any) => void;
    selectedValue: string;
}

export default function RadioButtonGroupComponent({ options, onChange, selectedValue }: RadioButtonProperties) {
    return (
        <FormControl component="fieldset">
            <RadioGroup onChange={onChange} value={selectedValue}>
                {options.map(({ value, label }) => (
                    <FormControlLabel
                        value={value}
                        control={<Radio />}
                        label={label} key={value}
                    />
                ))}
            </RadioGroup>
        </FormControl>
    )
}