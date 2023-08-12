import { useDispatch, useSelector } from "react-redux";
import {
  selectToken,
  setLoginDetails,
  setSignOut,
} from "../features/login/loginSlice";

interface IProps {}

const Header = (props: IProps) => {
  const dispatch = useDispatch();
  const token = useSelector(selectToken);

  const handleLogin = () => {
    dispatch(
      setLoginDetails({ token: Math.random().toString(16).substring(2, 10) })
    );
  };

  const handleLogout = () => {
    dispatch(setSignOut({}));
  };

  console.log("token is ", token);
  return (
    <>
      {token === undefined || token === "" ? (
        <input
          type="button"
          id="btnLogin"
          value="LOGIN"
          onClick={handleLogin}
        />
      ) : (
        <input
          type="button"
          id="btnLogout"
          value="LOGOUT"
          onClick={handleLogout}
        />
      )}
    </>
  );
};

export default Header;
