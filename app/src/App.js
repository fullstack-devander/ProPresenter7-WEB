import './App.css';
import { Dropdown } from 'primereact/dropdown';
import { Image } from 'primereact/image';
import { ScrollPanel } from 'primereact/scrollpanel';
import { Button } from 'primereact/button';
import { Sidebar } from 'primereact/sidebar';

import React, { useState, useEffect } from 'react';
import PlaylistPanel from './components/playlist-panel/PlaylistPanel';

function App() {
  const [isVisibleTopPanel, setIsVisibleTopPanel] = useState(false);

  const [presentationDetails, setPresentationDetails] = useState(null); // selected presentation details: { uuid, name, slideCount }

  const [preview, setPreview] = useState(null);
  const [activeSlideDetails, setActiveSlideDetails] = useState(null); // active slide details: { presentationUuid, slideIndex }

  const [isLoadingPreview, setIsLoadingPreview] = useState(false);

  const apiUrl = 'http://localhost:3001';
  //const apiUrl = '';

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
    <div className="App">

        <Sidebar visible={isVisibleTopPanel} position="top" onHide={() => setIsVisibleTopPanel(false)}>
          <PlaylistPanel onSelectPresentation={(uuid) => {
            console.log("onSelectPresentation (callback)");
            console.log("Selected presentation uuid:");
            console.log(uuid);
          }} />
        </Sidebar>

        <div className='title-panel'>
          <div>
            <h1 style={{margin: '0'}}>Presentations</h1>
          </div>
          <div>
            <Button icon="pi pi-align-justify" onClick={() => setIsVisibleTopPanel(true)} />
          </div>
        </div>
      
      {/*
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
