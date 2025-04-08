import { useEffect, useState } from "react";
import {
    Typography,
    Select,
    MenuItem,
    FormControl,
    InputLabel,
    Box,
    Grid,
    Stack,
    Paper
} from "@mui/material";
import {
    BarChart,
    BarSeriesType,
    axisClasses
} from "@mui/x-charts";
import apiClient from "../../app/axios/apiClient.ts";

interface PeriodStatisticsResponse {
    date: string;
    consumedCalories: number;
    consumedProteins: number;
    consumedFats: number;
    consumedCarbohydrates: number;
    status: 'NotReached' | 'Reached' | 'Exceeded';
}

export default function PeriodStatisticsView() {
    const [period, setPeriod] = useState<'last3days' | 'currentweek' | 'previousweek'>('last3days');
    const [data, setData] = useState<PeriodStatisticsResponse[]>([]);

    useEffect(() => {
        apiClient.Diary.getStatisticsByPeriod(period)
            .then(res => setData(res))
            .catch(err => console.error("Error fetching stats", err));
    }, [period]);

    console.log(data)

    const xLabels = data.map(d => d.date);

    const series: BarSeriesType[] = [
        {
            type: 'bar',
            data: data.map(d => d.consumedCalories),
            label: 'Калорії',
            id: 'calories'
        },
        {
            type: 'bar',
            data: data.map(d => d.consumedProteins),
            label: 'Білки',
            id: 'proteins'
        },
        {
            type: 'bar',
            data: data.map(d => d.consumedFats),
            label: 'Жири',
            id: 'fats'
        },
        {
            type: 'bar',
            data: data.map(d => d.consumedCarbohydrates),
            label: 'Вуглеводи',
            id: 'carbohydrates'
        }
    ];

    function getStatusText(status: string) {
        switch (status) {
            case 'Reached': return 'Норма досягнута';
            case 'Exceeded': return 'Перевищено норму';
            case 'NotReached': return 'Норма не досягнута';
            default: return 'Невідомо';
        }
    }

    function getStatusColor(status: string) {
        switch (status) {
            case 'Reached': return 'success.light';
            case 'Exceeded': return 'warning.light';
            case 'NotReached': return 'error.light';
            default: return 'grey.100';
        }
    }

    return (
        <Box sx={{ maxWidth: 1200, mx: "auto", p: 3 }}>
            <Typography variant="h4" gutterBottom textAlign="center">
                Статистика за спожиті КБЖВ
            </Typography>

            <FormControl sx={{ mb: 4, minWidth: 250 }}>
                <InputLabel>Період</InputLabel>
                <Select
                    value={period}
                    label="Період"
                    onChange={(e) => setPeriod(e.target.value as typeof period)}
                >
                    <MenuItem value="last3days">Останні 3 дні</MenuItem>
                    <MenuItem value="currentweek">Поточний тиждень</MenuItem>
                    <MenuItem value="previousweek">Попередній тиждень</MenuItem>
                </Select>
            </FormControl>

            <Grid container spacing={4}>
                <Grid item xs={12} md={7}>
                    <Paper elevation={3} sx={{ p: 2 }}>
                        <BarChart
                            xAxis={[{ scaleType: 'band', data: xLabels }]}
                            series={series}
                            height={400}
                            sx={{
                                [`.${axisClasses.left} .MuiTypography-root`]: { fontSize: 14 },
                                [`.${axisClasses.bottom} .MuiTypography-root`]: { fontSize: 12 },
                            }}
                        />
                    </Paper>
                </Grid>

                <Grid item xs={12} md={5}>
                    <Typography variant="h6" gutterBottom>
                        Статус по днях:
                    </Typography>

                    <Stack spacing={1}>
                        {data.map((day) => (
                            <Paper
                                key={day.date}
                                sx={{
                                    p: 2,
                                    bgcolor: getStatusColor(day.status),
                                    borderLeft: '6px solid',
                                    borderColor:
                                        day.status === 'Reached' ? 'green' :
                                            day.status === 'Exceeded' ? 'orange' : 'red'
                                }}
                                elevation={1}
                            >
                                <Typography fontWeight={500}>
                                    {day.date} — {getStatusText(day.status)}
                                </Typography>
                            </Paper>
                        ))}
                    </Stack>
                </Grid>
            </Grid>
        </Box>
    );
}
