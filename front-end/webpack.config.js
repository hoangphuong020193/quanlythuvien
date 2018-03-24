/**
 * @author: @AngularClass
 */

/**
 * Look in ./config folder for webpack.dev.js
 */
switch (process.env.NODE_ENV) {
  case 'production':
    module.exports = require('./config/webpack.prod')({
      env: 'production'
    });
    break;
  case 'test':
    module.exports = require('./config/webpack.test')({
      env: 'test'
    });
    break;
  case 'development':
  default:
    module.exports = require('./config/webpack.dev')({
      env: 'development'
    });
}
