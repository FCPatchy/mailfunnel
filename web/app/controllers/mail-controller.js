import Ember from 'ember';

export default Ember.Controller.extend({
	actions: {
		deleteEmail: function(email){
			alert('hey');
			this.store.findRecord('mail', email.id).then(function(mail){
				mail.destroyRecord();
			})
			console.log('Delete', email);
		}
	}
});
