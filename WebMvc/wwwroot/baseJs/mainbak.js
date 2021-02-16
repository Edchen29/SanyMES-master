layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'hhweb', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var hhweb = layui.hhweb;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER
    //给按钮添加快捷键
    //Ctrl+I键添加,Ctrl+R键刷新,Ctrl+D键删除,Ctrl+E键编辑,Ctrl+F查询,Ctrl+S保存,Ctrl+A全选,Ctrl+Shift+A取消全选,左 上一页 右 下一页
    $(document).bind("keydown", function (e) {
        //禁用页面F5刷新
        if (e.which == 116) {
            e.preventDefault(); //Skip default behavior of the enter key
        }
    });

    $(document).ready(function () {
        
        function GetDateStr(AddDayCount) {
            var dd = new Date();
            dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
            var y = dd.getFullYear();
            var m = 0;//获取当前月份的日期
            var d = 0;
            m = dd.getMonth() + 1;
            d = dd.getDate();
            if (m >= 10) {
                m = m;
            }
            else {
                m = "0" + m;
            }
            if (d >= 10) {
                d = d;
            }
            else {
                d = "0" + d;
            }
            return y + "-" + m + "-" + d;
        }

        var timeTicket = setInterval(function () {
            window.location.reload();
        }, 90000)
        clearInterval(timeTicket);

        var receipt = 0;
        var shipment = 0;
        //图三:待维修概况
        $.ajax({
            url: "/base/EchartsData/GetRepairInfo",
            data: {},
            type: "POST",
            dataType: "json",
            async: true,
            success: function (data) {
                //没有数据时提示
                //if (data.hasOwnProperty("type")) {
                //    dialogMsg(data.message, 0);
                //    return;
                //}

                var myChart = echarts.init(document.getElementById('chart3'));
                //myChart.showLoading({
                //    text: '正在加载中......',
                //    effect: 'whirling'
                //});
                var taskoption = {
                   // backgroundColor: '#06284e',
                    title: {
                        text: '待维修事务统计',
                        subtext: '单位：个',

                    },
                    tooltip: {
                        formatter: "{a} <br/>{c} {b}"
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            mark: { show: true },
                            restore: { show: true },
                            saveAsImage: { show: true }
                        }
                    },
                    series: [
                        {
                            name: '等待维修的工单数：',
                            type: 'gauge',
                            center: ['10%', '25%'],    // 默认全局居中
                            z: 3,
                            min: 0,
                            max: 100,
                            splitNumber: 10,
                            radius: '30%',
                            axisLine: {            // 坐标轴线
                                lineStyle: {       // 属性lineStyle控制线条样式
                                    color: [[0.29, 'lime'], [0.86, '#1e90ff'], [1, '#ff4500']],
                                    width: 2,
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            axisTick: {            // 坐标轴小标记
                                length: 12,        // 属性length控制线长
                                lineStyle: {       // 属性lineStyle控制线条样式
                                    color: 'auto',
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            splitLine: {           // 分隔线
                                length: 20,         // 属性length控制线长
                                lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                    width: 3,
                                    color: '#fff',
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            axisLabel: {
                                backgroundColor: 'auto',
                                borderRadius: 2,
                                color: '#eee',
                                padding: 3,
                                textShadowBlur: 2,
                                textShadowOffsetX: 1,
                                textShadowOffsetY: 1,
                                textShadowColor: '#222'
                            },
                            title: {
                                textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                    fontWeight: 'bolder',
                                    fontSize: 15,
                                    fontStyle: 'italic'
                                }
                            },
                            detail: {
                                // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                //formatter: function (value) {
                                //    value = (value + '').split('.');
                                //    value.length < 2 && (value.push('00'));
                                //    return ('00' + value[0]).slice(-2)
                                //        + '.' + (value[1] + '00').slice(0, 2);
                                //},
                                fontWeight: 'bolder',
                                borderRadius: 3,
                                backgroundColor: '#444',
                                borderColor: '#aaa',
                                shadowBlur: 5,
                                shadowColor: '#333',
                                shadowOffsetX: 0,
                                shadowOffsetY: 3,
                                borderWidth: 2,
                                textBorderColor: '#000',
                                textBorderWidth: 2,
                                textShadowBlur: 2,
                                textShadowColor: '#fff',
                                textShadowOffsetX: 0,
                                textShadowOffsetY: 0,
                                fontFamily: 'Arial',
                                width: 80,
                                color: '#eee',
                                rich: {}
                            },
                            data: [{ value: data.data[0].wo, name: '工单数' }]
                        },
                        {
                            shadowColor: '#fff', //默认透明
                            name: '等待维修数量：',
                            type: 'gauge',
                            center: ['28%', '25%'],    // 默认全局居中
                            radius: '30%',
                            min: 0,
                            max: 100,
                            //endAngle: 1,
                            splitNumber: 10,
                            axisLine: {            // 坐标轴线
                                lineStyle: {       // 属性lineStyle控制线条样式
                                    color: [[0.09, 'lime'], [0.82, '#1e90ff'], [1, '#ff4500']],
                                    width: 5,
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            axisTick: {            // 坐标轴小标记
                                length: 15,        // 属性length控制线长
                                lineStyle: {       // 属性lineStyle控制线条样式
                                    color: 'auto',
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            splitLine: {           // 分隔线
                                length: 25,         // 属性length控制线长
                                lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                    width: 3,
                                    color: '#fff',
                                    shadowColor: '#fff', //默认透明
                                    shadowBlur: 10
                                }
                            },
                            pointer: {
                                width: 5
                            },
                            title: {
                                offsetCenter: [0, '-30%'],       // x, y，单位px
                            },
                            detail: {
                                textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                    fontWeight: 'bolder'
                                }
                            },
                            data: [{ value: data.data[0].repair, name: '待维修数' }]
                        }
                    ]
                };

                myChart.setOption(taskoption);
                //myChart.hideLoading();
            },
            //error: function (XMLHttpRequest, textStatus, errorThrown) {
            //    layer.alert(errorThrown, { icon: 2, title: '提示' });
            //}
        });

    })
});
