import Ember from 'ember';
import config from '../config/environment';

export default Ember.Route.extend({
	model: function() {
		var mails = $.get(config.APP.xhrBaseUrl + 'app/mail');
		return mails;
	}
});
