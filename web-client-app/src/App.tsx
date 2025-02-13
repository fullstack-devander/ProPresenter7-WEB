import './App.css';
import { Image } from 'primereact/image';
import { Button } from 'primereact/button';
import { Toolbar } from 'primereact/toolbar';
import { ScrollPanel } from 'primereact/scrollpanel';
import { Panel } from 'primereact/panel';

import { useState, useEffect } from 'react';

import {
    getActiveSlideIndex,
    getPresentationDetails,
    getThumbnail,
    triggerNextSlide,
    triggerPrevSlide,
    triggerSlide
} from './services/ProPresenterService';

import { Presentation, Slide } from './core/index';

function App() {
    const [activeSlide, setActiveSlide] = useState<Slide | null>(null);
    const [presentationDetails, setPresentationDetails] = useState<Presentation | null>(null);

    useEffect(() => {
        // TODO: Uncomment when relevant endpoint is implemented
        //Init();
    }, []);

    async function Init(): Promise<void> {
        const presentationDetails = await getPresentationDetails();
        setPresentationDetails(presentationDetails);

        const activeSlide = await getActiveSlideIndex();
        setActiveSlide(activeSlide);
    }

    function slideImages(): any[] | null {
        if (presentationDetails == null) {
            return null;
        }

        const slides = [];

        for (let i = 0; i < presentationDetails.slideCount; i++) {
            slides.push(<Image
                src={getThumbnail(presentationDetails.uuid, i)}
                className={presentationDetails.uuid === activeSlide?.presentationUuid && i === activeSlide?.slideIndex ? 'slide active-slide' : 'slide'}
                key={i}
                width='300'
                onClick={() => {
                    setActiveSlide(null);
                    onTriggerSlide(presentationDetails.uuid, i);
                }} />);
        }

        return slides;
    }

    function onTriggerSlide(uuid: string, slideIndex: number): void {
        triggerSlide(uuid, slideIndex).then(() => {
            setTimeout(() => {
                getActiveSlideIndex().then(slide => {
                    setActiveSlide(slide);
                });
            }, 200);
        });
    }

    function onTriggerNextSlide() {
        triggerNextSlide().then(() => {
            setTimeout(() => {
                getActiveSlideIndex().then(slide => {
                    setActiveSlide(slide);
                });
            }, 200);
        });
    }

    function onTriggerPrevSlide() {
        triggerPrevSlide().then(() => {
            setTimeout(() => {
                getActiveSlideIndex().then(slide => {
                    setActiveSlide(slide);
                });
            }, 200);
        });
    }

    return (
        <div className="App">
            <Toolbar
                center={presentationDetails?.name}>
            </Toolbar>
            <div className='slide-container'>
                <ScrollPanel style={{ height: "calc(100vh - 260px)", width: "100%" }} className="scroll">
                    {slideImages()}
                </ScrollPanel>
            </div>

            <Panel>
                <Button
                    icon="pi pi-caret-left"
                    size='large'
                    tooltip='Previous slide'
                    tooltipOptions={{ position: 'top' }}
                    onClick={onTriggerPrevSlide}
                />
                <Button
                    icon="pi pi-caret-right"
                    size='large'
                    tooltip='Next slide'
                    tooltipOptions={{ position: 'top' }}
                    onClick={onTriggerNextSlide}
                />
            </Panel>

        </div>
    );
}

export default App;
