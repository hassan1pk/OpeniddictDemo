import { combineReducers, configureStore } from "@reduxjs/toolkit";
import loginReducer from "../features/login/loginSlice";
//import { ILoginState } from "../types/ILoginState";

const rootReducer = combineReducers({
  login: loginReducer,
});

/*export interface AppState {
  login: ILoginState;
}*/

export type AppState = ReturnType<typeof rootReducer>;

export default configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({ serializableCheck: false }),
});
