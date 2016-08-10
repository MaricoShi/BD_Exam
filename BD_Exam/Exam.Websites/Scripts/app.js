﻿(function ($, window, document, undefined) {

    var old = $.fn.twbsPagination;

    var TwbsPagination = function (element, options) {
        this.$element = $(element);
        this.options = $.extend({}, $.fn.twbsPagination.defaults, options);
        this.init(this.options);
    };

    TwbsPagination.prototype = {

        constructor: TwbsPagination,

        init: function (options) {
            this.options = $.extend({}, this.options, options);

            switch (this.options.version) {
                case '1.0':
                    this.currentPages = this.getPages_v_1_0(this.options.startPage);
                    break;
                case '1.1':
                    this.currentPages = this.getPages_v_1_1(this.options.startPage);
                    break;
                default:
                    this.currentPages = this.getPages_v_1_1(this.options.startPage);
                    break;
            }

            if (this.options.startPage < 1 || this.options.startPage > this.options.totalPages) {
                throw new Error('Start page option is incorrect');
            }

            if (this.options.totalPages <= 0) {
                throw new Error('Total pages option cannot be less 1 (one)!');
            }

            if (!$.isNumeric(this.options.visiblePages) && !this.options.visiblePages) {
                this.options.visiblePages = this.options.totalPages;
            }

            if (this.options.onPageClick instanceof Function) {
                this.$element.bind('page', this.options.onPageClick);
            }

            var tagName = (typeof this.$element.prop === 'function') ?
                this.$element.prop('tagName') : this.$element.attr('tagName');

            if (tagName === 'UL') {
                this.$listContainer = this.$element;
            } else {
                this.$listContainer = $('<ul></ul>');
            }

            this.$listContainer.addClass(this.options.paginationClass);

            this.$listContainer.append(this.buildListItems(this.currentPages.numeric));

            if (tagName !== 'UL') {
                this.$element.append(this.$listContainer);
            }

            this.show(this.options.startPage);
        },

        show: function (page) {
            if (page < 1 || page > this.options.totalPages) {
                throw new Error('Page is incorrect.');
            }

            switch (this.options.version) {
                case '1.0':
                    this.render(this.getPages_v_1_0(page));
                    break;
                case '1.1':
                    this.render(this.getPages_v_1_1(page));
                    break;
                default:
                    this.render(this.getPages_v_1_1(page));
                    break;
            }

            this.setupEvents();

            this.$element.trigger('page', page);
        },

        buildListItems: function (pages) {
            var $listItems = $();

            if (this.options.first) {
                $listItems = $listItems.add(this.buildItem('first', 1));
            }

            if (this.options.prev) {
                $listItems = $listItems.add(this.buildItem('prev', 1));
            }

            for (var i = 0; i < pages.length; i++) {
                $listItems = $listItems.add(this.buildItem('page', pages[i]));
            }

            if (this.options.next) {
                $listItems = $listItems.add(this.buildItem('next', 2));
            }

            if (this.options.last) {
                $listItems = $listItems.add(this.buildItem('last', this.options.totalPages));
            }

            return $listItems;
        },

        buildItem: function (type, page) {
            var itemContainer = $('<li></li>'),
                itemContent = $('<a></a>'),
                itemText = null;

            itemContainer.addClass(type);
            itemContainer.attr('data-page', page);

            switch (type) {
                case 'page':
                    itemText = page;
                    break;
                case 'first':
                    itemText = this.options.first;
                    break;
                case 'prev':
                    itemText = this.options.prev;
                    break;
                case 'next':
                    itemText = this.options.next;
                    break;
                case 'last':
                    itemText = this.options.last;
                    break;
                default:
                    break;
            }

            itemContainer.append(itemContent.attr('href', this.href(page)).text(itemText));
            return itemContainer;
        },

        getPages_v_1_0: function (currentPage) {
            var pages = [];

            var startPage;
            var section = parseInt(currentPage / this.options.visiblePages, 10);
            if (currentPage % this.options.visiblePages === 0) {
                startPage = (section - 1) * this.options.visiblePages + 1;
            } else {
                startPage = section * this.options.visiblePages + 1;
            }

            var endPage = Math.min(this.options.totalPages, (startPage + this.options.visiblePages) - 1);
            for (var i = startPage; i <= endPage; i++) {
                pages.push(i);
            }

            return { "currentPage": currentPage, "numeric": pages };
        },

        getPages_v_1_1: function (currentPage) {
            var pages = [];

            var half = Math.floor(this.options.visiblePages / 2);
            var start = currentPage - half + 1 - this.options.visiblePages % 2;
            var end = currentPage + half;

            // handle boundary case
            if (start <= 0) {
                start = 1;
                end = this.options.visiblePages;
            }
            if (end > this.options.totalPages) {
                start = this.options.totalPages - this.options.visiblePages + 1;
                end = this.options.totalPages;
            }

            var itPage = start;
            while (itPage <= end) {
                pages.push(itPage);
                itPage++;
            }

            return { "currentPage": currentPage, "numeric": pages };
        },

        render: function (pages) {
            if (!this.equals(this.currentPages.numeric, pages.numeric)) {
                this.$listContainer.children().remove();
                this.$listContainer.append(this.buildListItems(pages.numeric));
                this.currentPages = pages;
            }

            this.$listContainer.find('.page').removeClass('active');
            this.$listContainer.find('.page').filter('[data-page="' + pages.currentPage + '"]').addClass('active');

            this.$listContainer.find('.first')
                .toggleClass('disabled', pages.currentPage === 1);

            this.$listContainer.find('.last')
                .toggleClass('disabled', pages.currentPage === this.options.totalPages);

            var prev = pages.currentPage - 1;
            this.$listContainer.find('.prev')
                .toggleClass('disabled', pages.currentPage === 1)
                .data('page', prev > 1 ? prev : 1);

            var next = pages.currentPage + 1;
            this.$listContainer.find('.next')
                .toggleClass('disabled', pages.currentPage === this.options.totalPages)
                .data('page', next < this.options.totalPages ? next : this.options.totalPages);
        },

        setupEvents: function () {
            var base = this;
            this.$listContainer.find('li').each(function () {
                var $this = $(this);
                $this.off();
                if ($this.hasClass('disabled') || $this.hasClass('active')) return;
                $this.click(function () {
                    base.show(parseInt($this.data('page'), 10));
                });
            });
        },

        equals: function (oldArray, newArray) {
            var i = 0;
            while ((i < oldArray.length) || (i < newArray.length)) {
                if (oldArray[i] !== newArray[i]) {
                    return false;
                }
                i++;
            }
            return true;
        },

        href: function (c) {
            return this.options.href.replace(this.options.hrefVariable, c);
        }

    };

    $.fn.twbsPagination = function (option) {
        var args = Array.prototype.slice.call(arguments, 1);
        var methodReturn;

        var $set = this.each(function () {
            var $this = $(this);
            var data = $this.data('twbs-pagination');
            var options = typeof option === 'object' && option;

            if (!data) $this.data('twbs-pagination', (data = new TwbsPagination(this, options)));
            if (typeof option === 'string') methodReturn = data[option].apply(data, args);
        });

        return (methodReturn === undefined) ? $set : methodReturn;
    };

    $.fn.twbsPagination.defaults = {
        totalPages: 0,
        startPage: 1,
        visiblePages: 5,
        href: 'javascript:void(0);',
        hrefVariable: '{{number}}',
        first: '首页',
        prev: '上一页',
        next: '下一页',
        last: '尾页',
        paginationClass: 'pagination',
        onPageClick: null,
        version: '1.0'
    };

    $.fn.twbsPagination.Constructor = TwbsPagination;

    $.fn.twbsPagination.noConflict = function () {
        $.fn.twbsPagination = old;
        return this;
    };

})(jQuery, window, document);

