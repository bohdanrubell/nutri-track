import { Box, Pagination } from "@mui/material";
import { MetaData } from '../models/pagination';

interface PaginationProperties {
    metaData: MetaData | null;
    onPageChange: (page: number) => void;
}


export default function PaginationComponent({ metaData, onPageChange }: PaginationProperties) {
    if (!metaData) return null; // Защита от падения

    const { currentPage, totalPages } = metaData;

    if (totalPages <= 1) return null;

    return (
        <Box display='flex' justifyContent='space-between' alignItems='center'>
            {totalPages > 1 && (
                <Pagination
                    color='secondary'
                    size='large'
                    count={totalPages}
                    page={currentPage}
                    onChange={(_e, page) => onPageChange(page)}
                    siblingCount={1} // кількість сторінок по боках від активної
                    boundaryCount={1} // кількість крайніх сторінок ліворуч і праворуч
                    showFirstButton
                    showLastButton
                />
            )}
        </Box>
    );
}
