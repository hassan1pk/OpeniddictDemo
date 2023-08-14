import { useEffect, useRef } from "react";

interface IParams {}

const OAuthCallback = (params: IParams) => {
  const hasRun = useRef(false);

  useEffect(() => {
    // Get the authorization code from the query string
    let code = new URLSearchParams(window.location.search).get("code");
    if (!hasRun.current) {
      console.log("auth code is", code);
      //alert(code);
      // Send the code to the original window or tab
      window.opener.postMessage({ code: code });
      hasRun.current = true;
    }
  }, []);

  return <p>OAuth callback</p>;
};

export default OAuthCallback;
