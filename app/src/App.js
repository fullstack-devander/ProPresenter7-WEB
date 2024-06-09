import './App.css';
import { Dropdown } from 'primereact/dropdown';
import { Image } from 'primereact/image';
import { ScrollPanel } from 'primereact/scrollpanel';

import React, { useState, useEffect } from 'react';

function App() {
  const [playlists, setPlaylists] = useState([]); // list of playlists: { uuid, name }
  const [activePlaylist, setActivePlaylist] = useState(null); // selected playlist: { uuid, name }
  const [presentationList, setPresentationList] = useState([]); // list of presentations: { uuid, name }
  const [activePresentation, setActivePresentation] = useState(null); // selected presentation: { uuid, name }
  const [presentationDetails, setPresentationDetails] = useState(null); // selected presentation details: { uuid, name, slideCount }

  const [isLoadingPreview, setIsLoadingPreview] = useState(false);

  //const apiUrl = 'http://localhost:3001';
  const apiUrl = '';

  useEffect(() => {
    fetch(`${apiUrl}/playlists`, { method: 'GET' }).then(res => res.json()).then(playlists => {
        const list = playlists.map(item => item.id);
        
        setPlaylists(list);
        setActivePlaylist(list[0].uuid);
        
        fetchPresentationList(list[0].uuid);
      }).finally(() => {
        console.log("LOADED");
      });
  }, []);

  async function fetchPresentationList(uuid) {
    const response = await fetch(`${apiUrl}/playlist/${uuid}`, { method: 'GET' }).then(res => res.json());
    const presentations = response.items.map(item => item.id);
    setPresentationList(presentations);
    setActivePresentation(presentations[0].uuid);
    await fetchPresentationDetails(presentations[0].uuid);
  }
  
  async function fetchPresentationDetails(uuid) {
    const response = await fetch(`${apiUrl}/presentation/${uuid}`, { method: 'GET' }).then(res => res.json());
    setPresentationDetails(response);
  }

  function slideImages() {
    const slides = [];

    for (let i = 0; i < presentationDetails?.slideCount; i++) {
      slides.push(<Image src={`${apiUrl}/presentation/${presentationDetails.uuid}/thumbnail/${i}`} width='300' style={{margin: 8}} />);
    }

    return slides;
  }

  async function changeActivePlaylist(dropDownChangeEvent) {
    setActivePlaylist(dropDownChangeEvent.value);
    await fetchPresentationList(dropDownChangeEvent.value);
  }

  async function changeActivePresentation(dropDownChangeEvent) {
    setActivePresentation(dropDownChangeEvent.value);
    await fetchPresentationDetails(dropDownChangeEvent.value);
  }

  async function previousCue() {
    console.log("PREVIOUS");
    const url = window.location.host;
    await fetch(`${apiUrl}/prev`, {
      method: 'GET'
    });
  }

  async function nextCue() {
    console.log("NEXT CUE");
    const url = `${window.location.host}/next`;
    await fetch(`${apiUrl}/next`, {
      method: 'GET'
    });
  }

  return (
    <div className="App" style={{padding: '32px 0'}}>
      <div>
        <Dropdown value={activePlaylist} options={playlists} optionValue="uuid" optionLabel="name" onChange={changeActivePlaylist} />
        <Dropdown value={activePresentation} options={presentationList} optionValue="uuid" optionLabel="name" onChange={changeActivePresentation} />
      </div>
      <div style={{margin: 16}}>
        {
          true &&
          <Image src={`${apiUrl}/preview`} width='400' />
        }
      </div>
      <div>
        {slideImages()}
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
