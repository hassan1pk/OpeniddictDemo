import { Fragment, useState } from "react";
import { requests } from "../app/api/api";

interface IProps {}
const Home = (props: IProps) => {
  const [weatherData, setWeatherData] = useState<any>(null);
  const [isError, setIsError] = useState<boolean>(false);
  const handleGetWeather = () => {
    requests
      .get(`https://localhost:7188/weatherforecast`)
      .then((res: any) => {
        console.log("Weather data retrieved is :", res);
        setWeatherData(res);
        setIsError(false);
      })
      .catch((err) => {
        console.error("Weather data could not be retrieved!", err);
        setIsError(true);
      });
  };

  return (
    <>
      <p>Home page</p>
      <input type="button" value="Get weather" onClick={handleGetWeather} />
      {!isError && (
        <p>
          {weatherData &&
            weatherData.map((item: any, index: number) => (
              <Fragment key={index}>
                <span>
                  Weather is {item.summary} on {item.date}. It's{" "}
                  {item.temperatureC} degress celsius.
                </span>
                <br></br>
              </Fragment>
            ))}
        </p>
      )}
      {isError && (
        <p style={{ color: "red" }}>Couldn't retrieve weather data!</p>
      )}
    </>
  );
};

export default Home;
