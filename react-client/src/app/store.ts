import { configureStore } from "@reduxjs/toolkit";
import storage from "redux-persist/es/storage";
import { PersistConfig, persistReducer, persistStore } from "redux-persist";
import rootReducer, { AppStateType } from "./RootReducer";

const persistConfig: PersistConfig<
  AppStateType,
  AppStateType,
  AppStateType,
  AppStateType
> = {
  key: "root",
  storage: storage,
  whitelist: ["login"],
  version: 1,
};

const persistedReducer = persistReducer<AppStateType>(
  persistConfig,
  rootReducer
);
const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({ serializableCheck: false }),
});

export const persistor = persistStore(store);

export default store;

/*export default configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({ serializableCheck: false }),
});*/
