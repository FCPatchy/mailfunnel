import Ember from 'ember';
import DS from 'ember-data';

export default DS.RESTAdapter.extend({
  pathForType: function(type) {
    var camelized = Ember.String.camelize(type);
    return camelized;
  },
});
