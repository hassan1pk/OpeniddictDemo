import { createSlice } from "@reduxjs/toolkit";
import { ILoginState } from "../../types/ILoginState";
import { AppState } from "../../app/store";

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

export const selectToken = (state: AppState) => state.login.token;

export default loginSlice.reducer;
