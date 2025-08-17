const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7001';

const PROXY_CONFIG = [
  {
    context: [
      "/api", // Map bff api (local api)
      "/bff", // Map /login and /logout, etc...
      "/remote" // Map resource server api (remote api)
    ],
    target,
    secure: false,
    changeOrigin: true,
    logLevel: 'debug'
  }
]

module.exports = PROXY_CONFIG;
