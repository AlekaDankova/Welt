const mysql = require('mysql');
const dbConn = mysql.createConnection({
    host: 'localhost',
    //user: 'root1',
    //password: '123q#!qw',
    user: 'root',
    password: '',
    database: 'welt'
});
dbConn.connect();
const DB = (query) => {
    return new Promise((resolve, reject) => {
        dbConn.query(query, function(error, results, fields){
            if (error){
                reject(error);
                return;
            }
            resolve(results);
        });
    });
}
module.exports = DB;