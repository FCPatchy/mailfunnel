import ModalDialog from 'ember-modal-dialog/components/modal-dialog';

export default ModalDialog.extend({
	showModal: false,
	actions: {
		showModal: function() {
			this.set('showModal', true);
		},
		confirm: function() {
			this.action();
			this.set('showModal', false);
		},
		reject: function() {
			this.set('showModal', false);
		}
	},
	containerClassNames: "prompt-modal",
	overlayClassNames: "prompt-modal-overlay"
});