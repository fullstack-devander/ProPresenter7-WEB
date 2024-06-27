import './App.css';
import { Dropdown } from 'primereact/dropdown';
import { Image } from 'primereact/image';
import { ScrollPanel } from 'primereact/scrollpanel';

import React, { useState, useEffect } from 'react';
import PlaylistPanel from './components/playlist-panel/PlaylistPanel';

function App() {
  const [playlists, setPlaylists] = useState([]); // list of playlists: { uuid, name }
  const [activePlaylist, setActivePlaylist] = useState(null); // selected playlist: { uuid, name }
  const [presentationList, setPresentationList] = useState([]); // list of presentations: { uuid, name }
  const [activePresentation, setActivePresentation] = useState(null); // selected presentation: { uuid, name }
  const [presentationDetails, setPresentationDetails] = useState(null); // selected presentation details: { uuid, name, slideCount }

  const [preview, setPreview] = useState(null);
  const [activeSlideDetails, setActiveSlideDetails] = useState(null); // active slide details: { presentationUuid, slideIndex }

  const [isLoadingPreview, setIsLoadingPreview] = useState(false);

  const apiUrl = 'http://localhost:3001';
  //const apiUrl = '';

  useEffect(() => {
    /*
    fetch(`${apiUrl}/playlists`, { method: 'GET' }).then(res => res.json()).then(playlists => {
      console.log(playlists);

      //const list = playlists.map(item => item.id);
      setPlaylists(playlists);
      setActivePlaylist(playlists[0].uuid);
      
      fetchPresentationList(activePlaylist);
    }).finally(() => {
      console.log("LOADED");
    });
    */
  }, []);

  useEffect(() => {
    /*
    fetch(`${apiUrl}/activeSlide`, { method: 'GET' }).then(res => res.json()).then(res => {
      setActiveSlideDetails(res);
      return res;
    }).then(res => {
      fetch(`${apiUrl}/presentation/${res.presentationUuid}/thumbnail/${res.slideIndex}`, { method: 'GET' }).then(res =>
        res.blob()).then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          setPreview(blobUrl);
        });
    });
    */

    /*
    fetch(`${apiUrl}/preview`, { method: 'GET' }).then(res => res.blob()).then(blob => {
      const blobUrl = URL.createObjectURL(blob);
      setPreview(blobUrl);
    });
    */
  }, [isLoadingPreview]);

  
  
  async function fetchPresentationDetails(uuid) {
    const response = await fetch(`${apiUrl}/presentation/${uuid}`, { method: 'GET' }).then(res => res.json());
    setPresentationDetails(response);
  }

  function slideImages() {
    const slides = [];

    for (let i = 0; i < presentationDetails?.slideCount; i++) {
      slides.push(<Image
        src={`${apiUrl}/presentation/${presentationDetails.uuid}/thumbnail/${i}`}
        key={i}
        width='300'
        style={{margin: 8}}
        onClick={async () => await onTriggerSlide(presentationDetails.uuid, i)} />);
    }

    return slides;
  }

  async function onTriggerSlide(uuid, slideIndex) {
    setIsLoadingPreview(true);
    
    await fetch(`${apiUrl}/presentation/${presentationDetails.uuid}/${slideIndex}/trigger`);
    await setTimeout(() => setIsLoadingPreview(false), 100);
  }

  async function previousCue() {
    await fetch(`${apiUrl}/trigger/prev`, { method: 'GET' });
  }

  async function nextCue() {
    await fetch(`${apiUrl}/trigger/next`, { method: 'GET' });
  }

  return (
    <div className="App" style={{padding: '32px 0'}}>
      <div>
        <PlaylistPanel />
      </div>
      {/*<div>
        <Dropdown value={activePlaylist} options={playlists} optionValue="uuid" optionLabel="name" onChange={changeActivePlaylist} />
        <Dropdown value={activePresentation} options={presentationList} optionValue="uuid" optionLabel="name" onChange={changeActivePresentation} />
      </div>
      <div style={{margin: 16, height:255}}>
        {
          !isLoadingPreview && <Image src={preview} width='400' />
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
      </div>*/}
    </div>
  );
}

export default App;
