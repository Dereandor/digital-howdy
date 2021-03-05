let config;

try {
    config = require("./config/config.json");
    // do stuff
} catch (ex) {
    config = null;
}


let defaultConfig = require("./config.default.json");

if(config==null) {
    global.gConfig = defaultConfig;
} else {
    global.gConfig = config;
}

