import { useDispatch, useSelector } from "react-redux";
import {
  selectToken,
  setLoginDetails,
  setSignOut,
} from "../features/login/loginSlice";
import { requests } from "../app/api/api";
import {
  code_challenge_method,
  code_challenge_promise,
  code_verifier_promise,
} from "../utilities/codeChallenge";
import { useEffect, useState } from "react";

interface IProps {}

const identityServerBaseUrl = "https://localhost:7033";
const authUrl = identityServerBaseUrl + "/connect/authorize";
const accessTokenUrl = identityServerBaseUrl + "/connect/token";
const currentWindowUrl = window.location.origin;
const callbackUrl = currentWindowUrl + "/oauthcallback";

const Header = (props: IProps) => {
  const dispatch = useDispatch();
  const [authWindow, setAuthWindow] = useState<Window | null>(null);
  const token = useSelector(selectToken);
  const [codeChallenge, setCodeChallenge] = useState<string | null>(null);
  const [codeChallengeMethod, setCodeChallengeMethod] = useState<string | null>(
    null
  );
  const [codeVerifier, setCodeVerifier] = useState<string | null>(null);

  useEffect(() => {
    code_challenge_promise().then((c) => {
      setCodeChallenge(c);
    });
    code_verifier_promise().then((v) => {
      setCodeVerifier(v);
    });
    setCodeChallengeMethod(code_challenge_method);
  }, []);

  useEffect(() => {
    // Define a handler function that will receive the data from the new window or tab
    const handleMessage = (event: MessageEvent) => {
      // Check the origin of the message
      if (event.origin !== currentWindowUrl) {
        // Ignore messages from other origins
        return;
      }
      // Check the data of the message
      if (event.data && event.data.code) {
        // The message contains the authorization code
        console.log("Received code:", event.data.code);

        // Close the new window or tab
        authWindow!.close();
        // Redirect to the callback URL
        //window.location.href = "https://example.com/callback?code=" + event.data.code;
        getToken(event.data.code);
      }
    };
    // Add an event listener for the message event on the original window or tab
    window.addEventListener("message", handleMessage);
    // Remove the event listener when the component is unmounted
    return () => {
      window.removeEventListener("message", handleMessage);
    };
  }, [authWindow]);

  const handleLogin = async () => {
    const auhtorizationUrl = `${authUrl}?response_type=code&client_id=postman&client_secret=postman-secret&redirect_uri=${encodeURI(
      callbackUrl
    )}&scope=api&code_challenge=${codeChallenge}&code_challenge_method=${codeChallengeMethod}`;

    // Open the auth URL in a new window or tab
    let newWindow = window.open(auhtorizationUrl, "_blank");
    // Save the reference to the new window or tab
    setAuthWindow(newWindow);
  };

  const getToken = (authCode: string) => {
    requests
      .postWithHeaders(
        accessTokenUrl,
        {
          grant_type: "authorization_code",
          code: authCode,
          redirect_uri: encodeURI(callbackUrl),
          client_id: "postman",
          client_secret: "postman-secret",
          code_verifier: codeVerifier,
        },
        { "Content-Type": "application/x-www-form-urlencoded" }
      )

      .then((res: any) => {
        console.log(res);
        dispatch(setLoginDetails({ token: res.access_token }));
      })
      .catch(() => {
        alert("Access token could not be retrieved");
      });
  };

  const handleLogout = () => {
    dispatch(setSignOut({}));
  };

  //console.log("token is ", token);
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
