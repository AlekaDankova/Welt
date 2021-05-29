module.exports = {
    registrByName: function(req, res){
        let name = req.params.name;
        let pass = req.params.pass;
        if(!name || !pass) return res.status(400).json({error: true, message: 'DatenFehlen: Name/Pass'});
        DB("SELECT * FROM registr WHERE name='" + name + "'")
            .then(results => {
                if(results.length == 0) {
                    DB("INSERT INTO registr (name, pass) values ('" + name + "', '" + pass + "')")
                        .then(results => {
                            DB("SELECT * FROM registr WHERE id=" + results.insertId)
                                .then(results => {
                                    return res.status(200).json(results[0]);
                                })
                                .catch(error => { return res.status(400).json({error: true, data: error}); });
                        })
                        .catch(error => { return res.status(400).json({error: true, data: error}); });
                } else {
                    if(pass == results[0].pass)
                        return res.status(200).json(results[0]);
                    else 
                        return res.json({error: true, message: 'Falsch Pass'});
                }
            })
            .catch(error => { return res.status(400).json({error: true, data: error}); });
    }
}