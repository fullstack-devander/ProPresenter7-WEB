import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';
import path from 'path';
import { fileURLToPath } from 'url';

dotenv.config();

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const PORT = process.env.PORT || 3001
const app = express();

app.use(cors({
    origin: [
        process.env.ORIGIN_URL,
    ],
    optionsSuccessStatus: 200,
}));

const staticAppPath = path.join(__dirname, './build');

console.log(staticAppPath);

app.use(express.static(staticAppPath));
app.use(express.json());

app.get('/playlists', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/playlists`;
    const playlists = await fetch(url, { method: 'GET' }).then(res => res.json());
    res.send(playlists).status(200);
});

app.get('/playlist/:uuid', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/playlist/${req.params.uuid}`;
    const playlist = await fetch(url, { method: 'GET' }).then(res => res.json());
    res.send(playlist).status(200);
});

app.get('/presentation/:uuid', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${req.params.uuid}`;
    console.log(url);
    const response = await fetch(url, { method: 'GET' }).then(res => res.json());

    let slideCount = 0;
    response.presentation.groups.forEach(group => {
        group.slides.forEach(() => ++slideCount);
    });

    const presentationRes = {
        uuid: response.presentation.id.uuid,
        name: response.presentation.id.name,
        slideCount: slideCount,
    };

    console.log(presentationRes);

    res.send(presentationRes).status(200);
});

app.get('/presentation/:uuid/thumbnail/:index', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${req.params.uuid}/thumbnail/${req.params.index}`;
    console.log(url);
    const thumbnail = await fetch(url, { method: 'GET' });
    console.log(thumbnail);
    const result = await thumbnail.blob().then(res => res.arrayBuffer());
    console.log(result);

    res.set({
        'content-length': thumbnail.headers.get('content-length'),
        'content-type': thumbnail.headers.get('content-type'),
    });

    res.send(Buffer.from(result)).status(200);
});

app.get('/preview', async (req, res) => {
    const slideIndexUrl = `${process.env.PROPRESENTER_API_URL}/v1/presentation/slide_index`;
    const slideIndexResponse = await fetch(slideIndexUrl, { method: 'GET' });
    const slideIndex = await slideIndexResponse.json();

    console.log(slideIndex);
    const presentationUuid = slideIndex.presentation_index.presentation_id.uuid;
    const cueIndex = slideIndex.presentation_index.index;
    
    const thumbnailUrl = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${presentationUuid}/thumbnail/${cueIndex}`;
    const thumbnailResponse = await fetch(thumbnailUrl, { method: 'GET' });
    const thumbnail = await thumbnailResponse.blob().then(res => res.arrayBuffer());
    
    res.set({
        'content-length': thumbnailResponse.headers.get('content-length'),
        'content-type': thumbnailResponse.headers.get('content-type'),
    });

    res.send(Buffer.from(thumbnail)).status(200);
});

// TODO: Obsolete; /preview should be used instead.
app.get('/presentation/slide_index', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/slide_index`;
    console.log(url);
    const response = await fetch(url, { method: 'GET' });
    const slideIndex = await response.json();

    res.send(slideIndex).status(200);
});

app.get('/next', async (req, res) => {
    console.log(`The API url (next): ${url}`);
    await fetch(`${process.env.PROPRESENTER_API_URL}/v1/trigger/next`, { method: 'GET' });
});

app.get('/prev', async (req, res) => {
    console.log(`The API url (prev): ${url}`);
    await fetch(`${process.env.PROPRESENTER_API_URL}/v1/trigger/previous`, { method: 'GET' });
});

app.use((err, _req, res, next) => {
    res.status(500).send("Unexpected error occured.");
});

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`);
});