import { createSlice } from "@reduxjs/toolkit";
import { ILoginState } from "../../types/ILoginState";
import { AppStateType } from "../../app/RootReducer";

const initialState: ILoginState = {
  token: "",
};

const loginSlice = createSlice({
  name: "login",
  initialState,
  reducers: {
    setLoginDetails: (state, action) => {
      state.token = action.payload.token;
    },

    setSignOut: (state, action) => {
      state.token = "";
    },
  },
});

export const { setLoginDetails, setSignOut } = loginSlice.actions;

export const selectToken = (state: AppStateType) => state.login.token;

export default loginSlice.reducer;
