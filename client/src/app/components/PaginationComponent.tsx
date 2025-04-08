import { Box, Pagination } from "@mui/material";
import { MetaData } from '../models/pagination';

interface PaginationProperties {
    metaData: MetaData,
    onPageChange: (page: number) => void;
}

export default function PaginationComponent({ metaData, onPageChange }: PaginationProperties) {
    const {currentPage, totalPages } = metaData;

    return (
        <Box display='flex' justifyContent='space-between' alignItems='center'>
            {totalPages > 1 && (
                <Pagination
                    color='secondary'
                    size='large'
                    count={totalPages}
                    page={currentPage}
                    onChange={(_e, page) => onPageChange(page)}
                />
            )}
        </Box>
    );
}
