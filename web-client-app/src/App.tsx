import './App.css'

function App() {

    fetch('http://localhost:5000/api/presentation')
        .then(res => res.json())
        .then(res => {
            console.log("Data received from server");
            console.log(res);
        });

    return (
        <>
            <h1>Test Application</h1>
        </>
    );
}

export default App;
