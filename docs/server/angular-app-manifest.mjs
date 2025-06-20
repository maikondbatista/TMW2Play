
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/TMW2Play/',
  locale: undefined,
  routes: [
  {
    "renderMode": 0,
    "preload": [
      "chunk-PS2VPN6F.js",
      "chunk-TN3BWXZF.js"
    ],
    "route": "/TMW2Play/Home"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-Q4YCYZRK.js",
      "chunk-TN3BWXZF.js"
    ],
    "route": "/TMW2Play/Profile/*"
  },
  {
    "renderMode": 0,
    "redirectTo": "/TMW2Play/Home",
    "route": "/TMW2Play/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 5454, hash: '2bd8c7412cd247abf36092ff80d8f75e2e2206c2d60fe26161e13886db621f63', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1077, hash: '4980ba3d4bd36f450b0b24ec728f34edafab444fad2ea0eb117929a062574547', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-SWR3O4VE.css': {size: 227146, hash: '0UxJSJhtgOo', text: () => import('./assets-chunks/styles-SWR3O4VE_css.mjs').then(m => m.default)}
  },
};
