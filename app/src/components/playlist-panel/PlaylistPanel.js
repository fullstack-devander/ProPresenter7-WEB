import './PlaylistPanel.css';
import { useState, useEffect } from 'react';
import { Dropdown } from 'primereact/dropdown';

function PlaylistPanel({ playlists, activePlaylist, activePresentation, onSelectPresentation }) {
    const [presentations, setPresentations] = useState(null);

    useEffect(() => {
        const playlist = playlists.find(playlist => playlist.uuid === activePlaylist);
        setPresentations(playlist.presentations);
    }, []);

    function selectPlaylist(uuid) {
        if (playlists) {
            const playlist = playlists.find(playlist => playlist.uuid === uuid);
            setPresentations(playlist.presentations);
            
            if (playlist.presentations.length > 0) {
                onSelectPresentation(playlist.presentations[0].uuid);
            }
        }
    }

    return (
        <div className='playlist-panel'>
            <Dropdown
                value={activePlaylist}
                options={playlists}
                optionValue="uuid"
                optionLabel="name"
                onChange={event => selectPlaylist(event.value)} />

            <Dropdown
                value={activePresentation}
                options={presentations}
                optionValue="uuid"
                optionLabel="name"
                onChange={event => onSelectPresentation(event.value)} />
        </div>
    );
}

export default PlaylistPanel;