(function ($) {

    jQuery.fn.extend({
        slimScroll: function (options) {

            var defaults = {
                width: '220px',

                // height in pixels of the visible scroll area
                height: '250px',

                // width in pixels of the scrollbar and rail
                size: '7px',

                // scrollbar color, accepts any hex/color value
                color: '#FFF',

                // scrollbar position - left/right
                position: 'right',

                // distance in pixels between the side edge and the scrollbar
                distance: '1px',

                // default scroll position on load - top / bottom / $('selector')
                start: 'top',

                // sets scrollbar opacity
                opacity: .4,

                // enables always-on mode for the scrollbar
                alwaysVisible: false,

                // check if we should hide the scrollbar when user is hovering over
                disableFadeOut: false,

                // sets visibility of the rail
                railVisible: false,

                // sets rail color
                railColor: '#333',

                // sets rail opacity
                railOpacity: .2,

                // whether  we should use jQuery UI Draggable to enable bar dragging
                railDraggable: true,

                // defautlt CSS class of the slimscroll rail
                railClass: 'slimScrollRail',

                // defautlt CSS class of the slimscroll bar
                barClass: 'slimScrollBar',

                // defautlt CSS class of the slimscroll wrapper
                wrapperClass: 'slimScrollDiv',

                // check if mousewheel should scroll the window if we reach top/bottom
                allowPageScroll: false,

                // scroll amount applied to each mouse wheel step
                wheelStep: 20,

                // scroll amount applied when user is using gestures
                touchScrollStep: 200,

                // sets border radius
                borderRadius: '7px',

                // sets border radius of the rail
                railBorderRadius: '7px'
            };

            var o = $.extend(defaults, options);

            // do it for every element that matches selector
            this.each(function () {

                var isOverPanel, isOverBar, isDragg, queueHide, touchDif,
					barHeight, percentScroll, lastScroll,
					divS = '<div></div>',
					minBarHeight = 30,
					releaseScroll = false;

                // used in event handlers and for better minification
                var me = $(this);

                // ensure we are not binding it again
                if (me.parent().hasClass(o.wrapperClass)) {
                    // start from last bar position
                    var offset = me.scrollTop();

                    // find bar and rail
                    bar = me.parent().find('.' + o.barClass);
                    rail = me.parent().find('.' + o.railClass);

                    getBarHeight();

                    // check if we should scroll existing instance
                    if ($.isPlainObject(options)) {
                        // Pass height: auto to an existing slimscroll object to force a resize after contents have changed
                        if ('height' in options && options.height == 'auto') {
                            me.parent().css('height', 'auto');
                            me.css('height', 'auto');
                            var height = me.parent().parent().height();
                            me.parent().css('height', height);
                            me.css('height', height);
                        }

                        if ('scrollTo' in options) {
                            // jump to a static point
                            offset = parseInt(o.scrollTo);
                        }
                        else if ('scrollBy' in options) {
                            // jump by value pixels
                            offset += parseInt(o.scrollBy);
                        }
                        else if ('destroy' in options) {
                            // remove slimscroll elements
                            bar.remove();
                            rail.remove();
                            me.unwrap();
                            return;
                        }

                        // scroll content by the given offset
                        scrollContent(offset, false, true);
                    }

                    return;
                }

                // optionally set height to the parent's height
                o.height = (o.height == 'auto') ? me.parent().height() : o.height;

                // wrap content
                var wrapper = $(divS).addClass(o.wrapperClass).css({
                    position: 'relative',
                    overflow: 'hidden',
                    width: o.width,
                    height: o.height
                });

                // update style for the div
                me.css({
                    overflow: 'hidden',
                    width: o.width,
                    height: o.height
                });

                // create scrollbar rail
                var rail = $(divS).addClass(o.railClass).css({
                    width: o.size,
                    height: '100%',
                    position: 'absolute',
                    top: 0,
                    display: (o.alwaysVisible && o.railVisible) ? 'block' : 'none',
                    'border-radius': o.railBorderRadius,
                    background: o.railColor,
                    opacity: o.railOpacity,
                    zIndex: 90
                });

                // create scrollbar
                var bar = $(divS).addClass(o.barClass).css({
                    background: o.color,
                    width: o.size,
                    position: 'absolute',
                    top: 0,
                    opacity: o.opacity,
                    display: o.alwaysVisible ? 'block' : 'none',
                    'border-radius': o.borderRadius,
                    BorderRadius: o.borderRadius,
                    MozBorderRadius: o.borderRadius,
                    WebkitBorderRadius: o.borderRadius,
                    zIndex: 99
                });

                // set position
                var posCss = (o.position == 'right') ? { right: o.distance } : { left: o.distance };
                rail.css(posCss);
                bar.css(posCss);

                // wrap it
                me.wrap(wrapper);

                // append to parent div
                me.parent().append(bar);
                me.parent().append(rail);

                // make it draggable and no longer dependent on the jqueryUI
                if (o.railDraggable) {
                    bar.bind("mousedown", function (e) {
                        var $doc = $(document);
                        isDragg = true;
                        t = parseFloat(bar.css('top'));
                        pageY = e.pageY;
                        $doc.bind("mousemove.slimscroll", function (e) {
                            currTop = t + e.pageY - pageY;
                            bar.css('top', currTop);
                            scrollContent(0, bar.position().top, false);// scroll content
                        });

                        $doc.bind("mouseup.slimscroll", function (e) {
                            isDragg = false; hideBar();
                            $doc.unbind('.slimscroll');
                        });
                        return false;
                    }).bind("selectstart.slimscroll", function (e) {
                        e.stopPropagation();
                        e.preventDefault();
                        return false;
                    });
                }

                // on rail over
                rail.hover(function () {
                    showBar();
                }, function () {
                    hideBar();
                });

                // on bar over
                bar.hover(function () {
                    isOverBar = true;
                }, function () {
                    isOverBar = false;
                });

                // show on parent mouseover
                me.hover(function () {
                    isOverPanel = true;
                    showBar();
                    hideBar();
                }, function () {
                    isOverPanel = false;
                    hideBar();
                });

                // support for mobile
                me.bind('touchstart', function (e, b) {
                    if (e.originalEvent.touches.length) {
                        // record where touch started
                        touchDif = e.originalEvent.touches[0].pageY;
                    }
                });

                me.bind('touchmove', function (e) {
                    // prevent scrolling the page if necessary
                    if (!releaseScroll) {
                        e.originalEvent.preventDefault();
                    }
                    if (e.originalEvent.touches.length) {
                        // see how far user swiped
                        var diff = (touchDif - e.originalEvent.touches[0].pageY) / o.touchScrollStep;
                        // scroll content
                        scrollContent(diff, true);
                        touchDif = e.originalEvent.touches[0].pageY;
                    }
                });

                // set up initial height
                getBarHeight();

                // check start position
                if (o.start === 'bottom') {
                    // scroll content to bottom
                    bar.css({ top: me.outerHeight(true) - bar.outerHeight(true) });
                    scrollContent(0, true);
                }
                else if (o.start !== 'top') {
                    // assume jQuery selector
                    scrollContent($(o.start).position().top, null, true);

                    // make sure bar stays hidden
                    if (!o.alwaysVisible) { bar.hide(); }
                }

                // attach scroll events
                attachWheel();

                function _onWheel(e) {
                    // use mouse wheel only when mouse is over
                    if (!isOverPanel) { return; }

                    var e = e || window.event;

                    var delta = 0;
                    if (e.wheelDelta) { delta = -e.wheelDelta / 120; }
                    if (e.detail) { delta = e.detail / 3; }

                    var target = e.target || e.srcTarget || e.srcElement;
                    if ($(target).closest('.' + o.wrapperClass).is(me.parent())) {
                        // scroll content
                        scrollContent(delta, true);
                    }

                    // stop window scroll
                    if (e.preventDefault && !releaseScroll) { e.preventDefault(); }
                    if (!releaseScroll) { e.returnValue = false; }
                }

                function scrollContent(y, isWheel, isJump) {
                    releaseScroll = false;
                    var delta = y;
                    var maxTop = me.outerHeight(true) - bar.outerHeight(true);

                    if (isWheel) {
                        // move bar with mouse wheel
                        delta = parseInt(bar.css('top')) + y * parseInt(o.wheelStep) / 100 * bar.outerHeight(true);

                        // move bar, make sure it doesn't go out
                        delta = Math.min(Math.max(delta, 0), maxTop);

                        // if scrolling down, make sure a fractional change to the
                        // scroll position isn't rounded away when the scrollbar's CSS is set
                        // this flooring of delta would happened automatically when
                        // bar.css is set below, but we floor here for clarity
                        delta = (y > 0) ? Math.ceil(delta) : Math.floor(delta);

                        // scroll the scrollbar
                        bar.css({ top: delta + 'px' });
                    }

                    // calculate actual scroll amount
                    percentScroll = parseInt(bar.css('top')) / (me.outerHeight(true) - bar.outerHeight(true));
                    delta = percentScroll * (me[0].scrollHeight - me.outerHeight(true));

                    if (isJump) {
                        delta = y;
                        var offsetTop = delta / me[0].scrollHeight * me.outerHeight(true);
                        offsetTop = Math.min(Math.max(offsetTop, 0), maxTop);
                        bar.css({ top: offsetTop + 'px' });
                    }

                    // scroll content
                    me.scrollTop(delta);

                    // fire scrolling event
                    me.trigger('slimscrolling', ~~delta);

                    // ensure bar is visible
                    showBar();

                    // trigger hide when scroll is stopped
                    hideBar();
                }

                function attachWheel() {
                    if (window.addEventListener) {
                        this.addEventListener('DOMMouseScroll', _onWheel, false);
                        this.addEventListener('mousewheel', _onWheel, false);
                        this.addEventListener('MozMousePixelScroll', _onWheel, false);
                    }
                    else {
                        document.attachEvent("onmousewheel", _onWheel)
                    }
                }

                function getBarHeight() {
                    // calculate scrollbar height and make sure it is not too small
                    barHeight = Math.max((me.outerHeight(true) / me[0].scrollHeight) * me.outerHeight(true), minBarHeight);
                    bar.css({ height: barHeight + 'px' });

                    // hide scrollbar if content is not long enough
                    var display = barHeight == me.outerHeight(true) ? 'none' : 'block';
                    bar.css({ display: display });
                }

                function showBar() {
                    // recalculate bar height
                    getBarHeight();
                    clearTimeout(queueHide);

                    // when bar reached top or bottom
                    if (percentScroll == ~~percentScroll) {
                        //release wheel
                        releaseScroll = o.allowPageScroll;

                        // publish approporiate event
                        if (lastScroll != percentScroll) {
                            var msg = (~~percentScroll == 0) ? 'top' : 'bottom';
                            me.trigger('slimscroll', msg);
                        }
                    }
                    else {
                        releaseScroll = false;
                    }
                    lastScroll = percentScroll;

                    // show only when required
                    if (barHeight >= me.outerHeight(true)) {
                        //allow window scroll
                        releaseScroll = true;
                        return;
                    }
                    bar.stop(true, true).fadeIn('fast');
                    if (o.railVisible) { rail.stop(true, true).fadeIn('fast'); }
                }

                function hideBar() {
                    // only hide when options allow it
                    if (!o.alwaysVisible) {
                        queueHide = setTimeout(function () {
                            if (!(o.disableFadeOut && isOverPanel) && !isOverBar && !isDragg) {
                                bar.fadeOut('slow');
                                rail.fadeOut('slow');
                            }
                        }, 1000);
                    }
                }

            });

            // maintain chainability
            return this;
        }
    });

    jQuery.fn.extend({
        slimscroll: jQuery.fn.slimScroll
    });
})(jQuery);

