import React from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import Home from "./components/Home";
import OAuthCallback from "./components/OAuthCallback";

function App() {
  return (
    <div className="App">
      <Router>
        <Header />
        <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route path="/oauthcallback" element={<OAuthCallback />}></Route>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
