import { useState, useEffect } from 'react';
import { Dropdown } from 'primereact/dropdown';

import { getPlaylists } from '../../services/ProPresenterAPIService';

function PlaylistPanel({ onSelectPresentation }) {
    const [playlists, setPlaylists] = useState([]);
    const [selectedPlaylistUuid, setSelectedPlaylistUuid] = useState(null);
    const [presentationList, setPresentationList] = useState([]);
    const [selectedPresentationUuid, setSelectedPresentationUuid] = useState(null);

    useEffect(() => {
        getPlaylists().then(playlists => {
            console.log(playlists);
            
            setPlaylists(playlists);

            const uuid = playlists.length > 0 ? playlists[0].uuid : null;
            setSelectedPlaylist(uuid);
        });
    }, []);

    useEffect(() => {
        setPresentationList(selectedPlaylist.presentations);
        
        const uuid = selectedPlaylist?.presentations.length > 0 ? selectedPlaylist.presentations[0].uuid : null;
        setSelectedPresentation(uuid);
    }, [selectedPlaylistUuid]);

    useEffect(() => {
        onSelectPresentation(selectedPresentationUuid);
    }, [selectedPresentationUuid]);

    return (
        <div>
            <Dropdown
                value={selectedPlaylist}
                options={playlists}
                optionValue="uuid"
                optionLabel="name"
                onChange={event => setSelectedPlaylistUuid(event.value)} />

            <Dropdown
                value={selectedPresentation}
                options={presentationList}
                optionValue="uuid"
                optionLabel="name"
                onChange={event => setSelectedPresentationUuid(event.value)} />
        </div>
    );
}

export default PlaylistPanel;