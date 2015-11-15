import Ember from 'ember';

export default Ember.Component.extend({
	didInsertElement: function(){
		var that = this;
		setTimeout(function(){
				that.$('.mail-details').removeClass('transparent');
	  }, 200);
	}
});