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
    const playlists = await fetch(`${process.env.PROPRESENTER_API_URL}/v1/playlists`, { method: 'GET' });
    res.send(playlists).status(200);
});

app.get('/next', async (req, res) => {
    const url = process.env.PROPRESENTER_API_URL;
    console.log(`The API url (next): ${url}`);
    await fetch(`${url}/v1/trigger/next`, { method: 'GET' });
});

app.get('/prev', async (req, res) => {
    const url = process.env.PROPRESENTER_API_URL;
    console.log(`The API url (prev): ${url}`);
    await fetch(`${url}/v1/trigger/previous`, { method: 'GET' });
});

app.use((err, _req, res, next) => {
    res.status(500).send("Unexpected error occured.");
});

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`);
});