import { requests } from "../app/api/api";

interface IProps {}
const Home = (props: IProps) => {
  const handleGetWeather = () => {
    requests
      .get(`https://localhost:7188/weatherforecast`)
      .then((res: any) => {
        console.log(res);
      })
      .catch(() => {
        alert("Weather data could not be retrieved!");
      });
  };

  return (
    <>
      <p>Home page</p>
      <input type="button" value="Get weather" onClick={handleGetWeather} />
    </>
  );
};

export default Home;
