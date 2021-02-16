var refreshMapTimer, mouseOnpointTimer
var LocationMap, PointMap, ContainersInfo, CarInfo;
var angle = 90;
var RoadWay = 1;

var locationc = "blue"; //点颜色
var carc = "red"; //小车颜色
var shelfc = "green"; //货架颜色

var mappixSize = 38;
var locationpixSize = mappixSize * 0.9;
var containerpixSize = mappixSize * 0.8;

var eachWidth = mappixSize / 1.2 / 1.2 / 1.2; //单元格大小
var pointr = 3 / 1.2 / 1.2 / 1.2; //点的半径为3

var locationw = locationpixSize / 1.2 / 1.2 / 1.2; //库位宽度
var locationh = locationpixSize / 1.2 / 1.2 / 1.2; //库位高度
var locationb = 1 / 1.2 / 1.2 / 1.2; //库位边框宽度

var carr = 18 / 1.2 / 1.2 / 1.2; //小车半径
var carb = 4 / 1.2 / 1.2;       //小车线条宽度

var containerw = containerpixSize / 1.2 / 1.2 / 1.2; //容器宽度
var containerh = containerpixSize / 1.2 / 1.2 / 1.2; //容器高度
var containerb = 2 / 1.2 / 1.2 / 1.2; //容器边框宽度

var MapWidth, MapHeight;
var MaxRow, MaxLine, MaxLayer;

$(document).ready(function () {
    LocationMap = $("#LocationMap");
    PointMap = $("#PointMap");
    ContainersInfo = $("#ContainersInfo");
    CarInfo = $("#CarInfo");

    if (window.localStorage.getItem("RoadWay") != undefined) {
        RoadWay = window.localStorage.getItem("RoadWay");
    }

    BindRoadWay();
    //加载并刷新地图
    DrawMap();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/ChartHub", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();

    connection.on("sendAgvData", function (json) {
        var x = json.X; //AGV x坐标
        x = x % MaxRow + 2
        if (MaxLine == 1) {
            RefreshCar({ X: x, Y: 1, AgvNo: 'Test' });
        }
        else {
            RefreshCar({ X: x, Y: (MaxLine / 2) * (MaxLayer + 1) + 1, AgvNo: 'Test' });
        }
    });

    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });
});

function Rotate() {
    angle -= 90;
    if (angle < -360) { angle = -90 };
    $("#mycanvas").rotate({
        angle: angle,
        center: ["50%", "50%"],
        animateTo: angle - 90,
    })
}

//获取所有巷道
function BindRoadWay() {
    $.post("/monitor/LocationMonitor/GetRoadWays",
        null,
        function (result) {
            if (result.count > 0) {
                $("#RoadWays").html("");
                var doms = "";
                var RoadWayExist = false;
                for (var i = 0; i < result.data.length; i++) {
                    doms += '<a id = btnRoadway' + result.data[i] + ' herf="javascript:;" class="layui-btn layui-btn-sm layui-btn-warm" onclick="javascript: Change(' + result.data[i] + ');" title="切换">' + result.data[i] + '号巷道</a>';
                    if (RoadWay == result.data[i]) {
                        RoadWayExist = true;
                    }
                }
                doms += '<a herf="javascript:;" class="layui-btn layui-btn-sm layui-btn-normal" id="btnRefresh" onclick="javascript: Refresh();" title="刷新"><i class="layui-icon">&#xe669;</i>刷新</a>';

                if (!RoadWayExist) {
                    RoadWay = result.data[0];
                }
                $("#RoadWays").html(doms);

                $('#btnRoadway' + RoadWay).css("background-color", 'green');
            } else {
                layer.msg(result.msg);
            }
        },
        "json");
}

//切换地图
function Change(iRoadWay) {
    layer.load();
    window.localStorage.setItem("RoadWay", iRoadWay);
    window.location.reload(true)
    layer.closeAll();
}

