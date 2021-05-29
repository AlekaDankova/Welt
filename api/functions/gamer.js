module.exports = {
    getDataByID: function(req, res){
        let id = req.params.id;
        if(!id) return res.status(400).json({error: true, message: 'DatenFehlen: Id'});
        DB("SELECT * FROM gamers WHERE id_name=" + id)
            .then(results => {
                if(results.length == 0) {
                    DB("INSERT INTO gamers (id_name, hp, attack, lvl, exp, tree) values (" + id + ", 10, 1, 1, 0, 0)")
                        .then(results => {
                            DB("SELECT * FROM gamers WHERE id=" + results.insertId)
                                .then(results => {
                                    return res.status(200).json(results[0]);
                                })
                                .catch(error => { return res.status(400).json({error: true, data: error}); });
                        })
                        .catch(error => { return res.status(400).json({error: true, data: error}); });
                } else {
                    return res.status(200).json(results[0]);
                }
            })
            .catch(error => { return res.status(400).json({error: true, data: error}); });
    },
    postByID: function (req, res){
        let id_name = req.params.id;
        if(!id_name) return res.status(400).json({error: true, message: 'DatenFehlen: Id'});
        DB("SELECT * FROM gamers WHERE id_name=" + id_name)
            .then(results => {
                if(results.length == 0){
                    DB("INSERT INTO gamers (id_name, hp, attack, lvl, exp, tree) values (" + id_name + ", 10, 1, 1, 0, 0)")
                        .then(results => {
                            DB("SELECT * FROM gamers WHERE id=" + results.insertId)
                                .then(results => {
                                    return res.status(200).json(results[0]);
                                })
                                .catch(error => { return res.status(400).json({error: true, data: error}); });
                        })
                        .catch(error => { return res.status(400).json({error: true, data: error}); });
                } else {
                    return res.status(200).json(results[0]);
                }
            })
            .catch(error => { return res.status(400).json({error: true, data: error}); });
    },
    saveData: function (req, res){
        let gamer = JSON.parse(req.body);
        let id_name = gamer.id_name;
        let hp = gamer.hp;
        let attack = gamer.attack;
        let lvl = gamer.lvl;
        let exp = gamer.exp;
        let tree = gamer.tree;
        if(!id_name || !hp || !attack || !lvl || !exp || !tree) return res.status(400).json({error: true, message: 'DatenFehlen: Gamer'});
        DB("UPDATE gamers SET hp=" + hp + ", attack=" + attack + ", lvl=" + lvl + ", exp=" + exp + ", tree=" + tree + " WHERE id_name=" + id_name)
            .then(results => { return res.status(200).json({ error: false, data: results }); })
            .catch(error => { return res.status(400).json({ error: true, data: error }); });
    }
}