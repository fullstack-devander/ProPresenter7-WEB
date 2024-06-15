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
    const result = await thumbnail.blob().then(res => res.arrayBuffer());

    res.set({
        'content-length': thumbnail.headers.get('content-length'),
        'content-type': thumbnail.headers.get('content-type'),
    });

    res.send(Buffer.from(result)).status(200);
});

app.get('/activeSlide', async (req, res) => {
    const slideIndexUrl = `${process.env.PROPRESENTER_API_URL}/v1/presentation/slide_index`;
    const slideIndexResponse = await fetch(slideIndexUrl, { method: 'GET' });
    const slideIndex = await slideIndexResponse.json();

    const result = {
        presentationUuid: slideIndex.presentation_index.presentation_id.uuid,
        slideIndex: slideIndex.presentation_index.index,
    };

    console.log(result);

    res.send(result).status(200);
});

app.get('/presentation/:uuid/:index/trigger', async (req, res) => {
    const uuid = req.params.uuid;
    const index = req.params.index;
    
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${uuid}/${index}/trigger`;
    console.log(url);

    await fetch(url, { method: 'GET' });

    res.sendStatus(200);
});

app.get('/trigger/next', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/trigger/next`;
    console.log(url);
    
    await fetch(url, { method: 'GET' });
});

app.get('/trigger/prev', async (req, res) => {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/trigger/previous`;
    console.log(`The API url (prev): ${url}`);

    await fetch(url, { method: 'GET' });
});

app.use((err, _req, res, next) => {
    res.status(500).send("Unexpected error occured.");
});

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`);
});