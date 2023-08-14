import { createSlice } from "@reduxjs/toolkit";
import { ILoginState } from "../../types/ILoginState";
import { AppStateType } from "../../app/RootReducer";

const initialState: ILoginState = {
  accessToken: "",
  refreshToken: "",
};

const loginSlice = createSlice({
  name: "login",
  initialState,
  reducers: {
    setLoginDetails: (state, action) => {
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
    },

    setSignOut: (state, action) => {
      state.accessToken = "";
      state.refreshToken = "";
    },
  },
});

export const { setLoginDetails, setSignOut } = loginSlice.actions;

export const selectAccessToken = (state: AppStateType) =>
  state.login.accessToken;
export const selectRefreshToken = (state: AppStateType) =>
  state.login.refreshToken;

export default loginSlice.reducer;
