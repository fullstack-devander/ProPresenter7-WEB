const apiUrl = 'http://localhost:3001'; // DEBUG
//const apiUrl = ''; // TEST | RELEASE

export async function getPlaylists() {
  return await fetch(`${apiUrl}/playlists`, { method: 'GET' }).then(res => res.json());
}

export async function getPresentationDetails(uuid) {
  return await fetch(`${apiUrl}/presentation/${uuid}`, { method: 'GET' }).then(res => res.json());
}

export async function getActiveSlideIndex() {
  return await fetch(`${apiUrl}/activeSlide`, { method: 'GET' }).then(res => res.json());
}

export async function triggerSlide(presentationUuid, slideIndex) {
  return await fetch(`${apiUrl}/presentation/${presentationUuid}/${slideIndex}/trigger`);
}

export function getThumbnail(presentationUuid, slideIndex) {
  return `${apiUrl}/presentation/${presentationUuid}/thumbnail/${slideIndex}`;
}

export async function triggerNextSlide() {
  return await fetch(`${apiUrl}/trigger/next`, { method: 'GET' });
}

export async function triggerPrevSlide() {
  return await fetch(`${apiUrl}/trigger/prev`, { method: 'GET' });
}