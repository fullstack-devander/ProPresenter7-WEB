import './PlaylistPanel.css';
import { useState, useEffect } from 'react';
import { Dropdown } from 'primereact/dropdown';

import { getPlaylists } from '../../services/ProPresenterAPIService';

function PlaylistPanel({ onSelectPresentation }) {
    const [isLoading, setIsLoading] = useState(true);
    const [playlists, setPlaylists] = useState(null);
    const [presentations, setPresentations] = useState(null);
    const [selectedPlaylistUuid, setSelectedPlaylistUuid] = useState();
    const [selectedPresentationUuid, setSelectedPresentationUuid] = useState();

    useEffect(() => {
        getPlaylists().then(playlists => {
            setPlaylists(playlists);

            if (playlists?.length > 0) {
                setSelectedPlaylistUuid(playlists[0].uuid);
            }
        }).finally(() => setIsLoading(false));
    }, []);

    useEffect(() => {
        if (playlists) {
            const playlist = playlists.find(playlist => playlist.uuid === selectedPlaylistUuid);
            setPresentations(playlist.presentations);
            
            if (playlist.presentations.length > 0) {
                setSelectedPresentationUuid(playlist.presentations[0].uuid);
            }
        }
    }, [selectedPlaylistUuid]);

    useEffect(() => {
        if (playlists) {
            onSelectPresentation(selectedPresentationUuid);
        }
    }, [selectedPresentationUuid]);

    return (
        <div>
            { isLoading && <div>Loading...</div> }
            { !isLoading && <div className='playlist-panel'>
                <Dropdown
                    value={selectedPlaylistUuid}
                    options={playlists}
                    optionValue="uuid"
                    optionLabel="name"
                    onChange={event => setSelectedPlaylistUuid(event.value)} />

                <Dropdown
                    value={selectedPresentationUuid}
                    options={presentations}
                    optionValue="uuid"
                    optionLabel="name"
                    onChange={event => setSelectedPresentationUuid(event.value)} />
            </div> }
        </div>
    );
}

export default PlaylistPanel;