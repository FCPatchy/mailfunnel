import Ember from 'ember';

export default Ember.Component.extend({
	tagName: 'span',
	
	click() {
		if(typeof(this.action) === 'function')
			this.action();
		else
			this.get('parentView').send(this.action, this.origContext);
	},

	didInsertElement: function(){
		this.$('[data-toggle="tooltip"]').tooltip();
	}
});
