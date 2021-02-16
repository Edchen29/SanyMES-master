namespace WebRepository
{
    public static class EnumHelper
    {
        public static string Value(this EnumBusinessType enumValue)
        {
            switch (enumValue)
            {
                case EnumBusinessType.入库_调整单:
                    return "AI";
                case EnumBusinessType.入库_采购到货单:
                    return "PI";
                case EnumBusinessType.入库_产成品入库单:
                    return "SI";
                case EnumBusinessType.入库_销售退货单:
                    return "SRI";
                case EnumBusinessType.入库_调拨入库单:
                    return "TI";
                case EnumBusinessType.入库_手持入库单:
                    return "PPI";
                case EnumBusinessType.入库_其他入库单:
                    return "QI";
                case EnumBusinessType.入库_生产退料单:
                    return "CI";

                case EnumBusinessType.出库_质检出库单:
                    return "CO";
                case EnumBusinessType.出库_加工出库单:
                    return "OO";
                case EnumBusinessType.出库_采购退货单:
                    return "PRO";
                case EnumBusinessType.出库_销售发货单:
                    return "SO";
                case EnumBusinessType.出库_调拨出库单:
                    return "TO";
                case EnumBusinessType.出库_手持出库单:
                    return "PO";
                case EnumBusinessType.出库_材料出库单:
                    return "MO";
                case EnumBusinessType.出库_其他出库单:
                    return "QO";
                case EnumBusinessType.出库_中间区出库单:
                    return "ZO";
                case EnumBusinessType.出库_盘点出库:
                    return "OUT";
                default:
                    return " ";
            }
        }

        public static string Value(this EnumTaskType enumValue)
        {
            switch (enumValue)
            {
                case EnumTaskType.整盘入库:
                    return "100";
                case EnumTaskType.补充入库:
                    return "200";
                case EnumTaskType.整盘出库:
                    return "300";
                case EnumTaskType.分拣出库:
                    return "400";
                case EnumTaskType.空容器入库:
                    return "500";
                case EnumTaskType.空容器出库:
                    return "600";
                case EnumTaskType.盘点:
                    return "700";
                case EnumTaskType.移库:
                    return "800";
                case EnumTaskType.出库查看:
                    return "900";
                case EnumTaskType.容器出库:
                    return "999";
                case EnumTaskType.容器回库:
                    return "1000";
                default:
                    return " ";
            }
        }
    }
    public static class EquipmentStart
    {
        /// <summary>
        /// 组对1 = "AssMachine1"
        /// </summary>
        public static readonly string 组对1 = "AssMachine1";
        /// <summary>
        /// 组对2 = "AssMachine2"
        /// </summary>
        public static readonly string 组对2 = "AssMachine2";
        /// <summary>
        /// 组对3 = "AssMachine3"
        /// </summary>
        public static readonly string 组对3 = "AssMachine3";
        /// <summary>
        /// 组对4 = "AssMachine4"
        /// </summary>
        public static readonly string 组对4 = "AssMachine4";
        /// <summary>
        /// 组对5 = "AssMachine5"
        /// </summary>
        public static readonly string 组对5 = "AssMachine5";
        /// <summary>
        /// 组对6 = "AssMachine6"
        /// </summary>
        public static readonly string 组对6 = "AssMachine6";
        /// <summary>
        /// 桁车1 = "Truss-Car001"
        /// </summary>
        public static readonly string 桁车1 = "Truss-Car001";
        /// <summary>
        /// 桁车2 = "Truss-Car002"
        /// </summary>
        public static readonly string 桁车2 = "Truss-Car002";
        /// <summary>
        /// 桁车3 = "Truss-Car003"
        /// </summary>
        public static readonly string 桁车3 = "Truss-Car003";
    }
    public static class CallStatus
    {
        /// <summary>
        /// 已准备 = "ready"
        /// </summary>
        public static readonly string 已准备 = "ready";
        /// <summary>
        /// 完成 = "done"
        /// </summary>
        public static readonly string 完成 = "done";
        /// <summary>
        /// 执行 = "doing"
        /// </summary>
        public static readonly string 执行 = "doing";
    }
    public static class CallType
    {
        /// <summary>
        /// 上料 = "feeding"
        /// </summary>
        public static readonly string 上料 = "feeding";
        /// <summary>
        /// 下料 = "download"
        /// </summary>
        public static readonly string 下料 = "download";
    }
    public static class TaskType
    {
        /// <summary>
        /// 上料配送 = "feeding"
        /// </summary>
        public static readonly string 上料配送 = "feeding";
        /// <summary>
        /// 回收料框  = "take"
        /// </summary>
        public static readonly string 回收料框 = "take";
        /// <summary>
        /// 下料取件 = "download"
        /// </summary>
        public static readonly string 下料取件 = "download";
        /// <summary>
        /// 补给料框 = "put"
        /// </summary>
        public static readonly string 补给料框 = "put";
    }
    public static class OrderType
    {
        /// <summary>
        /// 正常工单 = "normal"
        /// </summary>
        public static readonly string 正常工单 = "normal";
        /// <summary>
        /// 重整工单 = "reform"
        /// </summary>
        public static readonly string 重整工单 = "reform";
        /// <summary>
        /// 报废工单 = "scrap"
        /// </summary>
        public static readonly string 报废工单 = "scrap";
    }
    public static class OrderStatus
    {
        /// <summary>
        /// 已准备 = "ready"
        /// </summary>
        public static readonly string 已准备 = "ready";
        /// <summary>
        /// 暂停 = "pause"
        /// </summary>
        public static readonly string 暂停 = "pause";
        /// <summary>
        /// 完成 = "done"
        /// </summary>
        public static readonly string 完成 = "done";
        /// <summary>
        /// 执行 = "doing"
        /// </summary>
        public static readonly string 执行 = "doing";
        /// <summary>
        /// 已撤消 = "cancel"
        /// </summary>
        public static readonly string 已撤消 = "cancel";
        /// <summary>
        /// 已配料 = "picked"
        /// </summary>
        public static readonly string 已配料 = "picked";
        /// <summary>
        /// 已冻结 = "frozen"
        /// </summary>
        public static readonly string 已冻结 = "frozen";
    }
    public static class BusinessType
    {
        /// <summary>
        /// 入库_调整单 = "AI";
        /// </summary>
        public static readonly string 入库_调整单 = "AI";
        /// <summary>
        /// 入库_采购到货单 = "PI";
        /// </summary>
        public static readonly string 入库_采购到货单 = "PI";
        /// <summary>
        /// 入库_产成品入库单 = "SI";
        /// </summary>
        public static readonly string 入库_产成品入库单 = "SI";
        /// <summary>
        /// 入库_销售退货单 = "SRI";
        /// </summary>
        public static readonly string 入库_销售退货单 = "SRI";
        /// <summary>
        /// 入库_调拨入库单 = "TI";
        /// </summary>
        public static readonly string 入库_调拨入库单 = "TI";
        /// <summary>
        /// 入库_手持入库单 = "AI";
        /// </summary>
        public static readonly string 入库_手持入库单 = "AI";
        /// <summary>
        /// 入库_其他入库单 = "QI";
        /// </summary>
        public static readonly string 入库_其他入库单 = "QI";
        /// <summary>
        /// 入库_生产退料单 = "CI";
        /// </summary>
        public static readonly string 入库_生产退料单 = "CI";
        /// <summary>
        /// 出库_质检出库单 = "CO";
        /// </summary>
        public static readonly string 出库_质检出库单 = "CO";
        /// <summary>
        /// 出库_加工出库单 = "OO";
        /// </summary>
        public static readonly string 出库_加工出库单 = "OO";
        /// <summary>
        /// 出库_采购退货单 = "PRO";
        /// </summary>
        public static readonly string 出库_采购退货单 = "PRO";
        /// <summary>
        /// 出库_销售发货单 = "SO";
        /// </summary>
        public static readonly string 出库_销售发货单 = "SO";
        /// <summary>
        /// 出库_调拨出库单 = "TO";
        /// </summary>
        public static readonly string 出库_调拨出库单 = "TO";
        /// <summary>
        /// 出库_手持出库单 = "PO";
        /// </summary>
        public static readonly string 出库_手持出库单 = "PO";
        /// <summary>
        /// 出库_材料出库单 = "MO";
        /// </summary>
        public static readonly string 出库_材料出库单 = "MO";
        /// <summary>
        /// 出库_其他出库单 = "QO";
        /// </summary>
        public static readonly string 出库_其他出库单 = "QO";
        /// <summary>
        /// 出库_中间区出库单 = "ZO";
        /// </summary>
        public static readonly string 出库_中间区出库单 = "ZO";
        /// <summary>
        /// 出库_盘点出库 = "OUT";
        /// </summary>
        public static readonly string 出库_盘点出库 = "OUT";
    }

    //public static class TaskType
    //{
    //    /// <summary>
    //    /// 整盘入库 = "100"
    //    /// </summary>
    //    public static readonly string 整盘入库 = "100";
    //    /// <summary>
    //    /// 补充入库 = "200"
    //    /// </summary>
    //    public static readonly string 补充入库 = "200";
    //    /// <summary>
    //    /// 整盘出库 = "300"
    //    /// </summary>
    //    public static readonly string 整盘出库 = "300";
    //    /// <summary>
    //    /// 分拣出库 = "400"
    //    /// </summary>
    //    public static readonly string 分拣出库 = "400";
    //    /// <summary>
    //    /// 空容器入库 = "500"
    //    /// </summary>
    //    public static readonly string 空容器入库 = "500";
    //    /// <summary>
    //    /// 空容器出库 = "600"
    //    /// </summary>
    //    public static readonly string 空容器出库 = "600";
    //    /// <summary>
    //    /// 盘点 = "700"
    //    /// </summary>
    //    public static readonly string 盘点 = "700";
    //    /// <summary>
    //    /// 移库 = "800"
    //    /// </summary>
    //    public static readonly string 移库 = "800";
    //    /// <summary>
    //    /// 出库查看 = "900"
    //    /// </summary>
    //    public static readonly string 出库查看 = "900";
    //    /// <summary>
    //    /// 容器出库 = "999"
    //    /// </summary>
    //    public static readonly string 容器出库 = "999";
    //    /// <summary>
    //    /// 容器回库 = "1000"
    //    /// </summary>
    //    public static readonly string 容器回库 = "1000";
    //}
    public static class AGVTaskNo
    {
        /// <summary>
        /// 工位叫料 = "F"
        /// </summary>
        public static readonly string 工位叫料 = "F";
        /// <summary>
        /// 取空料框 = "T"
        /// </summary>
        public static readonly string 取空料框 = "T";
        /// <summary>
        /// 工位叫料 = "D"
        /// </summary>
        public static readonly string 成品下料 = "D";
        /// <summary>
        /// 补给空框 = "P"
        /// </summary>
        public static readonly string 补给空框 = "P";
    }
    public static class TaskNo
    {
        //新增带货入库的任务编号都要以I开头

        /// <summary>
        /// 空板补充 = "KB"
        /// </summary>
        public static readonly string 空板补充 = "KB";
        /// <summary>
        /// 容器出库 = "POUT"
        /// </summary>
        public static readonly string 容器出库 = "POUT";
        /// <summary>
        /// 容器回库 = "PIN"
        /// </summary>
        public static readonly string 容器回库 = "PIN";
        /// <summary>
        /// 查看容器出库 = "COUT"
        /// </summary>
        public static readonly string 查看容器出库 = "COUT";
        /// <summary>
        /// 查看容器回库 = "CIN"
        /// </summary>
        public static readonly string 查看容器回库 = "CIN";
        /// <summary>
        /// 出库自动分配 = "O"
        /// </summary>
        public static readonly string 出库自动分配 = "O";
        /// <summary>
        /// 入库自动分配 = "I"
        /// </summary>
        public static readonly string 入库自动分配 = "I";
        /// <summary>
        /// 出库手动分配 = "OM"
        /// </summary>
        public static readonly string 出库手动分配 = "OM";
        /// <summary>
        /// 入库手动分配 = "IM"
        /// </summary>
        public static readonly string 入库手动分配 = "IM";
        /// <summary>
        /// 空托盘入库 = "K"
        /// </summary>
        public static readonly string 空托盘入库 = "K";
    }

    public static class ContainerStatus
    {
        /// <summary>
        /// 空 = "empty"
        /// </summary>
        public static readonly string 空 = "empty";
        /// <summary>
        /// 有 = "some"
        /// </summary>
        public static readonly string 有 = "some";
        /// <summary>
        /// 满 = "full"
        /// </summary>
        public static readonly string 满 = "full";
    }
    public static class MaterialConfirm
    {
        /// <summary>
        /// 已通知 = 0
        /// </summary>
        public static readonly int 已通知 = 0;
        /// <summary>
        /// 未确认 = 1
        /// </summary>
        public static readonly int 未确认 = 1;
        /// <summary>
        /// 已确认 = 2
        /// </summary>
        public static readonly int 已确认 = 2;
    }
    public static class ContainerLock
    {
        /// <summary>
        /// 未锁 = 0
        /// </summary>
        public static readonly short? 未锁 = 0;
        /// <summary>
        /// 任务锁 = 1
        /// </summary>
        public static readonly short? 任务锁 = 1;
        /// <summary>
        /// 盘点锁 = 2
        /// </summary>
        public static readonly short? 盘点锁 = 2;
    }

    public static class LocationStatus
    {
        /// <summary>
        /// 禁用 = "disable"
        /// </summary>
        public static readonly string 禁用 = "disable";
        /// <summary>
        /// 空仓位 = "empty"
        /// </summary>
        public static readonly string 空仓位 = "empty";
        /// <summary>
        /// 任务锁定中... = "lock"
        /// </summary>
        public static readonly string 任务锁定中 = "lock";
        /// <summary>
        /// 有货 = "some"
        /// </summary>
        public static readonly string 有货 = "some";
        /// <summary>
        /// 已满 = "full"
        /// </summary>
        public static readonly string 已满 = "full";
        /// <summary>
        /// 空容器 = "emptycontainer"
        /// </summary>
        public static readonly string 空容器 = "emptycontainer";
        /// <summary>
        /// 入库占用 = "receiptLock"
        /// </summary>
        public static readonly string 入库占用 = "receiptLock";
    }

    public static class InventoryStatus
    {
        /// <summary>
        /// 次品 = "defective"
        /// </summary>
        public static readonly string 次品 = "defective";
        /// <summary>
        /// 待确认 = "discussed"
        /// </summary>
        public static readonly string 待确认 = "discussed";
        /// <summary>
        /// 良品 = "good"
        /// </summary>
        public static readonly string 良品 = "good";
        /// <summary>
        /// 报废品 = "scrap"
        /// </summary>
        public static readonly string 报废品 = "scrap";
    }

    public static class ReceiptContainerHeaderStatus
    {
        /// <summary>
        /// 新建 = 0
        /// </summary>
        public static readonly int 新建 = 0;
        /// <summary>
        /// 开始上架 = 10
        /// </summary>
        public static readonly int 开始上架 = 10;
        /// <summary>
        /// 上架完成 = 20
        /// </summary>
        public static readonly int 上架完成 = 20;
    }

    public static class ReceiptHeaderStatus
    {
        /// <summary>
        /// 新建 = 0
        /// </summary>
        public static readonly short? 新建 = 0;
        /// <summary>
        /// 订单池 = 100
        /// </summary>
        public static readonly short? 订单池 = 100;
        /// <summary>
        /// 入库预约 = 120
        /// </summary>
        public static readonly short? 入库预约 = 120;
        /// <summary>
        /// 分配完成 = 130
        /// </summary>
        public static readonly short? 分配完成 = 130;
        /// <summary>
        /// 入库到货 = 150
        /// </summary>
        public static readonly short? 入库到货 = 150;
        /// <summary>
        /// 入库质检 = 180
        /// </summary>
        public static readonly short? 入库质检 = 180;
        /// <summary>
        /// 收货 = 200
        /// </summary>
        public static readonly short? 收货 = 200;
        /// <summary>
        /// 定位 = 240
        /// </summary>
        public static readonly short? 定位 = 240;
        /// <summary>
        /// 上架 = 300
        /// </summary>
        public static readonly short? 上架 = 300;
        /// <summary>
        /// 过账 = 800
        /// </summary>
        public static readonly short? 过账 = 800;
        /// <summary>
        /// 回传 = 900
        /// </summary>
        public static readonly short? 回传 = 900;
        /// <summary>
        /// 空托回库 = 1000
        /// </summary>
        public static readonly short? 空托回库 = 1000;
    }

    public static class ShipmentContainerHeaderStatus
    {
        /// <summary>
        /// 新建 = 0
        /// </summary>
        public static readonly int 新建 = 0;
        /// <summary>
        /// 生成任务 = 10
        /// </summary>
        public static readonly int 生成任务 = 10;
        /// <summary>
        /// 拣货完成 = 20
        /// </summary>
        public static readonly int 拣货完成 = 20;
        /// <summary>
        /// 复核完成 = 30
        /// </summary>
        public static readonly int 复核完成 = 30;
    }

    public static class ShipmentHeaderStatus
    {
        /// <summary>
        /// 新建 = 0
        /// </summary>
        public static readonly short? 新建 = 0;
        /// <summary>
        /// 订单池 = 100
        /// </summary>
        public static readonly short? 订单池 = 100;
        /// <summary>
        /// 出库预约 = 120
        /// </summary>
        public static readonly short? 出库预约 = 120;
        /// <summary>
        /// 订单分析 = 150
        /// </summary>
        public static readonly short? 订单分析 = 150;
        /// <summary>
        /// 波次 = 200
        /// </summary>
        public static readonly short? 波次 = 200;
        /// <summary>
        /// 分配完成 = 250
        /// </summary>
        public static readonly short? 分配完成 = 250;
        /// <summary>
        /// 拣货完成 = 300
        /// </summary>
        public static readonly short? 拣货完成 = 300;
        /// <summary>
        /// 称重 = 400
        /// </summary>
        public static readonly short? 称重 = 400;
        /// <summary>
        /// 复核 = 500
        /// </summary>
        public static readonly short? 复核 = 500;
        /// <summary>
        /// 装载 = 600
        /// </summary>
        public static readonly short? 装载 = 600;
        /// <summary>
        /// 过账 = 700
        /// </summary>
        public static readonly short? 过账 = 700;
        /// <summary>
        /// 发运 = 800
        /// </summary>
        public static readonly short? 发运 = 800;
        /// <summary>
        /// 回传 = 900
        /// </summary>
        public static readonly short? 回传 = 900;
    }

    public static class AGVTaskState
    {
        /// <summary>
        /// 异常 = -99
        /// </summary>
        public static readonly int? 异常 = -100;
        /// <summary>
        /// 充电 = -1
        /// </summary>
        public static readonly int? 充电 = -1;
        /// <summary>
        /// 空闲 = 0
        /// </summary>
        public static readonly int? 空闲 = 0;
        /// <summary>
        /// 任务生成=1
        /// </summary>
        public static readonly int? 任务生成=1;
        /// <summary>
        /// 任务下发=5
        /// </summary>
        public static readonly int? 任务下发 = 5;
        /// <summary>
        /// 小车响应=10
        /// </summary>
        public static readonly int? 小车响应 = 10;
        /// <summary>
        /// 回收料框开始=20
        /// </summary>
        public static readonly int? 回收料框开始 = 20;
        /// <summary>
        /// 回收料框到位=30
        /// </summary>
        public static readonly int? 回收料框到位 = 30;
        /// <summary>
        /// 回收料框完成=40
        /// </summary>
        public static readonly int? 回收料框完成 = 40;
        /// <summary>
        /// 配送开始=50
        /// </summary>
        public static readonly int? 配送开始=50;
        /// <summary>
        /// 配送装料完成=60
        /// </summary>
        public static readonly int? 配送装料完成 = 60;
        /// <summary>
        /// 配送到达检测点=70
        /// </summary>
        public static readonly int? 配送到达检测点 = 70;
        /// <summary>
        /// 配送检测失败=75
        /// </summary>
        public static readonly int? 配送检测失败 = 75;
        /// <summary>
        /// 配送检测通过=80
        /// </summary>
        public static readonly int? 配送检测通过 = 80;
        /// <summary>
        /// 配送到位=85
        /// </summary>
        public static readonly int? 配送到位 = 85;
        /// <summary>
        /// 配送完成=90
        /// </summary>
        public static readonly int? 配送完成 = 90;
        /// <summary>
        /// 配送投入使用=95
        /// </summary>
        public static readonly int? 配送投入使用 = 95;
        /// <summary>
        /// 配送使用完毕 = 100
        /// </summary>
        public static readonly int? 配送使用完毕 = 100;
        /// <summary>
        /// 取工件开始 = 200
        /// </summary>
        public static readonly int? 取工件开始 = 200;
        /// <summary>
        /// 取工件到达 = 210
        /// </summary>
        public static readonly int? 取工件到达 = 210;
        /// <summary>
        /// 取工件完成 = 220
        /// </summary>
        public static readonly int? 取工件完成 = 220;
        /// <summary>
        /// 放料车开始 = 250
        /// </summary>
        public static readonly int? 放料车开始 = 250;
        /// <summary>
        /// 放料车到达 = 260
        /// </summary>
        public static readonly int? 放料车到达 = 260;
        /// <summary>
        /// 放料车完成 = 270
        /// </summary>
        public static readonly int? 放料车完成 = 270;
        /// <summary>
        /// 任务完成 = 300
        /// </summary>
        public static readonly int? 任务完成 = 300;
    }
    public static class TaskStatus
    {
        /// <summary>
        /// 错误 = -99
        /// </summary>
        public static readonly int? 错误 = -99;
        /// <summary>
        /// 无任务 = -1
        /// </summary>
        public static readonly int? 无任务 = -1;
        /// <summary>
        /// 新建任务 = 0
        /// </summary>
        public static readonly int? 新建任务 = 0;
        /// <summary>
        /// 待下发任务 = 5
        /// </summary>
        public static readonly int? 待下发任务 = 5;
        /// <summary>
        /// 下达任务 = 10
        /// </summary>
        public static readonly int? 下达任务 = 10;
        /// <summary>
        /// 开始执行 = 20
        /// </summary>
        public static readonly int? 开始执行 = 20;
        /// <summary>
        /// 到达堆垛机接出站台(堆垛机执行出库完成) = 25
        /// </summary>
        public static readonly int? 到达堆垛机接出站台 = 25;
        /// <summary>
        /// 已经完成堆垛机接出站台任务响应 = 28
        /// </summary>
        public static readonly int? 已经完成堆垛机接出站台任务响应 = 28;
        /// <summary>
        /// 已经到站台 = 30
        /// </summary>
        public static readonly int? 已经到站台 = 30;
        /// <summary>
        /// 放货中（放不下待放其它容器） = 31
        /// </summary>
        public static readonly int? 放货中 = 31;
        /// <summary>
        /// 到达站台出库查看确认完成 = 32
        /// </summary>
        public static readonly int? 出库查看完成 = 32;
        /// <summary>
        /// 放/取货完成 = 33
        /// </summary>
        public static readonly int? 放取货完成 = 33;
        /// <summary>
        /// 模拟电气拣放货完成按钮 = 34
        /// </summary>
        public static readonly int? 模拟电气拣放货完成按钮 = 34;
        /// <summary>
        /// 到达堆垛机接入站台（表示堆垛机可以对其进行入库） = 35
        /// </summary>
        public static readonly int? 到达堆垛机接入站台 = 35;
        /// <summary>
        /// 堆垛机进行了入库响应 = 38
        /// </summary>
        public static readonly int? 堆垛机进行了入库响应 = 38;
        /// <summary>
        /// 已经完成 = 40
        /// </summary>
        public static readonly int? 已经完成 = 40;
    }

    public static class TransactionType
    {
        /// <summary>
        /// 入库 = receipt
        /// </summary>
        public static readonly string 入库 = "receipt";
        /// <summary>
        /// 出库 = shipment
        /// </summary>
        public static readonly string 出库 = "shipment";
    }

    public static class StationType
    {
        /// <summary>
        /// 单通道 = 1
        /// </summary>
        public static readonly int 单通道 = 1;
        /// <summary>
        /// 多通道 = 2
        /// </summary>
        public static readonly int 多通道 = 2;
    }
}