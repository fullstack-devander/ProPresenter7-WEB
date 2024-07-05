const apiUrl = 'http://localhost:3001'; // DEBUG
//const apiUrl = ''; // TEST | RELEASE

export async function getPlaylists() {
    return await fetch(`${apiUrl}/playlists`, { method: 'GET' }).then(res => res.json());
}

export async function getPresentationDetails(uuid) {
    return await fetch(`${apiUrl}/presentation/${uuid}`, { method: 'GET' }).then(res => res.json());
}