import './App.css';
import { Sidebar } from 'primereact/sidebar';
import { Button } from 'primereact/button';

import React, { useState, useEffect } from 'react';

function App() {
  const [slide, setSlide] = useState(null);
  const [isVisiblePlaylists, setIsVisiblePlaylists] = useState(false);
  const [playlists, setPlaylists] = useState([]);

  useEffect(() => {
    fetch('/playlists', { method: 'GET' }).then(res => res.json())
      .then(playlists => {
        console.log(playlists);
        setPlaylists(playlists);
      })
      .finally(() => console.log("LOADED"));
  }, []);

  async function previousCue() {
    console.log("PREVIOUS");
    const url = window.location.host;
    await fetch(`/prev`, {
      method: 'GET'
    });
  }

  async function nextCue() {
    console.log("NEXT CUE");
    const url = `${window.location.host}/next`;
    await fetch('/next', {
      method: 'GET'
    });
  }

  return (
    <div className="App" style={{padding: '32px 0'}}>
      <div>
        <Sidebar visible={isVisiblePlaylists} onHide={() => setIsVisiblePlaylists(false)}>
          Playlists
        </Sidebar>
        <Button label="Playlists" onClick={() => setIsVisiblePlaylists(true)} />
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
