/// <reference path="jquery-1.4.1.min.js" />
/// <reference path="jquery.growl.js" />

$(function() {

    $('.focus').focus();

})

/******************************************************************/
/******************************************************************/

$.growl.settings.noticeTemplate =
   '<div class="growlNotice">' +
   '  <h3>' +
   '     <a rel="close">' +
   '        <img alt="" src="%image%" />' +
   '        %title%' +
   '     </a>' +
   '  </h3>' +
   '  <p>%message%</p>' +
   '</div>';
$.growl.settings.noticeCss = {};
$.growl.settings.displayTimeout = 5000;
$.growl.settings.defaultImage = webRoot + 'content/images/info.gif';
$.growl.settings.errorImage = webRoot + 'content/images/error.gif';

/******************************************************************/
/******************************************************************/

sessionAdminBox = {

    beginPost: function() {
        $('form#postComment').validate({ errorClass: 'input-validation-error', errorPlacement: function(error, element) { } });
        return $('form#postComment').validate().form();
    },

    commentPosted: function() { $('form#postComment textarea').val(''); }

};
