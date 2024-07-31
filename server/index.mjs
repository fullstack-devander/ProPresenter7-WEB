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

app.use(express.static(staticAppPath));
app.use(express.json());

app.get('/playlists', async (req, res) => {
  try {
    const responseModels = [];

    const playlistUrl = `${process.env.PROPRESENTER_API_URL}/v1/playlists`;

    const receivedPlaylists = await fetch(playlistUrl, { method: 'GET' }).then(
      response => response.json()).then(
        playlists => playlists.filter(
          playlist => playlist.field_type == 'playlist'));

    for (let index in receivedPlaylists) {
      const playlist = receivedPlaylists[index];
      const presentationUri = `${process.env.PROPRESENTER_API_URL}/v1/playlist/${playlist.id.uuid}`;
      const receivedPresentation = await fetch(presentationUri, { method: 'GET' }).then(
        response => response.json());

      responseModels.push({
        uuid: playlist.id.uuid,
        name: playlist.id.name,
        presentations: receivedPresentation.items.map(presentation => ({
          uuid: presentation.id.uuid,
          name: presentation.id.name,
        })),
      });
    }

    res.status(200).send(responseModels);
  }
  catch (ex) {
    console.error(`Playlists error: ${ex.message}`);
    res.status(500).send('Internal server error');
  }
});

app.get('/presentation/:uuid', async (req, res) => {
  try {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${req.params.uuid}`;
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
  
    res.status(200).send(presentationRes);
  } catch (ex) {
    console.error(`Presentation Details error: ${ex.message}`);
    resizeBy.status(500).send('Internal server error');
  }
  
});

app.get('/presentation/:uuid/thumbnail/:index', async (req, res) => {
  try {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${req.params.uuid}/thumbnail/${req.params.index}`;
    const thumbnail = await fetch(url, { method: 'GET' });
    const result = await thumbnail.blob().then(res => res.arrayBuffer());
  
    res.set({
      'content-length': thumbnail.headers.get('content-length'),
      'content-type': thumbnail.headers.get('content-type'),
    });
  
    res.status(200).send(Buffer.from(result));
  } catch (ex) {
    console.error(`Presentation Thumbnail error: ${ex.message}`);
    res.status(500).send('Internal server error');
  }
});

app.get('/activeSlide', async (req, res) => {
  try {
    const slideIndexUrl = `${process.env.PROPRESENTER_API_URL}/v1/presentation/slide_index`;
    const slideIndexResponse = await fetch(slideIndexUrl, { method: 'GET' });
    const slideIndex = await slideIndexResponse.json();
  
    const result = {
      presentationUuid: slideIndex.presentation_index.presentation_id.uuid,
      slideIndex: slideIndex.presentation_index.index,
    };
  
    res.status(200).send(result);
  } catch (ex) {
    console.error(`Active Slide error: ${ex.message}`);
    res.status(500).send(ex.message);
  }
  
});

app.get('/presentation/:uuid/:index/trigger', async (req, res) => {
  try {
    const uuid = req.params.uuid;
    const index = req.params.index;

    const url = `${process.env.PROPRESENTER_API_URL}/v1/presentation/${uuid}/${index}/trigger`;
    await fetch(url, { method: 'GET' });

    res.sendStatus(200);
  } catch (ex) {
    console.error(`Trigger Slide error: ${ex.message}`);
    res.status(500).send('Internal server error');
  }
});

app.get('/trigger/next', async (req, res) => {
  try {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/trigger/next`;
    await fetch(url, { method: 'GET' });

    res.sendStatus(200);
  } catch (ex) {
    console.error(`Trigger next slide error: ${ex.message}`);
    res.status(500).send('Internal server error');
  }
});

app.get('/trigger/prev', async (req, res) => {
  try {
    const url = `${process.env.PROPRESENTER_API_URL}/v1/trigger/previous`;
    await fetch(url, { method: 'GET' });

    res.sendStatus(200);
  } catch (ex) {
    console.error(`Trigger prev slide error: ${ex.message}`);
    res.status(500).send('Internal server error');
  }
});

app.use((err, _req, res, next) => {
  res.status(500).send("Unexpected error occured.");
});

app.listen(PORT, () => {
  console.log(`Server listening on port ${PORT}`);
});