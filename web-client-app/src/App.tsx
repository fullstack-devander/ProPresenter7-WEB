import './App.css'
import { Button } from 'primereact/button';

function App() {

    console.log(import.meta.env.VITE_API_URL);

    return (
        <>
            <h1>Test Application</h1>
            <Button label="Test" />
        </>
    );
}

export default App;