//画地图,并刷新地图
function DrawMap() {
    var WidthMax = 35; //行最大值
    var HeightMax = 20 + (4 - 1); //一行共多少仓位 + (列数 - 1) //每列间隔开
    var WidthMin = 1;  //固定值
    var HeightMin = 1; //固定值

    //获取地图X,Y轴最大值
    $.post("/monitor/LocationMonitor/GetRoadWayConfig",
        { roadway: RoadWay },
        function (result) {
            MaxRow = result.data.MaxRow;
            MaxLine = result.data.MaxLine;
            MaxLayer = result.data.MaxLayer;

            WidthMax = MaxRow + 1;
            HeightMax = (MaxLine * MaxLayer) + (MaxLine - 1) + 2;

            var maxX = WidthMax + 1;
            var maxY = HeightMax + 1;
            var minX = WidthMin - 1;
            var minY = HeightMin - 1;

            MapWidth = (maxX - minX) * mappixSize / 1.728 + 12;
            MapHeight = (maxY - minY) * mappixSize / 1.728 + 12;

            LocationMap.attr("width", MapWidth);
            LocationMap.attr("height", MapHeight - eachWidth / 1.9);
            PointMap.attr("width", MapWidth);
            PointMap.attr("height", MapHeight - eachWidth / 1.9);
            ContainersInfo.attr("width", MapWidth);
            ContainersInfo.attr("height", MapHeight - eachWidth / 1.9);
            CarInfo.attr("width", MapWidth);
            CarInfo.attr("height", MapHeight - eachWidth / 1.9);

            $(PointMap).removeLayer().drawLayers();
            $(PointMap).clearCanvas();

            for (var i = 1; i <= MaxRow; i++) {
                var x = GetAdjustX(i + 1) * eachWidth;
                var y = GetAdjustY(MaxLine * (MaxLayer + 1) + 1) * eachWidth + MapHeight;
                $(PointMap).drawText({
                    name: 'PointR' + i,
                    fillStyle: 'blue',
                    strokeWidth: 2 / (90 / mappixSize),
                    x: x, y: y,
                    fontSize: 20 / (90 / mappixSize) + 'px',
                    fontFamily: 'Verdana, sans-serif',
                    text: i
                })
                    .drawArc({
                        strokeStyle: 'green',
                        strokeWidth: 4,
                        x: x, y: y,
                        radius: 18 / (90 / mappixSize)
                    });
            }

            for (var i = 1; i <= MaxLine; i++) {
                var x = eachWidth;
                var y = GetAdjustY((MaxLine - i) * (MaxLayer + 1) + 1 + MaxLayer / 2 + 0.5) * eachWidth + MapHeight;
                $(PointMap).drawText({
                    name: 'PointL' + i,
                    fillStyle: 'blue',
                    strokeWidth: 2 / (90 / mappixSize),
                    x: x, y: y,
                    fontSize: 20 / (90 / mappixSize) + 'px',
                    fontFamily: 'Verdana, sans-serif',
                    text: i
                })
                    .drawArc({
                        strokeStyle: 'red',
                        strokeWidth: 4,
                        x: x, y: y,
                        radius: 18 / (90 / mappixSize)
                    });
            }

            //刷新地图
            Refresh();
        },
        "json");
}

//刷新地图
function Refresh() {
    layer.load();
    //加载库位
    CreateLocationMap();

    //加载堆垛机
    if (MaxLine == 1) {
        RefreshCar({ X: MaxRow / 2 + 1, Y: 1, AgvNo: 'Test' });
    }
    else {
        RefreshCar({ X: MaxRow / 2 + 1, Y: (MaxLine / 2) * (MaxLayer + 1) + 1, AgvNo: 'Test' });
    }
    layer.closeAll();
}

//启停自动刷新
function StartStop() {
    if (refreshMapTimer != undefined) {
        window.clearInterval(refreshMapTimer);
        refreshMapTimer = undefined;
    }
    refreshMapTimer = setInterval(function () {
        Refresh();
    }, 1000);
}

//获取地图库位数据
function CreateLocationMap() {
    $.post("/monitor/LocationMonitor/GetLocations",
        { roadway: RoadWay },
        function (result) {
            if (result.count > 0) {
                DrawLocationMap(result.data);
            } else {
                layer.msg(result.msg);
            }
        },
        "json");
}

