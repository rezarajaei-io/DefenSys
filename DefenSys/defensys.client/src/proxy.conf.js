const { env } = require('process');
const port = 7130;// This is Api Server Port, Change It If You Server Port Is Deffrent
const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${port}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7130';

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
