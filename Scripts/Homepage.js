/// <reference path="_references.js" />
var validationModule = function () {
    //private
    var $errorFields;
    function showTip(field, tip) {
        var fieldName = $(field).parents('.holding').attr('data-fieldname');
        switch (tip) {
            case '.blank':
            case '.invalid':
                $errorFields.push(fieldName);
                break;
            case '.ok':
                if ($errorFields.indexOf(fieldName) >= 0)
                    $errorFields.splice($errorFields.indexOf(fieldName), 1);
        }
        var $sideTips = $(field).siblings('.sidetip');
        $sideTips.children('.active').removeClass('active');
        $sideTips.children(tip).addClass('active');
    }
    function validateField(field, event) {
        var $field = $(field),
            fieldName = $field.parents('.holding').attr('data-fieldname');
        if ($field.val() == '' && event.type != 'loaded')
            showTip(field, '.blank');
        else if ($field.val() != '') {
            var regex;
            switch (fieldName) {
                case 'name':
                    regex = /^([A-Za-z]{3,10})( ([A-Za-z]{0,10})){0,2}$/;
                    break;
                case 'email':
                    regex = /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i;
                    break;
                case 'password':
                    regex = /(.{6,15})/;
                    break;
                case 'username':
                    regex = /^([A-Za-z.0-9]{3,15})$/;
                    break;
                default:
                    return;
            }
            showTip(field, regex.test($field.val()) ? '.ok' : '.invalid');
        }
    }

    //public
    function registrationForm() {
        $errorFields = new Array();
        $('input').on('focus', function () {
            showTip(this, '.tip');
        });
        $('input').on('loaded focusout submitEvent', function (event) {
            validateField(this, event);
        });
        $('.submit').on('click', function (event) {
            $('input').trigger('submitEvent');
            if ($errorFields.length > 0)
                event.preventDefault();
        });
    }
    return {
        registrationForm: registrationForm
    };
}(),
    dataModule = function () {
        //private
        var tweethub = $.connection.tweethub,
            followerhub = $.connection.followerhub;
        followerhub.addSuggestions = function (suggestions) {
            $.each(suggestions, function (i) {
                followerSuggestionViewModelObj.add(suggestions[i]);
            });
        };
        tweethub.showTweet = function (tweet) {
            $('.tweet-form .spinner').hide();
            $('button.tweet-action').removeClass('disabled');
            $('#tweet-box-mini-home-profile').val('');
            effectsModule.clearBox();
            effectsModule.alertMessage("Your Tweet was posted!", 1500);
            tweetViewModelObj.add(tweet);
        };
        tweethub.addAll = function (tweet) {
            tweetViewModelObj.add(tweet);
        };
        function followerSuggestionViewModel() {
            this.suggestions = ko.observableArray();
            this.followingcount = ko.observable(0);
            this.add = function (suggestion) {
                this.suggestions.unshift(suggestion);
            };
            this.follow = function (suggestion, event) {
                followerhub.addFollower(suggestion.UserName);
                $('[data-element-term="following_stats"]>strong').text(parseInt($('[data-element-term="following_stats"]>strong').text()) + 1);
                $(event.srcElement).closest('.user-actions').removeClass('not-following').addClass('following');
            };
        }
        function tweetViewModel() {
            var helpers = {},
                count = ko.observable(0);
            this.tweets = ko.observableArray();
            this.hasTweets = ko.computed(function () {
                return count() > 0;
            });




            this.add = function (tweet) {
                tweet = $.parseJSON(tweet);
                tweet.IsFavorite = ko.observable(tweet.IsFavorite);
                tweet.DateAndTime = helpers.toDate(tweet.DateAndTime);
                this.tweets.unshift(tweet);
                if (tweet.IsMine)
                    count(count() + 1);
                $('[data-element-term="tweet_stats"] strong').text(count());
            },


            this.favorite = function (tweet, event) {
                tweethub.favorite(tweet.Id);
                $(event.srcElement).closest('.tweet').addClass('favorited');
            },
            this.unfavorite = function (tweet, event) {
                tweethub.unFavorite(tweet.Id);
                $(event.srcElement).closest('.tweet').removeClass('favorited');
            },

            this.deletetweet = function (tweet, event) {
                tweethub.deletetweet(tweet.Id);
                $(event.srcElement).closest('.tweet').removeClass('favorited');
                tweetViewModelObj.tweets.remove(tweet);
                count(count() - 1);
                $('[data-element-term="tweet_stats"] strong').text(count());
            },

            helpers.toDate = function (ticks) {
                var d = new Date();
                d.setTime(ticks);
                return prettyDate(d.toISOString());
            };
        }
        //private vars
        var tweetViewModelObj = new tweetViewModel(),
            followerSuggestionViewModelObj = new followerSuggestionViewModel();
        //public
        function initTweets() {
            if($('#tweets').length > 0)
                ko.applyBindings(tweetViewModelObj, $('#tweets').get(0));
            if ($('#empty-timeline-recommendations').length > 0)
                ko.applyBindings(followerSuggestionViewModelObj, $('#empty-timeline-recommendations').get(0));
            else if ($('[data-component-term="similar_user_recommendations"]').length > 0)
                ko.applyBindings(followerSuggestionViewModelObj, $('[data-component-term="similar_user_recommendations"]').get(0));
            $.connection.hub.start(function () {
                tweethub.connected();
                if ($('[data-page="profile"]').length > 0)
                    tweethub.getAll($('[data-user-name]').attr('data-user-name'));
                else
                    tweethub.getAll("");
                followerhub.getSuggestions();
            });
        }
        function newTweet() {
            $('.home-tweet-box .tweet-action').click(function () {
                tweethub.add($('#tweet-box-mini-home-profile').val());
                $('.tweet-form .spinner').css('display', 'inline-block');
                $('button.tweet-action').addClass('disabled');
            });
        }
        return {
            initTweets: initTweets,
            newTweet: newTweet,
        };
    }(),
    effectsModule = function () {
        //private
        function scroller() {
            $('.scroller').on('click', function () {
                if ($(this).css('height') != '300px')
                    $(this).animate({
                        height: '300px',
                    }, 1500);
                else {
                    $(this).animate({
                        height: '68px',
                    }, 1500);
                }
            });
        }
        function imageChanger() {
            var frontpagerandomclasses = new Array(
                'front-random-image-cricket',
                'front-random-image-jp-mountain',
                'front-random-image-city-balcony',
                'front-image-nascar');
            $('body').addClass('logged-out ms-windows webkit front-page ' +
                frontpagerandomclasses[Math.ceil(Math.random() * frontpagerandomclasses.length)]);
        }
        function togglePlaceHolder(elem, className) {
            var $this = $(elem),
                $holding_input = $this.parent();
            if ($this.val() != '' && !$holding_input.hasClass(className))
                $holding_input.addClass(className);
            if ($this.val() == '')
                $holding_input.removeClass(className);
        };
        function addBackground() {
            $('[data-background]').attr('style', 'background-image:url(' + $('body').attr('data-background') + ');');
        }
        function clearBox() {
            if (!$('#tweet-box-mini-home-profile').val() == '')
                return;
            $('.tweet-form').addClass('condensed');
            $('#tweet-box-mini-home-profile').val('Compose new Tweet...');
        }
        function sidebarTweetBox() {
            $('#tweet-box-mini-home-profile').click(function () {
                if (!$(this).closest('form').hasClass('condensed'))
                    return;
                $(this).val('');
                $(this).closest('form').removeClass('condensed');
            });
            $('.home-tweet-box').focusout(clearBox);

            var defaultMessage = "Compose a New Tweet...";
            function tweetBoxViewModel() {
                this.message = ko.observable(defaultMessage);
                this.tweetcount = ko.computed(function () {
                    if (this.message() != defaultMessage)
                        return 140 - this.message().length;
                    else {
                        return 140;
                    }
                }, this);
                this.tweetsCount = ko.observable(0);
                this.warning = ko.computed(function () {
                    var count = this.tweetcount();
                    if (count > 10) return 'tweet-counter';
                    else if (count <= 10 && count >= 0) return "tweet-counter warn";
                    else return "tweet-counter superwarn";
                }, this);
                this.btnclass = ko.computed(function () {
                    var count = this.tweetcount();
                    if (count < 0 || count >= 140)
                        return "btn primary-btn tweet-action disabled";
                    return "btn primary-btn tweet-action";
                }, this);
            }
            if ($('.home-tweet-box').length > 0) {
                var tweetBoxViewModelObj = new tweetBoxViewModel();
                ko.applyBindings(tweetBoxViewModelObj, $('.home-tweet-box').get(0));
            }
        }
        //public
        function registrationForm() {
            scroller();
            $('.holding input').on('keyup loaded', function () {
                togglePlaceHolder(this, 'has-content');
            });
        }
        function homePage() {
            imageChanger();
            $('.placeholding-input input').on('keyup', function () {
                togglePlaceHolder(this, 'hasome');
            });
        }
        function loginForm() {
            if ($('.alert-messages .message-text').text() != '')
                $('.alert-messages').removeClass('hidden');
            $('.holding input').on('keyup loaded focusout', function () {
                togglePlaceHolder(this, 'hasome');
            });
            $('.alert-messages').on('click', 'a.dismiss', function () {
                $('.alert-messages').addClass('hidden');
            });
        }
        function profilePage() {
            $('[data-nav="logout"]').click(function () {
                window.location = '/Account/SignOut';
            });
            if ($('[data-profile-nav]').length > 0)
                $('[data-nav="' + $('[data-profile-nav]').attr('data-profile-nav').toLowerCase() + '"]').closest('li').addClass('active');
            $('#user-dropdown').click(function () {
                if ($(this).hasClass('open'))
                    $(this).removeClass('open');
                else {
                    $(this).addClass('open');
                }
            });
            addBackground();
            sidebarTweetBox();
        }
        function alertMessage(message, secs) {
            $('.message-inside .message-text').text(message);
            $('.alert-messages').removeClass('hidden');
            if (secs)
                setInterval(function () {
                    $('.alert-messages').addClass('hidden');
                }, secs);
        }
        return {
            registrationForm: registrationForm,
            homePage: homePage,
            loginForm: loginForm,
            profilePage: profilePage,
            clearBox: clearBox,
            alertMessage: alertMessage,
        };
    }();

(function ($) {
    switch ($('section').attr('id')) {
        case 'Home':
            if ($('body.logged-out').length > 0)
                effectsModule.homePage();
            else {
                effectsModule.profilePage();
                dataModule.initTweets();
                dataModule.newTweet();
            }
            break;
        case 'Register':
            effectsModule.registrationForm();
            validationModule.registrationForm();
            break;
        case 'Login':
            effectsModule.loginForm();
            break;
    }
    $('input').trigger('loaded', [true]);
})(jQuery);
