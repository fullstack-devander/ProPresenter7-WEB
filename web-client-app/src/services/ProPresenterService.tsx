import { Presentation, Slide } from '../core';

//const apiUrl = import.meta.env.VITE_API_URL;

export async function getPresentationDetails(): Promise<Presentation> {
    // TODO: Return inforamtion about a presentation (title, slides).
    throw new Error("Not implemented");
}

export function getThumbnail(presentationUuid: any, slideIndex: any): string {
    // TODO: Return image of a slide.
    throw new Error("Not implemented");
}

export async function getActiveSlideIndex(): Promise<Slide> {
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