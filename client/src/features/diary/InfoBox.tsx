import {Paper, Stack, Typography} from "@mui/material";

interface InfoBoxProperties{
    label: string;
    value: string;
    color?: 'error' | 'success' | 'warning';
}

export default function InfoBox({ label, value, color }: InfoBoxProperties){
    return(
        <Paper sx={{ p: 1.5, minWidth: 100, textAlign: 'center', border: '2px solid', borderColor: `${color}.main` }}>
            <Stack spacing={0.5}>
                <Typography variant="body1">{label}:</Typography>
                <Typography variant="h6" color={color}>{value}</Typography>
            </Stack>
        </Paper>
    )
}