(function ($) {

    jQuery.fn.extend({
        blockUI: function (options) {

            var defaults = {

            };

            var o = $.extend(defaults, options);

            // do it for every element that matches selector
            this.each(function () {
                if ($(this).data("loadid") != undefined)
                {
                    $('#' + $(this).data("loadid"), $(this)).show();
                    return false;
                }
                var isTimeOut = false;
                var timeTick = $.now();
                while (!isTimeOut) {
                    if (timeTick != $.now()) {
                        isTimeOut = true; break;
                    }
                }

                $('<div>', { id: 'loading_' + timeTick }).addClass("myloading")
                    .append(
                        $('<div>').addClass("my_loading_inner")
                            .append($('<div>').addClass("my_loading_icon"))
                            .append($('<span>').text("正在处理中..."))
                    ).appendTo($(this));
                $(this).data("loadid", 'loading_' + timeTick);
            });

            // maintain chainability
            return this;
        },
        unblockUI: function (options) {

            var defaults = {

            };

            var o = $.extend(defaults, options);

            // do it for every element that matches selector
            this.each(function () {

                if ($(this).data("loadid") != undefined) {
                    $('#' + $(this).data("loadid"), $(this)).remove();
                    $(this).removeData("loadid");
                    return false;
                }
            });

            // maintain chainability
            return this;
        }
    });
})(jQuery);













