module.exports = {
  name: 'post-build-hook',

  postBuild: function (results) {
  	var dir = 'dist';
    var fs = this.project.require('ember-cli/node_modules/fs-extra');

    if (!fs.existsSync(dir)) {
    	fs.mkdirSync(dir)
    }
    
    fs.copySync(dir, '../Mailfunnel/bin/Debug/app', {
    	dereference: true,
    	clobber: true
    }, function(err){ console.log(err); });
  }
};
