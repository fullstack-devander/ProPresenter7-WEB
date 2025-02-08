// TODO: Think to move all methods into a class

//const apiUrl = import.meta.env.VITE_API_URL;

export async function getPresentationDetails(uuid: any) {
    // TODO: Return inforamtion about a presentation (title, slides).
    throw new Error("Not implemented");
}

export function getThumbnail(presentationUuid: any, slideIndex: any) {
    // TODO: Return image of a slide.
    throw new Error("Not implemented");
}

export async function getActiveSlideIndex() {
    // TODO: Return index of an active slide.
    throw new Error("Not implemented");
}

export async function triggerSlide(presentationUuid: any, slideIndex: any) {
    // TODO: Triger a slide
    throw new Error("Not implemented");
}

export async function triggerNextSlide() {
    // TODO: Trigger next slide.
    throw new Error("Not implemented");
}

export async function triggerPrevSlide() {
    // TODO: Trigger previous slide.
    throw new Error("Not implemented");
}