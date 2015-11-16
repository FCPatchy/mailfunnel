import Ember from 'ember';
import config from '../config/environment';

export default Ember.Route.extend({
	model: function() {
		return Ember.RSVP.hash({
			mail: $.get(config.APP.xhrBaseUrl + 'app/mail'),
			groups: $.get(config.APP.xhrBaseUrl + 'app/groups')
		});
	}
});
