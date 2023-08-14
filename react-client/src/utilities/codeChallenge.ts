export const code_verifier_promise =
  async function generateCodeVerifier(): Promise<string> {
    let key: CryptoKey = await window.crypto.subtle.generateKey(
      {
        name: "AES-GCM",
        length: 256,
      },
      true,
      ["encrypt", "decrypt"]
    );
    let buffer: ArrayBuffer = await window.crypto.subtle.exportKey("raw", key);
    let code_verifier: string = (buffer as Buffer).toString("hex");
    //console.log(code_verifier); // e.g. "0fb5b7602cdfa61088951eac36429862946e86d20b15250a8f0159f1ad001605"
    return Promise.resolve(code_verifier);
  };

// Browser
function base64urlEncode(buffer: ArrayBuffer): string {
  let str = "";
  let bytes = new Uint8Array(buffer);
  let len = bytes.byteLength;
  for (let i = 0; i < len; i++) {
    str += String.fromCharCode(bytes[i]);
  }
  return btoa(str).replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/, "");
}

export const code_challenge_method = "S256";
export const code_challenge_promise =
  async function generateCodeChallenge(): Promise<string> {
    let hash = await window.crypto.subtle.digest(
      "SHA-256",
      new TextEncoder().encode(await code_verifier_promise())
    );
    let code_challenge = base64urlEncode(hash);
    //console.log(code_challenge); // e.g. "SRvuz5GW2HhXzHs6b3O_wzJq4sWN0W2ma96QBx_Z77s"
    return Promise.resolve(code_challenge);
  };
