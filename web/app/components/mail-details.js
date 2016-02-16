import Ember from 'ember';
import DS from 'ember-data';

export default Ember.Component.extend({
	that: null,
	init: function() {
		this._super(...arguments);
		this.that = this;
	},
	actions: {
		delete: function(email) {
			//console.log(sendAction('deleteEmail', email));
			console.log(this.that.sendAction('deleteEmail', email));
		}
	},
	didInsertElement: function(){
		var that = this;
		setTimeout(function(){
				that.$('.mail-details').removeClass('transparent');
	  }, 200);
	}
});