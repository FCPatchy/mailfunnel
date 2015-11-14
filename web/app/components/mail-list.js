import Ember from 'ember';

export default Ember.Component.extend({
	actions: {
		select: function(email) {
			this.sendAction('showEmail', email.__id);
		}
	}
});
