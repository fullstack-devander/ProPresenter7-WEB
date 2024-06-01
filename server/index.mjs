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

const staticAppPath = path.join(__dirname, './web');

console.log(staticAppPath);

app.use(express.static(staticAppPath));
app.use(express.json());

app.use((err, _req, res, next) => {
    res.status(500).send("Unexpected error occured.");
});

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`);
});