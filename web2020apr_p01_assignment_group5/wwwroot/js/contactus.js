(function ($) {
    "use strict";


    /*==================================================================
    [ Focus Contact2 ]*/
    $('.input100').each(function(){
        $(this).on('blur', function(){
            if($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })    
    })


    /*==================================================================
    [ Validate ]*/
    var firstname = $('.validate-input input[name="firstname"]');
    var lastname = $('.validate-input input[name="lastname"]');
    var email = $('.validate-input input[name="email"]');
    var number = $('.validate-input input[name="tel"]');

    $('.validate-form').on('submit',function(){
        var check = true;

        if($(firstname).val().trim() == ''){
            showValidate(firstname);
            check=false;
        }

        if($(lastname).val().trim() == ''){
            showValidate(lastname);
            check=false;
        }


        if($(email).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
            showValidate(email);
            check=false;
        }

        if($(number).val().trim() == ''){
            showValidate(number);
            check=false;
        }

        return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
        hideValidate(this);
    });
    });

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    
    

})(jQuery);