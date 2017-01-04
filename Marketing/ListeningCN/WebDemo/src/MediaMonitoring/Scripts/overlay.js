function toggleOverlay(overlayDiv, overlayContainerDiv, showcontenttype, triggerElement, data) {
    var body = $('body'),
        overlay = $(overlayDiv),
        overlayContainer = $(overlayContainerDiv),
        communicationContainer = $('.communication-content');
    transEndEventNames = {
        'WebkitTransition': 'webkitTransitionEnd',
        'MozTransition': 'transitionend',
        'OTransition': 'oTransitionEnd',
        'msTransition': 'MSTransitionEnd',
        'transition': 'transitionend'
    },
    transEndEventName = transEndEventNames[Modernizr.prefixed('transition')],
    support = { transitions: Modernizr.csstransitions };
    if (overlay.hasClass('open')) {
        overlay.removeClass('open');
        body.removeClass('disable-scrollbar');
        body.removeClass( 'overlay-open');
        overlayContainer.removeClass('visible');        
        var onEndTransitionFn = function (ev) {
            if (support.transitions) {
                if (ev.propertyName !== 'visibility') return;
                overlay.off(transEndEventName, onEndTransitionFn);
            }
            overlay.removeClass('close');
        };
        if (support.transitions) {
            overlay.on(transEndEventName, onEndTransitionFn);
        }
        else {
            onEndTransitionFn();
        }
         overlayContainer.removeClass('visible');
    }
    else {
        if (showcontenttype == 'weibo')
        {
            ShowWeiboOverlayConent(triggerElement.getAttribute('data-toggle-weiboId'))
            $("#WeiboList").show(1000);
        }
        else if (showcontenttype == 'newslist')
        {
            ShowNewListOverlayContent(data);
            $("#NewsList").show(1000);
        }
        else if (showcontenttype == 'sentimentNewslist')
        {
            ShowSentimentNewsOverlayContent(data);
            $("#NewsList").show(1000);
        }
        else if (showcontenttype == 'commentsentiments')
        {
            ShowCommentsSentimentOverlayContent(data);
            $("#NewsList").show(1000);
        }

       
        overlay.addClass('open');
        body.addClass('disable-scrollbar');
        body.addClass('overlay-open');
        overlayContainer.addClass('visible');
        
        communicationContainer.addClass('hidden');
    }
}

function ShowNewListOverlayContent(data)
{
    var $$overlayDetailNewsList = $('#js-hr-overlay-NewsList');
    if (data)
    {
        var TemplateSource = $('#overlay-newslist-content-template').html();
        var Template = Handlebars.compile(TemplateSource);
        $$overlayDetailNewsList.html(Template(data));
    }
}

function ShowSentimentNewsOverlayContent(data) {
    var $$overlayDetailNewsList = $('#js-hr-overlay-NewsList');
    if (data) {
        var TemplateSource = $('#overlay-newssentimentlist-content-template').html();
        var Template = Handlebars.compile(TemplateSource);
        $$overlayDetailNewsList.html(Template(data));
    }
}

function ShowCommentsSentimentOverlayContent(data) {
    var $$overlayDetailNewsList = $('#js-hr-overlay-NewsList');
    if (data) {
        var TemplateSource = $('#overlay-commentsentimentlist-content-template').html();
        var Template = Handlebars.compile(TemplateSource);
        $$overlayDetailNewsList.html(Template(data));
    }
}


function ShowWeiboOverlayConent(id)
{
    var $$overlayDetailArea = $('#js-hr-overlay-detail');
    $.get("/api/Scan/GetWeiboDetail/", { id: id }).success(function (data) {
        if (data)
        {
            var overlayDetailSource = $('#overlay-content-template').html();
            var overlayDetailTemplate = Handlebars.compile(overlayDetailSource);
            $$overlayDetailArea.html(overlayDetailTemplate(data));
        }
    })
}

function ShowCommunicationContent()
{
    $(".js-hidden-overlay").hide(1000);
    $(".js-communication").removeClass('hidden').show(1000);
}