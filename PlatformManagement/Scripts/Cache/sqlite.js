(function(factory) {
    if (typeof define === "function" && define.amd) {
        define(function() {
            return factory();
        });
    } else {
        window.sqlite = factory();
    }
})(function () {
    var sqlite = function (db_name, size) {
        var _db = openDatabase(db_name, '1.0.0', '', size);
        return {
            execute: function (sql, param, callback) {
                //参数处理  
                if (!param) {
                    param = [];
                } else if (typeof param == 'function') {
                    callback = param;
                    param = [];
                }

                this.query(sql, param, function (result) {
                    if (typeof callback == 'function') {
                        callback(result.rowsAffected);
                    }
                });
            },

            query: function (sql, param, callback) {
                //参数处理  
                if (!param) {
                    param = [];
                } else if (typeof param == 'function') {
                    callback = param;
                    param = [];
                }

                var self = this;
                //只有一个参数  
                _db.transaction(function (tx) {
                    //4个参数：sql，替换sql中问号的数组，成功回调，出错回调  
                    tx.executeSql(sql, param, function (tx, result) {
                        if (typeof callback == 'function') {
                            callback(result);
                        }
                    }, self.onfail);
                });
            },
            /** 
             * 插入，回调返回last id 
             * void insert( string, object[, function]) 
           */
            insert: function (table, data, callback) {
                if (typeof data != 'object' && typeof callback == 'function') {
                    callback(0);
                }
                var k = [];
                var v = [];
                var param = [];
                for (var i in data) {
                    k.push(i);
                    v.push('?');
                    param.push(data[i]);
                }
                var sql = "INSERT INTO " + table + " (" + k.join(',') + ") VALUES(" + v.join(',') + ")";
                console.log(sql);
                console.log(param);
                this.query(sql, param, function (result) {
                    if (typeof callback == 'function') {
                        callback(result.insertId);
                    }
                });
            },

            update: function (table, data, where, param, callback) {
                //参数处理  
                if (!param) {
                    param = [];
                } else if (typeof param == 'function') {
                    callback = param;
                    param = [];
                }

                var set_info = this.mkWhere(data);
                for (var i = set_info.param.length - 1; i >= 0; i--) {
                    param.unshift(set_info.param[i]);
                }
                var sql = "UPDATE " + table + " SET " + set_info.sql;
                if (where) {
                    sql += " WHERE " + where;
                }

                this.query(sql, param, function (result) {
                    if (typeof callback == 'function') {
                        callback(result.rowsAffected);
                    }
                });
            },

            toDelete: function (table, where, param, callback) {
                //参数处理  
                if (!param) {
                    param = [];
                } else if (typeof param == 'function') {
                    callback = param;
                    param = [];
                }

                var sql = "DELETE FROM " + table + " WHERE " + where;
                this.query(sql, param, function (result) {
                    if (typeof callback == 'function') {
                        callback(result.rowsAffected);
                    }
                });
            },

            fetchAll: function (sql, param, callback) {
                if (!param) {
                    param = [];
                } else if (typeof param == 'function') {
                    callback = param;
                    param = [];
                }

                this.query(sql, param, function (result) {
                    if (typeof callback == 'function') {
                        var out = [];

                        if (result.rows.length) {
                            for (var i = 0; i < result.rows.length; i++) {
                                out.push(result.rows.item(i));
                            }
                        }

                        callback(out);
                    }
                });
            },

            showTables: function (table_name, callback) {
                this.fetchAll("select * from sqlite_master where type='table' and name like ?", [table_name], callback);
            },

            mkWhere: function (data) {
                var arr = [];
                var param = [];
                if (typeof data === 'object') {
                    for (var i in data) {
                        arr.push(i + "=?");
                        param.push(data[i]);
                        console.log('data.i:' + i);
                    }
                }
                return { sql: arr.join(' AND '), param: param };
            },

            onfail: function (tx, e) {
                console.log('sql error: ' + e.message);
            }
        }
    };
    return new sqlite('localCache', 1024 * 1024 * 2);
});
