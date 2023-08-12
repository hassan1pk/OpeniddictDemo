import { combineReducers, configureStore } from "@reduxjs/toolkit";
import loginReducer from "../features/login/loginSlice";
import { ILoginState } from "../types/ILoginState";

export interface RootStore {
  login: ILoginState;
}

const rootReducer = combineReducers({
  login: loginReducer,
});

export default configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({ serializableCheck: false }),
});
