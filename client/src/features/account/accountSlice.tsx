import {User} from "../../app/models/user.ts";
import {createAsyncThunk, createSlice, isAnyOf} from "@reduxjs/toolkit";
import {FieldValues} from "react-hook-form";
import apiClient from "../../app/axios/apiClient.ts";
import {router} from "../../app/router/Routes.tsx";
import {toast} from "react-toastify";

interface AccountState{
    user: User | null;
}

const initState: AccountState = {
    user: null
}

export const signInUser = createAsyncThunk<User, FieldValues>(
    'account/signInUser',
    async (data, thunkAPI) => {
        try {
            const userDto = await apiClient.Account.login({
                username: data.username,
                password: data.password
            });
            const {...user } = userDto;
            localStorage.setItem('user', JSON.stringify(user));
            if (user){
                toast.success("Успішна авторизація!")
                router.navigate('/productNutrition')
            }
            return user;
        }catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }
)

export const fetchCurrentUser = createAsyncThunk<User>(
    "account/fetchCurrentUser",
    async (_, thunkAPI) => {
        const storedUser = localStorage.getItem("user");
        if (storedUser) {
            thunkAPI.dispatch(setUser(JSON.parse(storedUser)));
        }
        try {
            const userDto = await apiClient.Account.currentUser();
            const { ...user } = userDto;
            localStorage.setItem("user", JSON.stringify(user));
            return user;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data });
        }
    },
    {
        condition: () => {
            return !!localStorage.getItem("user");
        },
    }
);

export const accountSlice = createSlice({
    name: 'account',
    initialState: initState,
    reducers: {
        signOut: (state) => {
            router.navigate('/', { state: { intentional: true } });
            state.user = null;
            localStorage.removeItem('user');
        },
        setUser: (state, action) => {
            const claims = JSON.parse(atob(action.payload.token.split('.')[1]));
            console.log(claims);
            const roles = claims['role']
            console.log(roles)
            state.user = {...action.payload, roles: typeof (roles) === 'string' ? [roles] : roles};
        }
    },
    extraReducers:(builder => {
        builder.addCase(fetchCurrentUser.rejected, (state) => {
            state.user = null;
            localStorage.removeItem('user');
            toast.error('Сесія вичерпана! Авторизуйтесь ще раз.');
            router.navigate('/');
        })
        builder.addMatcher(isAnyOf(signInUser.fulfilled, fetchCurrentUser.fulfilled), (state,action) => {
            const claims = JSON.parse(atob(action.payload.token.split('.')[1]));
            const roles = claims['role']
            state.user = {...action.payload, roles: typeof (roles) === 'string' ? [roles] : roles};
        });
        builder.addMatcher(isAnyOf(signInUser.rejected), (_state, action) => {
            throw action.payload;
        })
    })
});


export const {signOut, setUser} = accountSlice.actions;