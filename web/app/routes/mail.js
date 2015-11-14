import Ember from 'ember';
import config from '../config/environment';

export default Ember.Route.extend({
	model: function(params) {
		return $.get(config.APP.xhrBaseUrl + 'app/mail/' + params.mail_id);
	},
	showDetails: function() {
		this.controllerFor('application').set('detailsShown', true);
	}.on('activate'),
	hideDetails: function() {
		this.controllerFor('application').set('detailsShown', false);
	}.on('deactivate')
});
