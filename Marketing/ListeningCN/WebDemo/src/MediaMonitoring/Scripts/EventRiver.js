(function (window) {
    var dataConvert = {};

    dataConvert.titleDic = {};

    dataConvert.convertEventSeries = function (data) {
        var colorDict = {
            "中性": '#3898c6',
            "负面": '#a94442',
            "正面": '#00bff3'
        };
        var events = new Array();
        for (var i = 0; i < data.Events.length; i++) {
            var evt = data.Events[i];
            if (this.getDataByName(events, evt.Sentiment) === null) {
                var evtc = {};
                evtc.weight = 123;
                evtc.type = "eventRiver";
                evtc.name = evt.Sentiment;
                evtc.data = [];
                events.push(evtc);
            }

            var es = this.getDataByName(events, evt.Sentiment);

            if (es != null) {
                var ev = this.convertEvent(evt);
                if (ev != null) {
                    es.data.push(ev);
                }
            } else {
                alert("item is null when get data");
            }
        }

        for (var i = 0; i < events.length; i++) {
            var series = events[i];
            series.itemStyle = {};
            series.itemStyle.normal = {};

            series.itemStyle.normal.color = colorDict[series.name];
            series.itemStyle.normal.label = {};
            series.itemStyle.normal.label.textStyle = {};
            var itemStyle = series.itemStyle.normal.label.textStyle;
            itemStyle.fontSize = 15;
            itemStyle.fontFamily = "Microsoft Yahei";
            //itemStyle.fontWeight = "Bold";
            series.itemStyle.emphasis = series.itemStyle.normal;

        }

        return events;
    }

    dataConvert.convertEvent = function (data) {
        var event = {};
        var keywordsAry = data.Keywords.split(" ");
        var keyword = "";
        var append = 0;
        for (var i = 0; i < keywordsAry.length; i++) {
            var word = keywordsAry[i];
            if (word.length >= 2 && word !== data.Company) {
                keyword += word;
                keyword += " ";

                if (typeof (dataConvert.titleDic["【" + data.Company + "】" + keyword]) === 'undefined') {
                    append += 1;
                }
                if (append >= 2) {
                    break;
                }
            }
        }

        event.name = "【" + data.Company + "】" + keyword;
        event.weight = 123;
        event.evolution = [];
        dataConvert.titleDic[event.name] = data.Name;

        for (var i = 0; i < data.DailyEvents.length; i++) {
            var dailyEvent = {};
            var dailyData = data.DailyEvents[i];
            dailyEvent.time = dailyData.Time;
            dailyEvent.value = Math.sqrt(dailyData.Value);
            dailyEvent.detail = {};
            dailyEvent.detail.text = dailyData.Text;
            ////  dailyEvent.detail.link = data.Link;
            ////  dailyEvent.detail.link = data.Link;
            event.evolution.push(dailyEvent);
        }

        return event;
    };

    dataConvert.getDataByName = function (data, name) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].name === name) {
                return data[i];
            }
        }

        return null;
    };

    var eventRiverDrawer = {};
    eventRiverDrawer.drawEventRiver = function (element, data, title, func) {


        var option = {
            colors: [],
            backgroundColor: '#003d73',
            title: {
                text: title,
                textStyle: { color: 'white' }
            },
            tooltip: {
                trigger: 'item',
                enterable: true,
                formatter: function (a, b, c) {
                    var text = dataConvert.titleDic[a.name];
                    text += "<br/><br/>";
                    for (var i = 0; i < a.data.evolution.length; i++) {
                        var e = a.data.evolution[i];
                        if (e.detail.text !== "0") {
                            text += "日期：" + e.time + "            访问量：" + e.detail.text;
                            text += "<br/>";
                        }
                    }
                    return text;
                }
            },
            grid:
            {
                x: 20,
                x2: 20,
                borderWidth: 0,
                borderColor: '#003d73'
            },
            legend: {
                data: [],
                textStyle: { color: 'white' }
            },
            toolbox: {
                //show: true,
                //feature: {
                //    mark: { show: true },
                //    restore: { show: true },
                //    saveAsImage: { show: true }
                //}

            },
            xAxis: [
                {
                    type: 'time',
                    boundaryGap: [0.05, 0.1],
                    axisLine: {
                        lineStyle: {
                            color: '#777'
                        }
                    },
                    nameTextStyle: {
                        color: '#fff',
                        fontSize: 14
                    },
                    axisTick: {
                        lineStyle: {
                            color: '#777'
                        }
                    },
                    axisLabel: {
                        textStyle: {
                            color: '#fff'
                        }
                    }
                }
            ],
            series: [
            ]
        };

        for (var i = 0; i < data.length; i++) {
            option.series.push(data[i]);
            option.legend.data.push(data[i].name);
        }

        var html = JSON.stringify(option);

        var myChart = echarts.init(document.getElementById(element));
        myChart.setOption(option);
        myChart.on("click", func);
    }

    window.eventRiverDrawer = eventRiverDrawer;
    window.dataConvert = dataConvert;
})(typeof (window) !== 'undefined' ? window : this);