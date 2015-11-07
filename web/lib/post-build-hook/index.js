module.exports = {
  name: 'post-build-hook',

  postBuild: function (results) {
    var fs = this.project.require('ember-cli/node_modules/fs-extra');
    fs.copySync('dist', '../Mailfunnel/bin/Debug/app', {
    	dereference: true,
    	clobber: true
    }, function(err){ console.log(err); });
  }
};
