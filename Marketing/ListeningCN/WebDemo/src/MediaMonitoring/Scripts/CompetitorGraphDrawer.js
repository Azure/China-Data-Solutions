(function (window) {
    var mapDrawer = {};


    mapDrawer.colorDic = {
        '#00aeef': ["#0055c2", "#80ffff"],
        '#f26522': ["#b53b00", "#fec6ab"],
        '#ffff00': ["#b98005", "#fffbb1"],
        '#39b54a': ["#078b19", "#b2ffbc"],
        '#b6b2b2': ["#6b6b6b", "#f0f0f0"]
    };

    mapDrawer.provinceDict = [
        { name: '安徽', en: 'Anhui', capital: '安徽' },
        { name: '北京', en: 'Beijing', capital: '北京' },
        { name: '重庆', en: 'Chongqing', capital: '重庆' },
        { name: '福建', en: 'Fujian', capital: '福建' },
        { name: '甘肃', en: 'Gansu', capital: '甘肃' },
        { name: '广东', en: 'Guangdong', capital: '广东' },
        { name: '广西', en: 'Guangxi', capital: '广西' },
        { name: '贵州', en: 'Guizhou', capital: '贵州' },
        { name: '海南', en: 'Hainan', capital: '海南' },
        { name: '河北', en: 'Hebei', capital: '河北' },
        { name: '黑龙江', en: 'Heilongjiang', capital: '黑龙江' },
        { name: '河南', en: 'Henan', capital: '河南' },
        { name: '湖北', en: 'Hubei', capital: '湖北' },
        { name: '湖南', en: 'Hunan', capital: '湖南' },
        { name: '江苏', en: 'Jiangsu', capital: '江苏' },
        { name: '江西', en: 'Jiangxi', capital: '江西' },
        { name: '吉林', en: 'Jilin', capital: '吉林' },
        { name: '辽宁', en: 'Liaoning', capital: '辽宁' },
        { name: '宁夏', en: 'Ningxia', capital: '宁夏' },
        { name: '内蒙古', en: 'NeiMengGu', capital: '内蒙古' },
        { name: '青海', en: 'Qinghai', capital: '青海' },
        { name: '陕西', en: 'Shaanxi', capital: '陕西' },
        { name: '山东', en: 'Shandong', capital: '山东' },
        { name: '上海', en: 'Shanghai', capital: '上海' },
        { name: '山西', en: 'Shanxi', capital: '山西' },
        { name: '四川', en: 'Sichuan', capital: '四川' },
        { name: '天津', en: 'Tianjin', capital: '天津' },
        { name: '西藏', en: 'Xizang', capital: '西藏' },
        { name: '新疆', en: 'Xinjiang', capital: '新疆' },
        { name: '云南', en: 'Yunnan', capital: '云南' },
        { name: '浙江', en: 'Zhejiang', capital: '浙江' }
    ];

    mapDrawer.cityGeos = {
        "海门": [121.1882240000, 31.8778570000],
        "鄂尔多斯": [109.7873140000, 39.6146300000],
        "招远": [120.4408110000, 37.3610500000],
        "舟山": [122.2143390000, 29.9910920000],
        "齐齐哈尔": [123.9245310000, 47.3600870000],
        "盐城": [120.1681870000, 33.3553010000],
        "赤峰": [118.8954630000, 42.2645860000],
        "青岛": [120.3894450000, 36.0723580000],
        "乳山": [121.5466270000, 36.9263900000],
        "金昌": [102.1941970000, 38.5257770000],
        "泉州": [118.6823160000, 24.8802420000],
        "莱西": [120.4428310000, 36.8636370000],
        "日照": [119.5336060000, 35.4227980000],
        "胶南": [119.8563100000, 35.8528580000],
        "南通": [120.9003010000, 31.9852370000],
        "西藏": [91.1210250000, 29.6500880000],
        "云浮": [112.0510450000, 22.9211540000],
        "梅州": [116.1291790000, 24.2943110000],
        "文登": [122.0107820000, 37.1541200000],
        "上海": [121.4802370000, 31.2363050000],
        "攀枝花": [101.7252620000, 26.5881090000],
        "威海": [122.1282450000, 37.5193220000],
        "承德": [117.9697980000, 40.9578550000],
        "厦门": [118.0959150000, 24.4858210000],
        "汕尾": [115.3816930000, 22.7913220000],
        "潮州": [116.6294300000, 23.6629230000],
        "丹东": [124.3385430000, 40.1290230000],
        "太仓": [121.1363470000, 31.4648020000],
        "曲靖": [103.8026850000, 25.4963280000],
        "烟台": [121.4544250000, 37.4698680000],
        "福建": [119.3029380000, 26.0804470000],
        "瓦房店": [121.9865880000, 39.6320800000],
        "即墨": [120.4536850000, 36.3952720000],
        "抚顺": [123.9635950000, 41.8860780000],
        "玉溪": [102.5537000000, 24.3575120000],
        "张家口": [114.8941650000, 40.8301720000],
        "阳泉": [113.5870870000, 37.8623400000],
        "莱州": [119.9487630000, 37.1826570000],
        "湖州": [120.0945660000, 30.8990150000],
        "汕头": [116.6887390000, 23.3592890000],
        "昆山": [120.9882660000, 31.3909000000],
        "宁波": [121.5566860000, 29.8801770000],
        "湛江": [110.3654940000, 21.2771630000],
        "揭阳": [116.3792200000, 23.5557730000],
        "荣成": [122.4927830000, 37.1711530000],
        "连云港": [119.2295710000, 34.6023420000],
        "葫芦岛": [120.8433880000, 40.7173640000],
        "常熟": [120.7588630000, 31.6597700000],
        "东莞": [113.7582310000, 23.0269970000],
        "河源": [114.7070970000, 23.7498290000],
        "淮安": [119.0224290000, 33.6162720000],
        "泰州": [119.9321150000, 32.4612000000],
        "广西": [108.3733510000, 22.8230370000],
        "营口": [122.2414750000, 40.6725650000],
        "惠州": [114.4233480000, 23.1164090000],
        "江阴": [120.2919690000, 31.9257900000],
        "蓬莱": [120.0919170000, 31.9294710000],
        "韶关": [113.6037570000, 24.8161740000],
        "嘉峪关": [98.2965140000, 39.7782680000],
        "广东": [113.6907000000, 23.3053080000],
        "延安": [109.4963610000, 36.5910030000],
        "山西": [112.5570600000, 37.8768850000],
        "清远": [113.0626190000, 23.6882380000],
        "中山": [113.3990230000, 22.5222620000],
        "云南": [102.8396670000, 24.8859530000],
        "寿光": [118.7973950000, 36.8617320000],
        "盘锦": [122.0772690000, 41.1259390000],
        "长治": [113.1230460000, 36.2015850000],
        "深圳": [114.0661120000, 22.5485150000],
        "珠海": [113.5832350000, 22.2763920000],
        "宿迁": [118.2820620000, 33.9676860000],
        "咸阳": [108.7157120000, 34.3355990000],
        "铜川": [108.9515580000, 34.9029570000],
        "平度": [119.9664890000, 36.7925170000],
        "佛山": [113.1284320000, 23.0277070000],
        "海南": [110.2064240000, 20.0500570000],
        "江门": [113.0881650000, 22.5844590000],
        "章丘": [117.5323440000, 36.6854150000],
        "肇庆": [112.4717700000, 23.0529840000],
        "大连": [121.6213910000, 38.9193450000],
        "临汾": [111.5261530000, 36.0940520000],
        "吴江": [120.6394020000, 31.0148470000],
        "石嘴山": [106.3907800000, 38.9897830000],
        "辽宁": [123.4389730000, 41.8113390000],
        "苏州": [120.5896130000, 31.3045660000],
        "茂名": [110.9317730000, 21.6690510000],
        "嘉兴": [120.7620450000, 30.7509120000],
        "长春": [125.3301700000, 43.8217800000],
        "胶州": [120.0400780000, 36.2703890000],
        "宁夏": [106.2389760000, 38.4923920000],
        "张家港": [120.5620520000, 31.8812170000],
        "三门峡": [111.2068320000, 34.7784420000],
        "锦州": [121.1336310000, 41.1008690000],
        "江西": [115.8645280000, 28.6876750000],
        "柳州": [109.4219800000, 24.3315190000],
        "三亚": [109.5186460000, 18.2582170000],
        "自贡": [104.7848910000, 29.3453790000],
        "吉林": [126.5560730000, 43.8435120000],
        "阳江": [111.9890510000, 21.8644210000],
        "泸州": [105.4490920000, 28.8775770000],
        "青海": [101.7842690000, 36.6234770000],
        "宜宾": [104.6481030000, 28.7576100000],
        "内蒙古": [111.7585180000, 40.8474610000],
        "四川": [104.0712160000, 30.5762790000],
        "大同": [113.3064460000, 40.0825390000],
        "镇江": [119.4314940000, 32.1956880000],
        "桂林": [110.2964420000, 25.2798930000],
        "张家界": [110.4849250000, 29.1224770000],
        "宜兴": [119.8300690000, 31.3460710000],
        "北海": [109.1266140000, 21.4869550000],
        "陕西": [108.9463060000, 34.3474360000],
        "金坛": [119.6045110000, 31.7290120000],
        "东营": [118.6810460000, 37.4399900000],
        "牡丹江": [129.6389760000, 44.5586470000],
        "遵义": [106.9336580000, 27.7317490000],
        "绍兴": [120.5866730000, 30.0365190000],
        "扬州": [119.4191070000, 32.3998600000],
        "常州": [119.9801420000, 31.8167910000],
        "潍坊": [119.1681380000, 36.7132120000],
        "重庆": [106.5571650000, 29.5709970000],
        "台州": [121.4269960000, 28.6622970000],
        "江苏": [119.7028910000, 33.0647350000],
        "滨州": [117.9792000000, 37.3883870000],
        "贵州": [106.6368160000, 26.6527470000],
        "无锡": [120.3189540000, 31.4967040000],
        "本溪": [123.7734680000, 41.2998470000],
        "克拉玛依": [84.8958700000, 45.5857650000],
        "渭南": [109.4839330000, 34.5023580000],
        "马鞍山": [118.5126910000, 31.6763300000],
        "宝鸡": [107.2438990000, 34.3677470000],
        "焦作": [113.2485570000, 35.2214930000],
        "句容": [119.1750720000, 31.9511470000],
        "北京": [116.4135540000, 39.9110130000],
        "徐州": [117.2923500000, 34.2101430000],
        "衡水": [115.6769420000, 37.7451660000],
        "包头": [109.8467550000, 40.6636360000],
        "绵阳": [104.6861640000, 31.4733640000],
        "新疆": [87.6233140000, 43.8328060000],
        "枣庄": [117.3285130000, 34.8165690000],
        "浙江": [120.8616930000, 29.0000590000],
        "淄博": [118.0612540000, 36.8191820000],
        "鞍山": [123.0009740000, 41.1141220000],
        "溧阳": [119.4911080000, 31.4217550000],
        "库尔勒": [86.1800780000, 41.7326160000],
        "安阳": [114.3996000000, 36.1036490000],
        "开封": [114.3139040000, 34.8029410000],
        "山东": [118.2749670000, 36.5027850000],
        "德阳": [104.4043190000, 31.1331050000],
        "温州": [120.7058690000, 28.0010950000],
        "九江": [116.0079930000, 29.7113280000],
        "邯郸": [114.5458080000, 36.6312220000],
        "临安": [119.7313180000, 30.2397320000],
        "甘肃": [103.8406920000, 36.0673120000],
        "沧州": [116.8452720000, 38.3102200000],
        "临沂": [118.3629900000, 35.1105310000],
        "南充": [106.1172310000, 30.8432970000],
        "天津": [117.2059140000, 39.0909080000],
        "富阳": [119.8466920000, 30.0010940000],
        "泰安": [117.0948930000, 36.2059050000],
        "诸暨": [120.2427200000, 29.7199910000],
        "河南": [113.6313490000, 34.7534880000],
        "黑龙江": [126.5424170000, 45.8077820000],
        "聊城": [115.9920770000, 36.4626810000],
        "芜湖": [118.4395610000, 31.3587980000],
        "唐山": [118.1870360000, 39.6366730000],
        "平顶山": [113.1989350000, 33.7720510000],
        "邢台": [114.5108890000, 37.0766460000],
        "德州": [116.3658250000, 37.4413130000],
        "济宁": [116.5938520000, 35.4202690000],
        "荆州": [112.2472200000, 30.3406060000],
        "宜昌": [111.2929710000, 30.6976020000],
        "义乌": [120.0812620000, 29.3113260000],
        "丽水": [119.9295030000, 28.4729790000],
        "洛阳": [112.4600330000, 34.6243760000],
        "秦皇岛": [119.6061840000, 39.9412590000],
        "株洲": [113.1404310000, 27.8337370000],
        "河北": [114.5208280000, 38.0486840000],
        "莱芜": [117.6832210000, 36.2193570000],
        "常德": [111.7049940000, 29.0377230000],
        "保定": [115.4710520000, 38.8800550000],
        "湘潭": [112.9505750000, 27.8358500000],
        "金华": [119.6540270000, 29.0844550000],
        "岳阳": [113.1356790000, 29.3632620000],
        "湖南": [112.9453330000, 28.2339710000],
        "衢州": [118.8807680000, 28.9416610000],
        "廊坊": [116.6903400000, 39.5435200000],
        "菏泽": [115.4876960000, 35.2394350000],
        "安徽": [117.2354470000, 31.8268700000],
        "湖北": [114.3118310000, 30.5984280000],
        "大庆": [125.1097270000, 46.5932160000]
    };

    mapDrawer.converEnDataToCN = function (data) {
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            for (var j = 0; j < this.provinceDict.length; j++) {
                var dictItem = this.provinceDict[j];
                if (dictItem.en === item.name) {
                    item.name = dictItem.name;
                }
            }
        }

        return data;
    }

    mapDrawer.convertData = function (data) {
        var res = [];
        for (var i = 0; i < data.length; i++) {
            var geoCoord = this.cityGeos[data[i].name];
            if (geoCoord) {
                res.push({
                    name: data[i].name,
                    value: geoCoord.concat(data[i].value)
                });
            }
        }
        return res;
    };

    mapDrawer.getMaxData = function (data) {
        var max = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].value > max) {
                max = data[i].value;
            }
        }

        return max;
    }

    mapDrawer.drawMapGraph = function (element, color, data, en) {
        if (typeof (data) === 'undefined') return;

        var cookedData = data;
        if (en) {
            cookedData = this.converEnDataToCN(data);
        }

        var topData = this.convertData(cookedData.sort(function (a, b) { return b.value - a.value }).slice(0, 6));
        var option = {
            backgroundColor: '#003d73',
            tooltip: {
                trigger: 'item'
            },
            visualMap: [
            {
                min: 0,
                max: 150000,
                left: 'left',
                top: 'bottom',
                hoverLink: false,
                show: false,
                calculable: false,
                seriesIndex: 0,
                color: this.colorDic[color]
            }
            ],
            geo: {
                map: 'china',
                label: {
                    emphasis: {
                        show: false
                    }
                },
                roam: false,
                itemStyle: {
                    normal: {
                        areaColor: '#323c48',
                        borderColor: '#111'
                    },
                    emphasis: {
                        areaColor: '#2a333d'
                    }
                }
            },
            series: [
                {
                    name: '访问量',
                    type: 'map',
                    mapType: 'china',
                    roam: false,
                    label: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: false
                        }
                    },
                    itemStyle: {
                        emphasis: {
                            color: "red"
                        }
                    },
                    data: cookedData
                }
            ]
        };

        option.visualMap[0].max = this.getMaxData(data);
        var selfMapChart = echarts.init(document.getElementById(element));
        selfMapChart.setOption(option);
    }

    var graphDrawer = {
        mapDrawer: {}
    };

    graphDrawer.mapDrawer = mapDrawer;

    graphDrawer.drawMediaExposure = function (element, data) {

        var schema = [
            { name: 'date', index: 0, text: '日' },
            { name: 'VisitCount', index: 1, text: '访问量' },
            { name: 'ReportCount', index: 2, text: '报道量' }
        ];


        var itemStyle = {
            normal: {
                opacity: 0.8,
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowOffsetY: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
        };

        var option = {
            title: {
                text: '媒体曝光度',
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                }
            },
            backgroundColor: '#003d73',
            color: [
                '#00aeef', '#f26522', '#ffff00', '#39b54a', '#b6b2b2'
            ],
            legend: {
                y: 15,
                data: [],
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                }
            },
            grid: {
                x: '12%',
                x2: '9%',
                y: '22%',
                y2: '12%'
            },
            tooltip: {
                padding: 10,
                backgroundColor: '#222',
                borderColor: '#777',
                borderWidth: 1,
                formatter: function (obj) {
                    var value = obj.value;
                    return '<div style="border-bottom: 1px solid rgba(255,255,255,.3); font-size: 18px;padding-bottom: 7px;margin-bottom: 7px">'
                        + obj.seriesName + ' ' + value[0] + ''
                        + '</div>'
                        + schema[1].text + '：' + value[1] + '<br>'
                        + schema[2].text + '：' + value[2] + '<br>';
                }
            },
            xAxis: {
                type: 'time',
                name: '日期',
                nameGap: 16,
                nameTextStyle: {
                    color: '#fff',
                    fontSize: 14
                },
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisTick: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisLabel: {
                    formatter: '{value}',
                    textStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                type: 'value',
                name: '访问量',
                nameLocation: 'end',
                nameGap: 20,
                nameTextStyle: {
                    color: '#fff',
                    fontSize: 16
                },
                axisLine: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisTick: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                splitLine: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                }
            },
            visualMap: [
                {
                    left: 'right',
                    top: '20%',
                    min: 1,
                    max: 350,
                    dimension: 2,
                    show: false,
                    itemWidth: 0,
                    itemHeight: 0,
                    show: false,
                    calculable: true,
                    precision: 0.1,
                    text: [''],
                    textGap: 10,
                    textStyle: {
                        color: '#fff'
                    },
                    inRange: {
                        symbolSize: [10, 30]
                    },
                    outOfRange: {
                        symbolSize: [10, 30],
                        color: ['rgba(255,255,255,.2)']
                    },
                    controller: {
                        inRange: {
                            color: ['#c23531']
                        },
                        outOfRange: {
                            color: ['#444']
                        }
                    }
                }
            ],
            series: [
                //{name: "Test", type: 'scatter', itemStyle: itemStyle, data: dataBJ}
            ]
        };

        for (var i = 0; i < data.legends.length; i++) {
            option.legend.data.push(data.legends[i]);
        }

        for (var i = 0; i < data.data.length; i++) {
            var name = data.legends[i];
            var singleSeries = {};
            singleSeries.name = name;
            singleSeries.type = 'scatter';
            singleSeries.itemStyle = itemStyle;
            singleSeries.data = data.data[i];
            option.series.push(singleSeries);
        }

        var mediaChart = echarts.init(document.getElementById(element));
        mediaChart.setOption(option);
    };



    graphDrawer.drawLine = function (element, data, func) {
        var option = {
            title: {
                text: '用户情感分布',
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                }
            },
            backgroundColor: '#003d73',
            color: [
                '#00aeef', '#f26522', '#ffff00', '#39b54a', '#b6b2b2'
            ],
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [],
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                },
                borderWidth: "5px",
                y: 15
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            //toolbox: {
            //    feature: {
            //        saveAsImage: {}
            //    }
            //},
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: [],
                nameTextStyle: {
                    color: '#fff',
                    fontSize: 14
                },
                max: 31,
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisTick: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisLabel: {
                    formatter: '{value}',
                    textStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                type: 'value',
                nameGap: 20,
                nameTextStyle: {
                    color: '#fff',
                    fontSize: 16
                },
                axisLine: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                axisTick: {
                    lineStyle: {
                        color: '#777'
                    }
                },
                splitLine: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                }
            },
            series: [
                //{
                //    name: '雅培',
                //    type: 'line',
                //    stack: '总量',
                //    smooth: true,
                //    data: [-120, 132, 101, 134, 90, 230, 210]
                //},
            ]
        };

        for (var i = 0; i < data.keys.length; i++) {
            var item = data.keys[i];
            option.xAxis.data.push(item);
        }
        if (data.keys.length == 0) {
            option.xAxis.data.push("");
        }

        for (var i = 0; i < data.series.length; i++) {
            var item = data.series[i];
            var legend = {};
            legend.name = item.name;
            legend.icon = 'circle';
            option.legend.data.push(legend);
            option.series.push(item);
        }

        var sentiChart = echarts.init(document.getElementById(element));
        sentiChart.setOption(option);
        sentiChart.on("click", func);
    };

    graphDrawer.drawRadar = function (element, data) {
        var option = {
            backgroundColor: '#003d73',
            title: {
                text: '用户年龄分布',
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                }
            },
            splitNumber: 0,
            color: [
                '#00aeef', '#f26522', '#ffff00', '#39b54a', '#b6b2b2'
            ],
            tooltip: {},
            legend: {
                data: [],
                textStyle: {
                    color: '#fff',
                    fontSize: 16
                },
                orient: 'vertical',
                left: 'right',
                itemGap: 20,
                y: 40,
                x2: 20
            },
            radar: {
                indicator: [
                ]
            },
            series: [
                {
                    name: '用户年龄分布',
                    type: 'radar',
                    data: [
                    ]
                }
            ]
        };


        if (data.indicators.length > 0) {
            for (var i = 0; i < data.indicators.length; i++) {
                var item = data.indicators[i];
                item.max = parseInt(item.max * 1.2);
                option.radar.indicator.push(item);
            }

            for (var i = 0; i < data.values.length; i++) {
                var item = data.values[i];
                var legend = {};
                legend.name = item.name;
                legend.icon = 'circle';
                option.legend.data.push(legend);
                option.series[0].data.push(item);
            }

            var ageChart = echarts.init(document.getElementById(element));
            ageChart.setOption(option);
        }
    };

    window.graphDrawer = graphDrawer;

})(typeof (window) !== 'undefined' ? window : this);