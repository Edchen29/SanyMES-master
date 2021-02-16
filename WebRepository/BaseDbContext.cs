using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace WebRepository
{
    public partial class BaseDBContext: DbContext
    {
        //public BaseDBContext(DbContextOptions<BaseDBContext> options)
        //    : base(options)
        //{ }
        public BaseDBContext(DbContextOptions<BaseDBContext> options, IConfiguration configuration) : base(options)
        {
            string ConnString = configuration.GetConnectionString("BaseDBContext");
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ConnString);
            //string enpwd = Encryption.Encrypt("1qaz!QAZ");
            sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);
            Database.GetDbConnection().ConnectionString = sqlConnectionStringBuilder.ConnectionString;
        }
        public virtual DbSet<AgvMonitor> AgvMonitors { get; set; }
        public virtual DbSet<Container> Containers { get; set; }
        public virtual DbSet<DefectCode> DefectCodes { get; set; }
        public virtual DbSet<EbomDetail> EbomDetails { get; set; }
        public virtual DbSet<EbomHeader> EbomHeaders { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentMonitor> EquipmentMonitors { get; set; }
        public virtual DbSet<EquipmentAlarmLog> EquipmentAlarmLogs { get; set; }
        public virtual DbSet<EquipmentAlarmText> EquipmentAlarmTexts { get; set; }
        public virtual DbSet<EquipmentItem> EquipmentItems { get; set; }
        public virtual DbSet<EquipmentMaintainLog> EquipmentMaintainLogs { get; set; }
        public virtual DbSet<EquipmentMaintainRule> EquipmentMaintainRules { get; set; }
        public virtual DbSet<EquipmentProp> EquipmentProps { get; set; }
        public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
        public virtual DbSet<EquipmentStatusLog> EquipmentStatusLogs { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<EquipmentTypeTemplate> EquipmentTypeTemplates { get; set; }
        public virtual DbSet<Excelreport> Excelreports { get; set; }
        public virtual DbSet<Factory> Factories { get; set; }
        public virtual DbSet<FlowScheme> FlowSchemes { get; set; }
        public virtual DbSet<InterfaceMbomDetail> InterfaceMbomDetails { get; set; }
        public virtual DbSet<InterfaceMbomHeader> InterfaceMbomHeaders { get; set; }
        public virtual DbSet<InterfaceOrderDetiail> InterfaceOrderDetiails { get; set; }
        public virtual DbSet<InterfaceOrderHeader> InterfaceOrderHeaders { get; set; }
        public virtual DbSet<InterfaceProductDetail> InterfaceProductDetails { get; set; }
        public virtual DbSet<InterfaceProductHeader> InterfaceProductHeaders { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryAlert> InventoryAlerts { get; set; }
        public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialCallHeader> MaterialCallHeaders { get; set; }
        public virtual DbSet<MaterialCallDetail> MaterialCallDetails { get; set; }
        public virtual DbSet<MaterialDemand> MaterialDemands { get; set; }
        public virtual DbSet<MaterialDistributeTaskDetail> MaterialDistributeTaskDetails { get; set; }
        public virtual DbSet<MaterialDistributeTaskHeader> MaterialDistributeTaskHeaders { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<MaterialUnit> MaterialUnits { get; set; }
        public virtual DbSet<MaterialUnitMultiple> MaterialUnitMultiples { get; set; }
        public virtual DbSet<MbomDetail> MbomDetails { get; set; }
        public virtual DbSet<MbomHeader> MbomHeaders { get; set; }
        public virtual DbSet<OrderAlert> OrderAlerts { get; set; }
        public virtual DbSet<OrderDetiail> OrderDetiails { get; set; }
        public virtual DbSet<OrderDetiailHistory> OrderDetiailHistories { get; set; }
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }
        public virtual DbSet<OrderHeaderHistory> OrderHeaderHistories { get; set; }
        public virtual DbSet<PbomDetail> PbomDetails { get; set; }
        public virtual DbSet<PbomHeader> PbomHeaders { get; set; }
        public virtual DbSet<ProductBarcodeRule> ProductBarcodeRules { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductHeader> ProductHeaders { get; set; }
        public virtual DbSet<Repair> Repairs { get; set; }
        public virtual DbSet<RepairCode> RepairCodes { get; set; }
        public virtual DbSet<SerialNumber> SerialNumbers { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<StepStation> StepStations { get; set; }
        public virtual DbSet<StepTrace> StepTraces { get; set; }
        public virtual DbSet<StepTraceHistory> StepTraceHistorys { get; set; }
        public virtual DbSet<StepTraceLog> StepTraceLogs { get; set; }
        public virtual DbSet<SysCount> SysCounts { get; set; }
        public virtual DbSet<SysDept> SysDepts { get; set; }
        public virtual DbSet<SysDictData> SysDictDatas { get; set; }
        public virtual DbSet<SysDictType> SysDictTypes { get; set; }
        public virtual DbSet<SysInfo> SysInfos { get; set; }
        public virtual DbSet<SysInterfaceLog> SysInterfaceLogs { get; set; }
        public virtual DbSet<SysJob> SysJobs { get; set; }
        public virtual DbSet<SysJobLog> SysJobLogs { get; set; }
        public virtual DbSet<SysLoginLog> SysLoginLogs { get; set; }
        public virtual DbSet<SysModule> SysModules { get; set; }
        public virtual DbSet<SysModuleElement> SysModuleElements { get; set; }
        public virtual DbSet<SysOperLog> SysOperLogs { get; set; }
        public virtual DbSet<SysRelevance> SysRelevances { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserOnline> SysUserOnlines { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<Workshop> Workshops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            modelBuilder.Entity<AgvMonitor>().HasKey(c => c.Id);
            modelBuilder.Entity<AgvMonitor>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Container>().HasKey(c => c.Id);
            modelBuilder.Entity<Container>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<DefectCode>().HasKey(c => c.Id);
            modelBuilder.Entity<DefectCode>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EbomDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<EbomDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EbomHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<EbomHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Equipment>().HasKey(c => c.Id);
            modelBuilder.Entity<Equipment>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentMonitor>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentMonitor>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentAlarmLog>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentAlarmLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentAlarmText>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentAlarmText>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentItem>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentItem>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentMaintainLog>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentMaintainLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentMaintainRule>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentMaintainRule>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentProp>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentProp>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentStatus>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentStatus>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentStatusLog>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentStatusLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentType>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentType>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<EquipmentTypeTemplate>().HasKey(c => c.Id);
            modelBuilder.Entity<EquipmentTypeTemplate>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Excelreport>().HasKey(c => c.Id);
            modelBuilder.Entity<Excelreport>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Factory>().HasKey(c => c.Id);
            modelBuilder.Entity<Factory>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<FlowScheme>().HasKey(c => c.Id);
            modelBuilder.Entity<FlowScheme>().Property(c => c.Id).HasColumnName("Id");

            modelBuilder.Entity<InterfaceMbomDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceMbomDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InterfaceMbomHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceMbomHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InterfaceOrderDetiail>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceOrderDetiail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InterfaceOrderHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceOrderHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InterfaceProductDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceProductDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InterfaceProductHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<InterfaceProductHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Inventory>().HasKey(c => c.Id);
            modelBuilder.Entity<Inventory>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InventoryAlert>().HasKey(c => c.Id);
            modelBuilder.Entity<InventoryAlert>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<InventoryTransaction>().HasKey(c => c.Id);
            modelBuilder.Entity<InventoryTransaction>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Line>().HasKey(c => c.Id);
            modelBuilder.Entity<Line>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Location>().HasKey(c => c.Id);
            modelBuilder.Entity<Location>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Material>().HasKey(c => c.Id);
            modelBuilder.Entity<Material>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialCallHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialCallHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialCallDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialCallDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialDemand>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialDemand>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialDistributeTaskDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialDistributeTaskDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialDistributeTaskHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialDistributeTaskHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialType>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialType>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialUnit>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialUnit>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MaterialUnitMultiple>().HasKey(c => c.Id);
            modelBuilder.Entity<MaterialUnitMultiple>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MbomDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<MbomDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<MbomHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<MbomHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<OrderAlert>().HasKey(c => c.Id);
            modelBuilder.Entity<OrderAlert>().Property(c => c.Id).HasColumnName("Id");

            modelBuilder.Entity<OrderDetiail>().HasKey(c => c.Id);
            modelBuilder.Entity<OrderDetiail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<OrderDetiailHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<OrderDetiailHistory>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<OrderHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<OrderHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<OrderHeaderHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<OrderHeaderHistory>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<PbomDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<PbomDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<PbomHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<PbomHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<ProductBarcodeRule>().HasKey(c => c.Id);
            modelBuilder.Entity<ProductBarcodeRule>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<ProductDetail>().HasKey(c => c.Id);
            modelBuilder.Entity<ProductDetail>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<ProductHeader>().HasKey(c => c.Id);
            modelBuilder.Entity<ProductHeader>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Repair>().HasKey(c => c.Id);
            modelBuilder.Entity<Repair>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<RepairCode>().HasKey(c => c.Id);
            modelBuilder.Entity<RepairCode>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SerialNumber>().HasKey(c => c.Id);
            modelBuilder.Entity<SerialNumber>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Station>().HasKey(c => c.Id);
            modelBuilder.Entity<Station>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Step>().HasKey(c => c.Id);
            modelBuilder.Entity<Step>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<StepStation>().HasKey(c => c.Id);
            modelBuilder.Entity<StepStation>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<StepTrace>().HasKey(c => c.Id);
            modelBuilder.Entity<StepTrace>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<StepTraceHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<StepTraceHistory>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<StepTraceLog>().HasKey(c => c.Id);
            modelBuilder.Entity<StepTraceLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysCount>().HasKey(c => c.Type);
            modelBuilder.Entity<SysCount>().Property(c => c.Type).HasColumnName("Type");

            modelBuilder.Entity<SysDept>().HasKey(c => c.Id);
            modelBuilder.Entity<SysDept>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysDictData>().HasKey(c => c.Id);
            modelBuilder.Entity<SysDictData>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysDictType>().HasKey(c => c.Id);
            modelBuilder.Entity<SysDictType>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysInfo>().HasKey(c => c.Id);
            modelBuilder.Entity<SysInfo>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysInterfaceLog>().HasKey(c => c.Id);
            modelBuilder.Entity<SysInterfaceLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysJob>().HasKey(c => c.Id);
            modelBuilder.Entity<SysJob>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysJobLog>().HasKey(c => c.Id);
            modelBuilder.Entity<SysJobLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysLoginLog>().HasKey(c => c.Id);
            modelBuilder.Entity<SysLoginLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysModule>().HasKey(c => c.Id);
            modelBuilder.Entity<SysModule>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysModuleElement>().HasKey(c => c.Id);
            modelBuilder.Entity<SysModuleElement>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysOperLog>().HasKey(c => c.Id);
            modelBuilder.Entity<SysOperLog>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysRelevance>().HasKey(c => c.Id);
            modelBuilder.Entity<SysRelevance>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysRole>().HasKey(c => c.Id);
            modelBuilder.Entity<SysRole>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysUser>().HasKey(c => c.Id);
            modelBuilder.Entity<SysUser>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<SysUserOnline>().HasKey(c => c.Id);
            modelBuilder.Entity<SysUserOnline>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Warehouse>().HasKey(c => c.Id);
            modelBuilder.Entity<Warehouse>().Property(c => c.Id).HasColumnName("id");

            modelBuilder.Entity<Workshop>().HasKey(c => c.Id);
            modelBuilder.Entity<Workshop>().Property(c => c.Id).HasColumnName("id");

        }
    }
}