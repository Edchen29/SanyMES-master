layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'hhweb', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var hhweb = layui.hhweb;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER
    //var thiswin = (top == undefined) ? window : top.window;
    //给按钮添加快捷键
    //Ctrl+I键添加,Ctrl+R键刷新,Ctrl+D键删除,Ctrl+E键编辑,Ctrl+F查询,Ctrl+S保存,Ctrl+A全选,Ctrl+Shift+A取消全选,左 上一页 右 下一页
    $(document).bind("keydown", function (e) {
        //禁用页面F5刷新
        if (e.which == 116) {
            e.preventDefault(); //Skip default behavior of the enter key
        }
    });
    $(document).ready(function () {
        //self.window.focus();
        //console.log(self.window.frames.document.visibilityState);
         // JSON.parse(window.sessionStorage.getItem("curmenu")).title;
        var stkChart = echarts.init(document.getElementById('stk-chart'));
        var productChart = echarts.init(document.getElementById('product-chart'));
        var taskChart = echarts.init(document.getElementById('task-chart'));
        //clearInterval(timeTicket);
        var timeTicket = setInterval(funchar, 20000);
        //self.window.onblur = function () {
        //    clearInterval(timeTicket);
        //    console.log("停");
        //};
        //self.window.focus = function () {
        //    console.log("启");
        //    timeTicket = setInterval(funchar, 20000);
        //}
        funchar();
        function funchar() {

            $.ajax({
                url: "/base/EchartsData/GetDistributionInfo",
                data: {},
                type: "POST",
                dataType: "json",
                async: true,
                success: function (data) {
                    var jdata = [], jdata2 = [], jdata3 = [], jdata4 = [], jdata5 = [];
                    for (var i = 0; i < data.data.length; i++) {
                        if (typeof data.data[i] !== 'undefined') {
                            if (data.data[i].name == "Assemble11") {
                                if (data.data[i].type == "A1-11") {
                                    jdata[0] = data.data[i].count;
                                } else if (data.data[i].type == "A1-12") {
                                    jdata2[0] = data.data[i].count;
                                } else if (data.data[i].type == "B1-21") {
                                    jdata3[0] = data.data[i].count;
                                } else if (data.data[i].type == "B1-22") {
                                    jdata4[0] = data.data[i].count;
                                } else if (data.data[i].type == "C1-31") {
                                    jdata5[0] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "Assemble12") {
                                if (data.data[i].type == "A1-41") {
                                    jdata[1] = data.data[i].count;
                                } else if (data.data[i].type == "A1-42") {
                                    jdata2[1] = data.data[i].count;
                                } else if (data.data[i].type == "B1-51") {
                                    jdata3[1] = data.data[i].count;
                                } else if (data.data[i].type == "B1-52") {
                                    jdata4[1] = data.data[i].count;
                                } else if (data.data[i].type == "C1-61") {
                                    jdata5[1] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "Assemble21") {
                                if (data.data[i].type == "A2-11") {
                                    jdata[2] = data.data[i].count;
                                } else if (data.data[i].type == "A2-12") {
                                    jdata2[2] = data.data[i].count;
                                } else if (data.data[i].type == "B2-21") {
                                    jdata3[2] = data.data[i].count;
                                } else if (data.data[i].type == "B2-22") {
                                    jdata4[2] = data.data[i].count;
                                } else if (data.data[i].type == "C2-31") {
                                    jdata5[2] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "Assemble22") {
                                if (data.data[i].type == "A2-41") {
                                    jdata[3] = data.data[i].count;
                                } else if (data.data[i].type == "A2-42") {
                                    jdata2[3] = data.data[i].count;
                                } else if (data.data[i].type == "B2-51") {
                                    jdata3[3] = data.data[i].count;
                                } else if (data.data[i].type == "B2-52") {
                                    jdata4[3] = data.data[i].count;
                                } else if (data.data[i].type == "C2-61") {
                                    jdata5[3] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "Assemble31") {
                                if (data.data[i].type == "A3-11") {
                                    jdata[4] = data.data[i].count;
                                } else if (data.data[i].type == "A3-12") {
                                    jdata2[4] = data.data[i].count;
                                } else if (data.data[i].type == "B3-21") {
                                    jdata3[4] = data.data[i].count;
                                } else if (data.data[i].type == "B3-22") {
                                    jdata4[4] = data.data[i].count;
                                } else if (data.data[i].type == "C3-31") {
                                    jdata5[4] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "Assemble32") {
                                if (data.data[i].type == "A3-41") {
                                    jdata[5] = data.data[i].count;
                                } else if (data.data[i].type == "A3-42") {
                                    jdata2[5] = data.data[i].count;
                                } else if (data.data[i].type == "B3-51") {
                                    jdata3[5] = data.data[i].count;
                                } else if (data.data[i].type == "B3-52") {
                                    jdata4[5] = data.data[i].count;
                                } else if (data.data[i].type == "C3-61") {
                                    jdata5[5] = data.data[i].count;
                                }
                            }

                        }
                    }
                    //var stkChart = echarts.init(document.getElementById('stk-chart'));
                    var stkoption = {
                        title: {
                            text: '上料配送任务',
                            //  backgroundColorr: '#FFFFFF', 
                            subtext: '单位:笔/任务',
                            textStyle: {
                                // fontWeight: 'normal',
                                // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
                                fontWeight: 'bolder',
                                fontSize: 18,
                                color: '#00FFFF'
                            },
                        },
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                            }
                        },
                        legend: {
                            right: 0,
                            data: [{
                                name: 'A主',
                                textStyle: { color: "red" }
                            },
                            {
                                name: 'A备',
                                textStyle: { color: "#0174DF" }
                            },
                            {
                                name: 'B主',
                                textStyle: { color: "#3ADF00" }
                            },
                            {
                                name: 'B备',
                                textStyle: { color: "orange" }
                            },
                            {
                                name: 'C通',
                                textStyle: { color: "aqua" }
                            }]
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: {
                            type: 'value',
                            //设置坐标轴字体颜色和宽度  
                            axisLine: {
                                lineStyle: {
                                    color: '#00FFFF',
                                    width: 2
                                }
                            },
                        },
                        yAxis: {
                            type: 'category',
                            //设置坐标轴字体颜色和宽度  
                            axisLine: {
                                lineStyle: {
                                    color: '#00FFFF',
                                    width: 3
                                }
                            },
                            data: ['上料1', '上料2', '上料3', '上料4', '上料5', '上料6']
                        },
                        series: [
                            {
                                name: 'A主',
                                type: 'bar',
                                textStyle: { color: "red" },//设置颜色
                                stack: '总量',
                                label: {
                                    show: true,
                                    // position: 'insideRight'
                                    position: 'inside'
                                },
                                data: jdata
                            },
                            {
                                name: 'A备',
                                type: 'bar',
                                itemStyle: { normal: { color: '#0174DF' } },
                                stack: '总量',
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: jdata2
                            },
                            {
                                name: 'B主',
                                type: 'bar',
                                itemStyle: { normal: { color: '#3ADF00' } },
                                stack: '总量',
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: jdata3
                            },
                            {
                                name: 'B备',
                                type: 'bar',
                                itemStyle: { normal: { color: 'orange' } },
                                stack: '总量',
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: jdata4
                            },
                            {
                                name: 'C通',
                                type: 'bar',
                                itemStyle: { normal: { color: 'aqua' } },
                                stack: '总量',
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: jdata5
                            }
                        ]
                    };
                     stkChart.setOption(stkoption);
                    //setTimeout(() => {
                    //    try {
                    //        stkChart.setOption(stkoption)
                    //    } catch (error) { }
                    //}, 500)
                },
                //error: function (XMLHttpRequest, textStatus, errorThrown) {
                //    layer.alert(errorThrown, { icon: 2, title: '提示' });
                //}
            });
            //产品统计
            $.ajax({
                url: "/base/EchartsData/GetProductInfo",
                data: {},
                type: "POST",
                dataType: "json",
                async: true,
                success: function (data) {

                    var one = 0, two = 0, three = 0;
                    var jdata = [];
                    for (var i = 0; i <= data.data.length; i++) {
                        if (typeof data.data[i] !== 'undefined') {
                            if (data.data[i].name == "Shop1") {
                                one = one + data.data[i].count;
                            } else if (data.data[i].name == "Shop2") {
                                two = two + data.data[i].count;
                            } else if (data.data[i].name == "Shop3") {
                                three = three + data.data[i].count;
                            }
                            jdata.push({ value: data.data[i].count, name: data.data[i].type });
                        }
                    }

                    // var productChart = echarts.init(document.getElementById('product-chart'));
                    var productoption = {
                        title: {
                            text: '生产岛产品统计',
                            textStyle: {
                                // fontWeight: 'normal',
                                // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
                                fontWeight: 'bolder',
                                fontSize: 18,
                                color: '#00FFFF'
                            },
                            color: '#ffffff',
                            subtext: '',
                            left: 0//'center'
                        },
                        tooltip: {
                            trigger: 'item',
                            formatter: '{a} <br/>{b}: {c} ({d}%)'
                        },
                        legend: {
                            icon: 'circle',
                           // itemHeight: 9,//改变圆圈大小
                            bottom: 0,
                            left: 0,
                            data: [{
                                name: '岛一',
                                textStyle: { color: "red" }//设置颜色
                                },
                                {
                                    name: '岛二',
                                    textStyle: { color: "#2f4553" }//设置颜色
                                }, {
                                    name: '岛三',
                                    textStyle: { color: "#61a0a9" }//设置颜色
                                },]
                        },
                        series: [
                            {
                                name: '产品数量',
                                type: 'pie',
                                selectedMode: 'single',
                                radius: [0, '30%'],

                                label: {
                                    position: 'inner'
                                },
                                labelLine: {
                                    show: false
                                },
                                data: [
                                    { value: one, name: '岛一', selected: true },
                                    { value: two, name: '岛二' },
                                    { value: three, name: '岛三' }
                                ]
                            },
                            {
                                name: '机型数量',
                                type: 'pie',
                                radius: ['40%', '55%'],
                                label: {
                                    // formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                                    formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}',
                                    backgroundColor: '#eee',
                                    borderColor: '#aaa',
                                    borderWidth: 1,
                                    borderRadius: 4,
                                    backgroundColor: 'rgba(128, 128, 128, 0.1)',
                                    shadowBlur: 3,
                                    shadowOffsetX: 2,
                                    shadowOffsetY: 2,
                                    shadowColor: '#999',
                                    padding: [0, 7],
                                    rich: {
                                        a: {
                                            color: '#999',
                                            lineHeight: 15,
                                            align: 'center'
                                        },
                                        // abg: {
                                        //  backgroundColor: '#333',
                                        //  width: '100%',
                                        //  align: 'right',
                                        // height: 22,
                                        //  borderRadius: [4, 4, 0, 0]
                                        // },
                                        hr: {
                                            borderColor: '#aaa',
                                            width: '100%',
                                            borderWidth: 0.5,
                                            height: 0
                                        },
                                        b: {
                                            //  width:30,
                                            fontSize: 14,
                                            lineHeight: 20
                                        },
                                        per: {
                                            color: '#eee',
                                            backgroundColor: '#334455',
                                            padding: [2, 4],
                                            borderRadius: 2
                                        }
                                    }
                                },
                                data: jdata
                            }
                        ]
                    };
                    productChart.setOption(productoption);
                },
                //error: function (XMLHttpRequest, textStatus, errorThrown) {
                //    layer.alert(errorThrown, { icon: 2, title: '提示' });
                //}
            });
            $.ajax({
                url: "/base/EchartsData/GetStationInfo",
                data: {},
                type: "POST",
                dataType: "json",
                async: true,
                success: function (data) {
                    var jdata = [], jdata2 = [], jdata3 = [];
                    for (var i = 0; i < data.data.length; i++) {
                        if (typeof data.data[i] !== 'undefined') {
                            if (data.data[i].name == "3") {

                                if (data.data[i].type == "1") {
                                    jdata[0] = data.data[i].count;
                                } else if (data.data[i].type == "2") {
                                    jdata[1] = data.data[i].count;
                                } else if (data.data[i].type == "3") {
                                    jdata[2] = data.data[i].count;
                                } else if (data.data[i].type == "4") {
                                    jdata[3] = data.data[i].count;
                                } else if (data.data[i].type == "5") {
                                    jdata[4] = data.data[i].count;
                                } else if (data.data[i].type == "6") {
                                    jdata[5] = data.data[i].count;
                                } else if (data.data[i].type == "7") {
                                    jdata[6] = data.data[i].count;
                                } else if (data.data[i].type == "8") {
                                    jdata[7] = data.data[i].count;
                                } else if (data.data[i].type == "9") {
                                    jdata[8] = data.data[i].count;
                                } else if (data.data[i].type == "10") {
                                    jdata[9] = data.data[i].count;
                                } else if (data.data[i].type == "11") {
                                    jdata[10] = data.data[i].count;
                                }
                            } else if (data.data[i].name == "4") {
                                if (data.data[i].type == "23") {
                                    jdata2[0] = data.data[i].count;
                                } else if (data.data[i].type == "24") {
                                    jdata2[1] = data.data[i].count;
                                } else if (data.data[i].type == "25") {
                                    jdata2[2] = data.data[i].count;
                                } else if (data.data[i].type == "26") {
                                    jdata2[3] = data.data[i].count;
                                } else if (data.data[i].type == "27") {
                                    jdata2[4] = data.data[i].count;
                                } else if (data.data[i].type == "28") {
                                    jdata2[5] = data.data[i].count;
                                } else if (data.data[i].type == "29") {
                                    jdata2[6] = data.data[i].count;
                                } else if (data.data[i].type == "30") {
                                    jdata2[7] = data.data[i].count;
                                } else if (data.data[i].type == "31") {
                                    jdata2[8] = data.data[i].count;
                                } else if (data.data[i].type == "32") {
                                    jdata2[9] = data.data[i].count;
                                } else if (data.data[i].type == "33") {
                                    jdata2[10] = data.data[i].count;
                                }

                            } else if (data.data[i].name == "5") {
                                if (data.data[i].type == "34") {
                                    jdata3[0] = data.data[i].count;
                                } else if (data.data[i].type == "35") {
                                    jdata3[1] = data.data[i].count;
                                } else if (data.data[i].type == "36") {
                                    jdata3[2] = data.data[i].count;
                                } else if (data.data[i].type == "37") {
                                    jdata3[3] = data.data[i].count;
                                } else if (data.data[i].type == "38") {
                                    jdata3[4] = data.data[i].count;
                                } else if (data.data[i].type == "39") {
                                    jdata3[5] = data.data[i].count;
                                } else if (data.data[i].type == "40") {
                                    jdata3[6] = data.data[i].count;
                                } else if (data.data[i].type == "41") {
                                    jdata3[7] = data.data[i].count;
                                } else if (data.data[i].type == "42") {
                                    jdata3[8] = data.data[i].count;
                                } else if (data.data[i].type == "43") {
                                    jdata3[9] = data.data[i].count;
                                } else if (data.data[i].type == "44") {
                                    jdata3[10] = data.data[i].count;
                                }
                            }

                        }
                    }
                    for (var j = 0; j < 11; j++) {
                        if (jdata[j] == null) {
                            jdata[j] = 0;
                        }
                        if (jdata2[j] == null) {
                            jdata2[j] = 0;
                        }
                        if (jdata3[j] == null) {
                            jdata3[j] = 0;
                        }
                    }

                    //var taskChart = echarts.init(document.getElementById('task-chart'));
                    var taskoption = {
                        // backgroundColor: 'rgba(128, 128, 128, 0.25)', //rgba设置透明度0.1
                        title: {
                            text: '工位效率分析',
                            subtext: '单位:次/天',
                            textStyle: {
                                // fontWeight: 'normal',
                                // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
                                fontWeight: 'bolder',
                                fontSize: 18,
                                color: '#00FFFF'
                            },
                        },
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            data: [{
                                name: '岛一',
                                textStyle: { color: "red" }//设置颜色
                            }, {
                                name: '岛二',
                                textStyle: { color: "green" }//设置颜色
                            }, {
                                name: '岛三',
                                textStyle: { color: "#0174DF" }//设置颜色
                            },]
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        toolbox: {
                            feature: {
                                saveAsImage: {}
                            }
                        },
                        xAxis: {
                            type: 'category',
                            axisLine: {
                                lineStyle: {
                                    color: '#00FFFF',
                                    width: 2
                                }
                            },
                            boundaryGap: false,
                            data: ['组对1', '组对2', '焊接1', '焊接2', '焊接3', '焊接4', '焊接5', '焊接6', '补焊', '补焊2', '机加']
                        },
                        yAxis: {
                            type: 'value',
                            axisLine: {
                                lineStyle: {
                                    color: '#00FFFF',
                                    width: 2
                                }
                            },
                        },
                        series: [
                            {
                                name: '岛一',
                                type: 'line',
                                symbolSize: 8,
                                itemStyle: {
                                    normal: {
                                        lineStyle: {
                                            color: 'red'
                                        }
                                    }
                                },
                                data: jdata
                            },
                            {
                                name: '岛二',
                                type: 'line',
                                symbolSize: 8,
                                itemStyle: {
                                    normal: {
                                        lineStyle: {
                                            color: 'green'
                                        }
                                    }
                                },
                                data: jdata2
                            },
                            {
                                name: '岛三',
                                type: 'line',
                                symbolSize: 8,
                                itemStyle: {
                                    normal: {
                                        lineStyle: {
                                            color: '#0174DF'
                                        }
                                    }
                                },
                                data: jdata3
                            }
                        ]
                    };
                    taskChart.setOption(taskoption);
                },
                //error: function (XMLHttpRequest, textStatus, errorThrown) {
                //    layer.alert(errorThrown, { icon: 2, title: '提示' });
                //}
            });
            //统计每日产能
            $.ajax({
                url: "/base/EchartsData/GetOutPutInfo",
                data: {},
                type: "POST",
                dataType: "json",
                async: true,
                success: function (data) {
                    var c = 0, c2 = 0, c3 = 0, totalc = 0;
                    var p = 0, p2 = 0, p3 = 0, totalp = 0;
                    for (var i = 0; i < data.data.length; i++) {
                        if (typeof data.data[i] !== 'undefined') {
                            for (var j = 0; j < data.data[i].length; j++) {
                                if (typeof data.data[i][j] !== 'undefined') {
                                    if (data.data[i][j].type == "P") {
                                        if (data.data[i][j].name == "3") {
                                            p = data.data[i][j].value;
                                        } else if (data.data[i][j].name == "4") {
                                            p2 = data.data[i][j].value;
                                        } else if (data.data[i][j].name == "5") {
                                            p3 = data.data[i][j].value;
                                        }
                                    } else {
                                        if (data.data[i][j].name == "3") {
                                            c = data.data[i][j].value;
                                        } else if (data.data[i][j].name == "4") {

                                            c2 = data.data[i][j].value;

                                        } else if (data.data[i][j].name == "5") {

                                            c3 = data.data[i][j].value;

                                        }
                                    }
                                }

                            }


                        }

                    }
                    totalc = c + c2 + c3;
                    totalp = p + p2 + p3;
                    var pone = document.getElementById("pone");
                    var wone = document.getElementById("wone");
                    var ponep = document.getElementById("ponep");
                    wone.innerHTML = c;
                    pone.innerHTML = p;
                    if (c == 0) {
                        ponep.innerHTML = Math.floor(p * 100 / 1 * 10) / 10 + "%";
                    } else {
                        ponep.innerHTML = Math.floor(p * 100 / c * 10) / 10 + "%";
                    }
                    
                    var ptwo = document.getElementById("ptwo");
                    var wtwo = document.getElementById("wtwo");
                    var ptwop = document.getElementById("ptwop");
                    wtwo.innerHTML = c2;
                    ptwo.innerHTML = p2;
                    if (c2 == 0) {
                        ptwop.innerHTML = Math.floor(p2 * 100 / 1 * 10) / 10 + "%";
                    } else {
                        ptwop.innerHTML = Math.floor(p2 * 100 / c2 * 10) / 10 + "%";
                    }
                    
                    var pthree = document.getElementById("pthree");
                    var wthree = document.getElementById("wthree");
                    var pthreep = document.getElementById("pthreep");
                    wthree.innerHTML = c3;
                    pthree.innerHTML = p3;
                    if (c3 == 0) {
                        pthreep.innerHTML = Math.floor(p3 * 100 / 1 * 10) / 10 + "%";
                    } else {
                        pthreep.innerHTML = Math.floor(p3 * 100 / c3 * 10) / 10 + "%";
                    }
                    
                    var ptotal = document.getElementById("ptotal");
                    var wtotal = document.getElementById("wtotal");
                    var ptotalp = document.getElementById("ptotalp");
                    wtotal.innerHTML = totalc;
                    ptotal.innerHTML = totalp;
                    if (totalc==0) {
                        ptotalp.innerHTML = Math.floor(totalp * 100 / 1 * 10) / 10 + "%";
                    } else {
                        ptotalp.innerHTML = Math.floor(totalp * 100 / totalc * 10) / 10 + "%";
                    }


                },
                //error: function (XMLHttpRequest, textStatus, errorThrown) {
                //    layer.alert(errorThrown, { icon: 2, title: '提示' });
                //}
            });
            //展示车间生产情况
            $.ajax({
                url: "/monitor/EquipmentMonitor/Load",
                data: {},
                type: "POST",
                dataType: "json",
                async: true,
                success: function (data) {
                    var table = document.getElementById("shipone");
                    var tableLength = table.rows.length;
                    for (var int = 2; int < tableLength; int++) {
                        table.deleteRow(1);
                    }
                    var table2 = document.getElementById("shiptwo");
                    var tableLength2 = table2.rows.length;
                    for (var int = 2; int < tableLength2; int++) {
                        table2.deleteRow(1);
                    }
                    var table3 = document.getElementById("shipthree");
                    var tableLength3 = table3.rows.length;
                    for (var int = 2; int < tableLength3; int++) {
                        table3.deleteRow(1);
                    }
                    // var currentRows = document.getElementById("shipone").rows.length;
                    var w = 1, work = 0, stop = 0, fire = 0, repair = 0, free = 0, other = 0, work2 = 0, stop2 = 0, fire2 = 0, repair2 = 0, free2 = 0, other2 = 0, work3 = 0, stop3 = 0, fire3 = 0, repair3 = 0, free3 = 0, other3 = 0;
                    var t = 1, j = 1;
                    for (var i = 0; i <= data.data.length; i++) {
                        if (typeof data.data[i] !== 'undefined') {
                            if (data.data[i].WorkshopCode == "Shop1") {
                                var oneTr = document.getElementById("shipone").insertRow(j);
                                var oneTd = oneTr.insertCell(0);
                                //insertTd.style.textAlign = "center";
                                oneTd.innerHTML = data.data[i].EquipmentName;
                                oneTd = oneTr.insertCell(1);
                                if (data.data[i].Status == "生产") {
                                    oneTd.style.color = "green";
                                    work = work + 1;
                                } else if (data.data[i].Status == "故障") {
                                    oneTd.style.color = "red";
                                    repair = repair + 1;
                                } else if (data.data[i].Status == "报警") {
                                    oneTd.style.color = "orange";
                                    fire = fire + 1;
                                } else if (data.data[i].Status == "停机") {
                                    oneTd.style.color = "gray";
                                    stop = stop + 1;
                                } else if (data.data[i].Status == "空闲") {
                                    oneTd.style.color = "DeepSkyBlue";
                                    free = free + 1;
                                } else {
                                    other = other + 1;
                                }

                                oneTd.innerHTML = data.data[i].Status;
                                oneTd = oneTr.insertCell(2);
                                oneTd.innerHTML = data.data[i].StepName;
                                oneTd = oneTr.insertCell(3);
                                oneTd.innerHTML = data.data[i].StationName;
                                oneTd = oneTr.insertCell(4);
                                oneTd.innerHTML = data.data[i].WONumber;
                                oneTd = oneTr.insertCell(5);
                                oneTd.innerHTML = data.data[i].ProductCode;
                                oneTd = oneTr.insertCell(6);
                                oneTd.innerHTML = data.data[i].SerialNumber;
                                j = j + 1;
                            } else if (data.data[i].WorkshopCode == "Shop2") {
                                var twoTr = document.getElementById("shiptwo").insertRow(w);
                                var twoTd = twoTr.insertCell(0);
                                twoTd.innerHTML = data.data[i].EquipmentName;
                                twoTd = twoTr.insertCell(1);
                                if (data.data[i].Status == "生产") {
                                    twoTd.style.color = "green";
                                    work2 = work2 + 1;
                                } else if (data.data[i].Status == "故障") {
                                    twoTd.style.color = "red";
                                    repair2 = repair2 + 1;
                                } else if (data.data[i].Status == "报警") {
                                    twoTd.style.color = "orange";
                                    fire2 = fire2 + 1;
                                } else if (data.data[i].Status == "停机") {
                                    twoTd.style.color = "gray";
                                    stop2 = stop2 + 1;
                                } else if (data.data[i].Status == "空闲") {
                                    twoTd.style.color = "DeepSkyBlue";
                                    free2 = free2 + 1;
                                } else {
                                    other2 = other2 + 1;
                                }
                                twoTd.innerHTML = data.data[i].Status;
                                twoTd = twoTr.insertCell(2);
                                twoTd.innerHTML = data.data[i].StepName;
                                twoTd = twoTr.insertCell(3);
                                twoTd.innerHTML = data.data[i].StationName;
                                twoTd = twoTr.insertCell(4);
                                twoTd.innerHTML = data.data[i].WONumber;
                                twoTd = twoTr.insertCell(5);
                                twoTd.innerHTML = data.data[i].ProductCode;
                                twoTd = twoTr.insertCell(6);
                                twoTd.innerHTML = data.data[i].SerialNumber;
                                w = w + 1;
                            } else if (data.data[i].WorkshopCode == "Shop3") {
                                var threeTr = document.getElementById("shipthree").insertRow(t);
                                var threeTd = threeTr.insertCell(0);
                                threeTd.innerHTML = data.data[i].EquipmentName;
                                threeTd = threeTr.insertCell(1);
                                if (data.data[i].Status == "生产") {
                                    threeTd.style.color = "green";
                                    work3 = work3 + 1;
                                } else if (data.data[i].Status == "故障") {
                                    threeTd.style.color = "red";
                                    repair3 = repair3 + 1;
                                } else if (data.data[i].Status == "报警") {
                                    threeTd.style.color = "orange";
                                    fire3 = fire3 + 1;
                                } else if (data.data[i].Status == "停机") {
                                    threeTd.style.color = "gray";
                                    stop3 = stop3 + 1;
                                } else if (data.data[i].Status == "空闲") {
                                    threeTd.style.color = "DeepSkyBlue";
                                    free3 = free3 + 1;
                                } else {
                                    other3 = other3 + 1;
                                }
                                threeTd.innerHTML = data.data[i].Status;
                                threeTd = threeTr.insertCell(2);
                                threeTd.innerHTML = data.data[i].StepName;
                                threeTd = threeTr.insertCell(3);
                                threeTd.innerHTML = data.data[i].StationName;
                                threeTd = threeTr.insertCell(4);
                                threeTd.innerHTML = data.data[i].WONumber;
                                threeTd = threeTr.insertCell(5);
                                threeTd.innerHTML = data.data[i].ProductCode;
                                threeTd = threeTr.insertCell(6);
                                threeTd.innerHTML = data.data[i].SerialNumber;
                                t = t + 1;
                            }
                        }
                    }
                    //岛一
                    var wone = document.getElementById("workone");
                    var wonep = document.getElementById("workonep");
                    var tatalonep = document.getElementById("totalonep");
                    var totaltwop = document.getElementById("totaltwop");
                    var totalthreep = document.getElementById("totalthreep");
                    tatalonep.innerHTML = stop + fire + free + work + repair + other;
                    totaltwop.innerHTML = stop2 + fire2 + free2 + work2 + repair2 + other2;
                    totalthreep.innerHTML = stop3 + fire3 + free3 + work3 + repair3 + other3;
                    wone.innerHTML = work;
                    //wonep.innerHTML = (work * 100 / (stop + fire + free + work + repair)).toFixed(1) + "%";
                    wonep.innerHTML = Math.floor(work * 100 / (stop + fire + free + work + repair) * 10) / 10 + "%";

                    var sone = document.getElementById("stopone");
                    var sonep = document.getElementById("stoponep");
                    sone.innerHTML = stop;
                    sonep.innerHTML = Math.floor(stop * 100 / (stop + fire + free + work + repair) * 10) / 10 + "%";

                    var fone = document.getElementById("freeone");
                    var fonep = document.getElementById("freeonep");
                    fone.innerHTML = free;
                    fonep.innerHTML = Math.floor(free * 100 / (stop + fire + free + work + repair) * 10) / 10 + "%";

                    var fione = document.getElementById("fireone");
                    var fionep = document.getElementById("fireonep");
                    fione.innerHTML = fire;
                    fionep.innerHTML = Math.floor(fire * 100 / (stop + fire + free + work + repair) * 10) / 10 + "%";

                    var rone = document.getElementById("repairone");
                    var ronep = document.getElementById("repaironep");
                    rone.innerHTML = repair;
                    ronep.innerHTML = Math.floor(repair * 100 / (stop + fire + free + work + repair) * 10) / 10 + "%";

                    //岛二
                    var wtwo = document.getElementById("worktwo");
                    var wtwop = document.getElementById("worktwop");
                    wtwo.innerHTML = work2;
                    wtwop.innerHTML = Math.floor(work2 * 100 / (stop2 + fire2 + free2 + work2 + repair2) * 10) / 10 + "%";

                    var stwo = document.getElementById("stoptwo");
                    var stwop = document.getElementById("stoptwop");
                    stwo.innerHTML = stop2;
                    stwop.innerHTML = Math.floor(stop2 * 100 / (stop2 + fire2 + free2 + work2 + repair2) * 10) / 10 + "%";

                    var ftwo = document.getElementById("freetwo");
                    var ftwop = document.getElementById("freetwop");
                    ftwo.innerHTML = free2;
                    ftwop.innerHTML = Math.floor(free2 * 100 / (stop2 + fire2 + free2 + work2 + repair2) * 10) / 10 + "%";

                    var fitwo = document.getElementById("firetwo");
                    var fitwop = document.getElementById("firetwop");
                    fitwo.innerHTML = fire2;
                    fitwop.innerHTML = Math.floor(fire2 * 100 / (stop2 + fire2 + free2 + work2 + repair2) * 10) / 10 + "%";

                    var rtwo = document.getElementById("repairtwo");
                    var rtwop = document.getElementById("repairtwop");
                    rtwo.innerHTML = repair2;
                    rtwop.innerHTML = Math.floor(repair2 * 100 / (stop2 + fire2 + free2 + work2 + repair2) * 10) / 10 + "%";

                    //岛三
                    var wthree = document.getElementById("workthree");
                    var wthreep = document.getElementById("workthreep");
                    wthree.innerHTML = work3;
                    wthreep.innerHTML = Math.floor(work3 * 100 / (stop3 + fire3 + free3 + work3 + repair3) * 10) / 10 + "%";

                    var sthree = document.getElementById("stopthree");
                    var sthreep = document.getElementById("stopthreep");
                    sthree.innerHTML = stop3;
                    sthreep.innerHTML = Math.floor(stop3 * 100 / (stop3 + fire3 + free3 + work3 + repair3) * 10) / 10 + "%";

                    var fthree = document.getElementById("freethree");
                    var fthreep = document.getElementById("freethreep");
                    fthree.innerHTML = free3;
                    fthreep.innerHTML = Math.floor(free3 * 100 / (stop3 + fire3 + free3 + work3 + repair3) * 10) / 10 + "%";

                    var fithree = document.getElementById("firethree");
                    var fithreep = document.getElementById("firethreep");
                    fithree.innerHTML = fire3;
                    fithreep.innerHTML = Math.floor(fire3 * 100 / (stop3 + fire3 + free3 + work3 + repair3) * 10) / 10 + "%";

                    var rthree = document.getElementById("repairthree");
                    var rthreep = document.getElementById("repairthreep");
                    rthree.innerHTML = repair3;
                    rthreep.innerHTML = Math.floor(repair3 * 100 / (stop3 + fire3 + free3 + work3 + repair3) * 10) / 10 + "%";
                },
                //error: function (XMLHttpRequest, textStatus, errorThrown) {
                //    layer.alert(errorThrown, { icon: 2, title: '提示' });
                //}
            });
        }
        
        //仓位统计 warecell-chart
        //$($('#warecell-chart')).height(($(document).height() - $("#page-inner").children()[0].offsetHeight - $("#page-inner").children()[2].offsetHeight) / 3);
        //var warecelloption = {
        //    // backgroundColor: 'rgba(128, 128, 128, 0.1)', //rgba设置透明度0.1
        //    title: {
        //        text: '上料缓存区统计',
        //        textStyle: {
        //            // fontWeight: 'normal',
        //            // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
        //            fontWeight: 'bolder',
        //            fontSize: 20,
        //            color: '#00FFFF'
        //        },
        //        color: '#ffffff',
        //        subtext: '',
        //        left: 'center'
        //    },
        //    tooltip: {
        //        trigger: 'item',
        //        formatter: "{a} <br/>{b} : {c} ({d}%)"
        //    },
        //    legend: {
        //        // orient: 'vertical',
        //        // top: 'middle',
        //        bottom: 10,
        //        left: 'center',
        //        data: [{
        //            name: '满仓',
        //            textStyle: { color: "red" }//设置颜色
        //        }, , {
        //            name: '锁定',
        //            textStyle: { color: "yellow" }//设置颜色
        //        }, {
        //            name: '禁用',
        //            textStyle: { color: "blue" }//设置颜色
        //        }, {
        //            name: '空闲',
        //            textStyle: { color: "#00FFFF" }//设置颜色
        //        }]
        //    },
        //    series: [
        //        {
        //            type: 'pie',
        //            radius: '65%',
        //            center: ['50%', '50%'],
        //            selectedMode: 'single',
        //            data: [
        //                { value: 54, name: '满仓' },
        //                { value: 7, name: '锁定' },

        //                { value: 3, name: '禁用' },
        //                { value: 36, name: '空闲' },

        //            ],
        //            itemStyle: {
        //                emphasis: {
        //                    shadowBlur: 10,
        //                    shadowOffsetX: 0,
        //                    shadowColor: 'rgba(0, 0, 0, 0.5)'
        //                }
        //            }
        //        }
        //    ]
        //};
       // warecellChart.setOption(warecelloption);

        
        //    {
        //   // backgroundColor: 'rgba(128, 128, 128, 0.25)', //rgba设置透明度0.1
        //  //  overflow: visible,
        //   // borderWidth: '10',
        //    color: ['red', '#3ADF00'],//修改legend图标颜色
        //    title: {
        //        text: '上料配送任务',
        //      //  backgroundColorr: '#FFFFFF', 
        //        subtext: '单位:笔/任务',
        //        textStyle: {
        //          // fontWeight: 'normal',
        //            // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
        //            fontWeight: 'bolder',
        //            fontSize: 18, 
        //            color: '#00FFFF'
        //        },  
        //    },
        //    tooltip: {
        //        trigger: 'axis',
        //        axisPointer: {
        //            type: 'shadow'
        //        }
        //    },
        //    legend: {
        //        right: 0,
        //        borderColor: '#00FFFF',
        //        //icon: 'circle',
        //        //itemHeight: 9,//改变圆圈大小
        //        //textStyle: {
        //        //    fontSize: 14,
        //        //    color: '#00FFFF',
        //        //    rich: {
        //        //        b: { color: '#00FFFF' }
        //        //    }
        //        //},
        //        data: [{
        //            name: 'A主',
        //            textStyle: { color: "red" }//设置颜色
        //        },
        //            {
        //                name: 'B主',
        //                textStyle: { color: "#3ADF00" }
        //            },
        //            {
        //                name: 'A备',
        //                textStyle: { color: "#3ADF00" }
        //            },
        //            {
        //                name: 'B备',
        //                textStyle: { color: "#3ADF00" }
        //            },
        //            {
        //                name: 'C通',
        //                textStyle: { color: "#3ADF00" }
        //            }]
        //    },
        //    grid: {
        //        left: '3%',
        //        right: '4%',
        //        bottom: '1%',
        //        containLabel: true
        //    },
        //    xAxis: {
        //        type: 'category',
        //        //设置坐标轴字体颜色和宽度  
        //        axisLine: {
        //            lineStyle: {
        //                color: '#00FFFF',
        //                width: 2
        //            }
        //        },  
        //        data: ['上料1', '上料2', '上料3', '上料4', '上料5', '上料6']
        //    },
        //    yAxis: {
        //        type: 'value',
        //        //设置坐标轴字体颜色和宽度  
        //        axisLine: {
        //            lineStyle: {
        //                color: '#00FFFF',
        //                width: 2
        //            }
        //        },  
        //        min: 0,
        //        max: 2,
        //        splitNumber: 2,
        //    },
        //    series: [
        //        {
        //            name: 'A主',
        //            type: 'bar',
        //            data: [1, 0, 0, 1, 1, 0]
        //        },
        //        {
        //            name: 'B主',
        //            type: 'bar',
        //            itemStyle: { normal: { color: '#3ADF00' } },
        //            data: [1,1,1,0,1,1]
        //        }, {
        //            name: 'A备',
        //            type: 'bar',
        //            itemStyle: { normal: { color: '#0174DF' } },
        //            data: [0,1,1,1,0,0]
        //        }, {
        //            name: 'B备',
        //            type: 'bar',
        //            itemStyle: { normal: { color: 'orange' } },
        //            data: [1,0,1,0,1,0]
        //        }, {
        //            name: 'C通',
        //            type: 'bar',
        //            itemStyle: { normal: { color: 'aqua' } },
        //            data: [0,1,0,1,0,1]
        //        },
        //    ]
        //};
    
        //AGV利用率 agv-chart
        //$($('#agv-chart')).height(($(document).height() - $("#page-inner").children()[0].offsetHeight - $("#page-inner").children()[2].offsetHeight) / 2);
        //var agvChart = echarts.init(document.getElementById('ShipOne'));
        //var agvoption = {
        //   // backgroundColor: 'rgba(128, 128, 128, 0.1)', //rgba设置透明度0.1
        //    title: {
        //        text: 'AGV利用率',
        //        subtext: '08:00~11:00数据统计，单位:分钟',
        //        textStyle: {
        //            // fontWeight: 'normal',
        //            // 其余属性默认使用全局文本样式，详见TEXTSTYLE  
        //            fontWeight: 'bolder',
        //            fontSize: 20,
        //            color: '#00FFFF'
        //        },  
        //    },
        //    grid: {
        //        left: '3%',
        //        right: '4%',
        //        bottom: '3%',
        //        containLabel: true
        //        //x2: 25,
        //        //y2: 20,
        //    },
        //    tooltip: {
        //        trigger: 'axis',
        //        showDelay: 0,
        //        formatter: function (params) {
        //            if (params.value.length > 1) {
        //                return params.seriesName + ' :<br/>'
        //                    + 'AGV' + params.value[0] + ': '
        //                    + params.value[1] + '% ';
        //            }
        //            else {
        //                return params.seriesName + ' :<br/>'
        //                    + params.name + ' : '
        //                    + params.value + '% ';
        //            }
        //        },
        //        axisPointer: {
        //            show: true,
        //            type: 'cross',
        //            lineStyle: {
        //                type: 'dashed',
        //                width: 1
        //            }
        //        }
        //    },
        //    legend: {
        //        data: [{
        //            name: 'AGV利用率',
        //            textStyle: { color: "red" }//设置颜色
        //        }]
        //    },
        //    xAxis: [
        //        {
        //            type: 'value',
        //            scale: true,
        //            axisLine: {
        //                lineStyle: {
        //                    color: '#00FFFF',
        //                    width: 2
        //                }
        //            }, 
        //            axisLabel: {
        //                formatter: 'AGV{value}'
        //            }
        //        }
        //    ],
        //    yAxis: [
        //        {
        //            type: 'value',
        //            axisLine: {
        //                lineStyle: {
        //                    color: '#00FFFF',
        //                    width: 2
        //                }
        //            }, 
        //            scale: true,
        //            axisLabel: {
        //                formatter: '{value} %'
        //            }
        //        }
        //    ],
        //    series: [
        //        {
        //            name: 'AGV利用率',
        //            type: 'scatter',
        //            data: [
        //                [1, 51.6], [2, 59], [3, 49.2], [4, 63], [5, 53.6],
        //                [6, 59], [7, 47.6], [8, 69.8], [9, 66.8], [10, 75.2],
        //                [11, 55.2], [12, 54.2], [13, 62.5], [14, 42], [15, 50],
        //                [16, 49.8], [17, 49.2], [18, 73.2], [19, 47.8], [20, 68.8],
        //                [21, 50.6], [22, 82.5], [23, 57.2], [24, 87.8], [25, 72.8],
        //                [26, 54.5], [27, 59.8], [28, 67.3], [29, 67.8], [30, 47],
        //                [31, 46.2], [32, 55], [33, 83], [34, 54.4], [35, 45.8],
        //                [36, 53.6], [37, 73.2], [38, 52.1], [39, 67.9], [40, 56.6],
        //                [41, 62.3], [42, 58.5], [43, 54.5], [44, 50.2], [45, 60.3],
        //                [46, 58.3], [47, 56.2], [48, 50.2], [49, 72.9], [50, 59.8],
        //                [51, 61], [52, 69.1], [53, 55.9], [54, 46.5], [55, 54.3],
        //                [56, 54.8], [57, 60.7], [58, 29], [59, 62], [60, 60.3],
        //                [61, 52.7], [62, 74.3], [63, 62], [64, 73.1], [65, 80],
        //                [66, 54.7], [67, 53.2], [68, 75.7], [69, 61.1], [70, 55.7],
        //                [71, 48.7], [72, 52.3], [73, 50], [74, 59.3], [75, 62.5],
        //                [76, 55.7], [77, 54.8], [78, 45.9], [79, 70.6], [80, 67.2],
        //                [81, 69.4], [82, 58.2], [83, 64.8], [84, 71.6], [85, 52.8],
        //                [86, 59.8], [87, 49], [88, 50], [89, 69.2], [90, 55.9],
        //                [91, 63.4], [92, 58.2], [93, 58.6], [94, 45.7], [95, 52.2],
        //                [96, 48.6], [97, 57.8], [98, 55.6], [99, 66.8], [100, 59.4],
        //                [101, 53.6], [102, 73.2], [103, 53.4], [104, 69], [105, 58.4],
        //                [106, 56.2], [107, 70.6], [108, 59.8], [109, 72], [110, 65.2],
        //                [111, 56.6], [112, 85.2], [113, 51.8], [114, 63.4], [115, 59],
        //                [116, 47.6], [117, 63], [118, 55.2], [119, 35], [120, 54],
        //            ],
        //        }
        //    ]
        //};

        //agvChart.setOption(agvoption);
        
        //var wipChart = echarts.init(document.getElementById('wip-chart'));
        //wipoption = {
        //  //  backgroundColor: 'rgba(128, 128, 128, 0.1)', //rgba设置透明度0.1
        //    tooltip: {
        //        formatter: "{a} <br/>{c} {b}"
        //    },
        //    toolbox: {
        //        show: true,
        //        feature: {
        //            mark: { show: true },
        //            restore: { show: true },
        //            saveAsImage: { show: true }
        //        }
        //    },
        //    series: [
        //        {
        //            name: '速度',
        //            type: 'gauge',
        //            min: 0,
        //            max: 220,
        //            splitNumber: 11,
        //            radius: '75%',
        //            axisLine: {            // 坐标轴线
        //                lineStyle: {       // 属性lineStyle控制线条样式
        //                    color: [[0.09, 'lime'], [0.82, '#1e90ff'], [1, '#ff4500']],
        //                    width: 3,
        //                    shadowColor: '#fff', //默认透明
        //                    shadowBlur: 10
        //                }
        //            },
        //            axisLabel: {            // 坐标轴小标记
        //                textStyle: {       // 属性lineStyle控制线条样式
        //                    fontWeight: 'bolder',
        //                    color: '#fff',
        //                    shadowColor: '#fff', //默认透明
        //                    shadowBlur: 10
        //                }
        //            },
        //            axisTick: {            // 坐标轴小标记
        //                length: 15,        // 属性length控制线长
        //                lineStyle: {       // 属性lineStyle控制线条样式
        //                    color: 'auto',
        //                    shadowColor: '#fff', //默认透明
        //                    shadowBlur: 10
        //                }
        //            },
        //            splitLine: {           // 分隔线
        //                length: 25,         // 属性length控制线长
        //                lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
        //                    width: 3,
        //                    color: '#fff',
        //                    shadowColor: '#fff', //默认透明
        //                    shadowBlur: 10
        //                }
        //            },
        //            pointer: {           // 分隔线
        //                shadowColor: '#fff', //默认透明
        //                shadowBlur: 5
        //            },
        //            title: {
        //                textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
        //                    fontWeight: 'bolder',
        //                    fontSize: 20,
        //                    fontStyle: 'italic',
        //                    color: '#fff',
        //                    shadowColor: '#fff', //默认透明
        //                    shadowBlur: 10
        //                }
        //            },
        //            detail: {
        //                backgroundColor: 'rgba(30,144,255,0.8)',
        //                borderWidth: 1,
        //                borderColor: '#fff',
        //                shadowColor: '#fff', //默认透明
        //                shadowBlur: 5,
        //                offsetCenter: [0, '45%'],       // x, y，单位px
        //                textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
        //                    fontWeight: 'bolder',
        //                    color: '#fff'
        //                }
        //            },
        //            data: [{ value: 40, name: '个/h' }]
        //        }
        //    ]
        //};
        //wipChart.setOption(wipoption);
        //setInterval(function () {
        //    wipoption.series[0].data[0].value = (Math.random() * 100).toFixed(2) - 0;
        //    //wipoption.series[1].data[0].value = (Math.random() * 7).toFixed(2) - 0;
        //    //wipoption.series[2].data[0].value = (Math.random() * 2).toFixed(2) - 0;
        //    //wipoption.series[3].data[0].value = (Math.random() * 2).toFixed(2) - 0;
        //    wipChart.setOption(wipoption);
        //}, 2000)


    })
});
