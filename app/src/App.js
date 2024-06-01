import './App.css';
import logo from './LOGO.png';

import React, { useState, useEffect } from 'react';

function App() {
  const proPresenterAddress = "http://10.120.6.58:1025/";
  const nextCueUrl = proPresenterAddress + "v1/trigger/next";
  const prevCueUrl = proPresenterAddress + "v1/trigger/previous";

  const [slide, setSlide] = useState(null);

  useEffect(() => {
    console.log("LOADED");
  });

  async function previousCue() {
    console.log("PREVIOUS");
    await fetch(prevCueUrl, {
      method: 'GET'
    });
  }

  async function nextCue() {
    console.log("NEXT CUE");
    await fetch(nextCueUrl, {
      method: 'GET'
    });
  }

  return (
    <div className="App" style={{padding: '32px 0'}}>
      <div>
        <img src={logo} style={{width: '150px'}}/>
      </div>
      <div style={{verticalAlign: 'bottom'}}>
        <div className='my-btn'>
          <button style={{width: '100%', padding: '24px 0', fontSize: '32px', fontWeight: 'bold'}} className="btn btn-primary btn-lg" onClick={nextCue}>
            Наступний слайд
          </button>
        </div>
        <div className='my-btn'>
          <button style={{width: '100%', padding: '24px 0', fontSize: '26px'}} className="btn btn-secondary btn-lg btn-block" onClick={previousCue}>
            Попередній слайд
          </button>
        </div>
      </div>
    </div>
  );
}

export default App;
