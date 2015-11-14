import Ember from 'ember';

export default Ember.Controller.extend({
	actions: {
		showEmail: function(emailId) {
			this.transitionTo('mail', emailId);
		}
	}
});