//画库位
function DrawLocationMap(data) {
    for (var i = 0; i < data.length; i++) {
        var irow = data[i].Row;
        var iline = data[i].Column;
        var ilayer = data[i].Layer;
        var irowindex = data[i].RowIndex;
        var locationname = data[i].Code;
        var iStatus = data[i].Status;

        var locationx = GetAdjustX(irow + 1);
        var locationy = GetAdjustY((MaxLine - iline) * (MaxLayer + 1) + ilayer + 1);

        var content = {
            "Row": irow,
            "Column": iline,
            "Layer": ilayer,
            "RowIndex": irowindex,
            "Code": locationname,
            "Status": iStatus,
        };

        var locationLayer = $(LocationMap).getLayer(locationname);
        var imgsrc = '/images/container/空柜空闲.png';

        if (iStatus == "empty") {
            imgsrc = '/images/container/空柜空闲.png';
        }
        else if (iStatus == "some") {
            imgsrc = '/images/container/半盘空闲.png';
        }
        else if (iStatus == "full") {
            imgsrc = '/images/container/整盘空闲.png';
        }
        else if (iStatus == "lock") {
            imgsrc = '/images/container/空柜锁定.png';
        }
        else if (iStatus == "emptycontainer") {
            imgsrc = '/images/container/空盘空闲.png';
        }
        
        if (locationLayer == undefined) {
            $(LocationMap).drawImage({
                layer: true,
                name: locationname,
                source: imgsrc,
                x: locationx * eachWidth - (eachWidth / 2), y: locationy * eachWidth - (eachWidth / 2) + MapHeight,
                fromCenter: false,
                width: locationw * 1.1,
                height: locationh * 1.1,
                data: content,
                shadowColor: '#222',
                shadowBlur: 1,
            });

            $(ContainersInfo).drawRect({
                layer: true,
                name: locationname,
                fillStyle: "transparent",
                x: locationx * eachWidth, y: locationy * eachWidth + MapHeight,
                width: locationw * 1.1,
                height: locationh * 1.1,
                data: content,
                click: function (canvaslayer) {
                    //$(LocationMap).animateLayer(canvaslayer.name, {
                    //    shadowBlur: 20,
                    //}, 10, null);
                    $(LocationMap).getLayer(locationname).shadowBlur = 20;
                    $(LocationMap).drawLayers();

                    var msgLayer = $(LocationMap).getLayer(canvaslayer.name);
                    var msg = "&emsp;库位编码:&nbsp" + msgLayer['data']['Code'] + "<br/>";
                    msg += "&emsp;行:&nbsp" + msgLayer['data']['Row'] + "&emsp;";
                    msg += "列:&nbsp" + msgLayer['data']['Column'] + "&emsp;";
                    msg += "层:&nbsp" + msgLayer['data']['Layer'] + "&emsp;<br/>";
                    msg += "&emsp;双深位索引:&nbsp" + msgLayer['data']['RowIndex'] + "&emsp;";
                    msg += "状态:&nbsp" + ContainerStatus(msgLayer['data']['Status']) + "<br/>";
                    layer.msg(msg, { time: 0, offset: [canvaslayer.y - eachWidth - 30 - $(document).scrollTop(), canvaslayer.x + eachWidth + 30] });
                },
                mouseover: function (canvaslayer) {
                    //mouseOnpointTimer = setTimeout(function () {
                    //    $(LocationMap).animateLayer(canvaslayer.name, {
                    //        shadowBlur: 20,
                    //    }, 0, null);

                        var msgLayer = $(LocationMap).getLayer(canvaslayer.name);
                        var msg = "&emsp;库位编码:&nbsp" + msgLayer['data']['Code'] + "<br/>";
                        msg += "&emsp;行:&nbsp" + msgLayer['data']['Row'] + "&emsp;";
                        msg += "列:&nbsp" + msgLayer['data']['Column'] + "&emsp;";
                        msg += "层:&nbsp" + msgLayer['data']['Layer'] + "&emsp;<br/>";
                        msg += "&emsp;双深位索引:&nbsp" + msgLayer['data']['RowIndex'] + "&emsp;";
                        msg += "状态:&nbsp" + ContainerStatus(msgLayer['data']['Status']) + "<br/>";
                        layer.msg(msg, { time: 0, offset: [canvaslayer.y - eachWidth - 30 - $(document).scrollTop(), canvaslayer.x + eachWidth + 30] });
                    //}, 10);
                },
                mouseout: function (canvaslayer) {
                    //if (mouseOnpointTimer != undefined) {
                    //    window.clearInterval(mouseOnpointTimer);
                    //    mouseOnpointTimer = undefined;
                    //}

                    //$(LocationMap).animateLayer(canvaslayer.name, {
                    //    shadowBlur: 1,
                    //}, 0, null);

                    layer.closeAll();
                },
            })
        }
        else {
            $(LocationMap).removeLayer(locationname);

            $(LocationMap).drawImage({
                layer: true,
                name: locationname,
                source: imgsrc,
                x: locationx * eachWidth - (eachWidth / 2), y: locationy * eachWidth - (eachWidth / 2) + MapHeight,
                fromCenter: false,
                width: locationw * 1.1,
                height: locationh * 1.1,
                data: content,
                shadowColor: '#222',
                shadowBlur: 1,
            });
        }
    }
}

//获取小车数据
function CreateCars() {
    $.post("/Acs/AcsMap/GetCars",
        null,
        function (data) {
            if (data.Status) {
                RefreshCars(data.Result);
            } else {
                layer.msg(data.Message);
            }
        },
        "json");
}

//刷新小车
function RefreshCars(data) {
    for (var i = 0; i < data.length; i++) {
        RefreshCar(data[i])
    }
}

//刷新单个小车
function RefreshCar(data) {
    var carname = data.AgvNo;

    var carLayer = $(CarInfo).getLayer(carname);
    if (carLayer == undefined) {
        DrawCar(data);
    }
    else {
        EraseCar(data);
        DrawCar(data);
    }
}

//画小车
function DrawCar(data) {
    var carx = GetAdjustX(data.X);
    var cary = GetAdjustY(data.Y);
    var carname = data.AgvNo;

    var imgsrc = "/images/car.png";

    $(CarInfo).drawImage({
        layer: true,
        name: carname,
        source: imgsrc,
        x: carx * eachWidth - (eachWidth / 2), y: cary * eachWidth - (eachWidth / 2) + MapHeight,
        fromCenter: false,
        width: eachWidth * 0.9,
        height: eachWidth * 0.9,
        shadowColor: '#222',
        shadowBlur: 5,
    });
}

//移除小车
function EraseCar(data) {
    $(CarInfo).removeLayer(data.AgvNo).drawLayers();
}

//地图X坐标调整
function GetAdjustX(x) {
    //return x - 14;
    return x;
}

//地图Y坐标调整
function GetAdjustY(y) {
    //return y * -1 + 100;
    return y * -1;
}

//获取容器状态
function ContainerStatus(data) {
    if (data == "some") {
        return "半盘";
    }
    else if (data == "full") {
        return "整盘";
    }
    else {
        return "空盘";
    }
}