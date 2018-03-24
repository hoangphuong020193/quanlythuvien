# Install a type definition package (Example: Jquery)

* npm install --save jquery
* npm install --save-dev @types/jquery
* In webpack config files:  plugins: [
    new ProvidePlugin({
      jQuery: 'jquery',
      $: 'jquery',
      jquery: 'jquery'
    })
  ]
* import $ from 'jquery'
* References: https://github.com/AngularClass/angular2-webpack-starter/wiki/How-to-include-jQuery

# Lint Typescripts
* tslint -c tslint.json src/**/*.ts (somehow 'npm run lint' does not show errors)