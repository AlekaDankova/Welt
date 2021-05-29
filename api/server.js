const express = require('express');
const app = express();
const port = 4041;
const Registr = require('./functions/registr');
const Gamer = require('./functions/gamer');
global.DB = require('./db');
app.use(express.text({
    limit: '16mb',
    type: '*/*'
}));
app.listen(port, () => console.log('Welt HTTP-Server. Port: ' + port));
app.all('/*', function (req, res, next) {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Headers', 'X-Requested-With, Content-Type');
    res.header('Access-Control-Allow-Methods', '*');
    next();
});
app.get('/', function (req, res) {
    return res.send({ error: false, message: 'API Welt'});
});
app.get('/registrByName/:name/:pass', Registr.registrByName);
app.get('/getDataByID/:id', Gamer.getDataByID);
app.post('/postData', Gamer.saveData);

app.post('/api/epv', function(req, res){
    let data = JSON.parse(req.body);
    return res.status(200).json(data);
});