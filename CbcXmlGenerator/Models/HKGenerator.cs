using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using log4net;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CbcXmlGenerator.Models
{
    public class HKGenerator : IGenerator
    {
        ILog log = log4net.LogManager.GetLogger(typeof(HKGenerator));

        public string Year { get; set; }
        public Commons.GenerateMode Mode { get; set; } 

        public HKGenerator(string year,Commons.GenerateMode mode)
        {
            Year = year;
            Mode = mode;
        }

        public string Generate(string path)
        {
            DateTime now = DateTime.Now;
            string timenow = now.ToString("yyyyMMddHHmmss");
            FileInfo excelInfo = new FileInfo(path);
            DirectoryInfo dirInfo = excelInfo.Directory;
            string outputPath = $"{Year}HK10001{timenow}01.xml";

            CbcOECDHK oecdhk = new CbcOECDHK();

            #region 設定 MessageSpec
            oecdhk.MessageSpec.CbcId = "CH67289";
            oecdhk.MessageSpec.ReportingEntityName = "Shanghai Commercial Bank Limited";
            oecdhk.MessageSpec.Language = "EN";
            oecdhk.MessageSpec.MessageRefId = outputPath.Replace(".xml","");
            oecdhk.MessageSpec.MessageTypeIndic = Mode == Commons.GenerateMode.Create
                ? "CBC401" : "CBC402";
            oecdhk.MessageSpec.ReportingPeriod = $"{Year}-12-31";
            oecdhk.MessageSpec.Timestamp = now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            #endregion

            #region 設定 CbcBody

            IWorkbook wb = null;
            ISheet ws = null;
            ISheet ws_Summary = null;
            IRow row = null;
            IRow row_Summary = null;
            ICell cell = null;
            ICell cell_Summary = null;
            int rowIndex = 1;
            int rowidx_Summary = 1;
            int numofSheet = 0;

            XmlSerializer serializer = null;
            XmlSerializerNamespaces ns = null;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (path.Contains(".xlsx"))
                    wb = new XSSFWorkbook(fs);
                else
                    wb = new HSSFWorkbook(fs);
                numofSheet = wb.NumberOfSheets;

                log.DebugFormat("Excel Opend : {0}", path);
                ws_Summary = wb.GetSheetAt(0);
                for(int i = 1;i < numofSheet;i++)
                {
                    ws = wb.GetSheetAt(i);                    
                    CbcBodyHK.CbcReports reports = new CbcBodyHK.CbcReports();
                    string refidFront = outputPath.Replace(".xml", "");
                    string refidEnd = "_OECD{0}{1}{2}";
                    switch(Mode)
                    {
                        case Commons.GenerateMode.Create:
                            reports.docSpec.docTypeIndic.value = "OECD1";
                            if (ws.SheetName.Equals("TW"))
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 1, "ENT", ws.SheetName);
                            else
                                reports.docSpec.docRefId.value = refidEnd + string.Format(refidEnd, 1, "REP", ws.SheetName);
                            break;
                        case Commons.GenerateMode.Modify:
                            reports.docSpec.docTypeIndic.value = "OECD2";
                            reports.docSpec.InitializeWhenModify();
                            reports.docSpec.corrFileSerialNumber.value = "00000000";              
                            if (ws.SheetName.Equals("TW"))
                            {
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 2, "ENT", ws.SheetName);
                                reports.docSpec.corrDocRefId.value = refidFront + string.Format(refidEnd, 1, "ENT", ws.SheetName);
                            }                                
                            else
                            {
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 2, "REP", ws.SheetName);
                                reports.docSpec.corrDocRefId.value = refidFront + string.Format(refidEnd, 1, "REP", ws.SheetName);
                            }                                
                            break;
                        case Commons.GenerateMode.CreateTest:
                            reports.docSpec.docTypeIndic.value = "OECD11";                                                        
                            if (ws.SheetName.Equals("TW"))
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 11, "ENT", ws.SheetName);
                            else
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 11, "REP", ws.SheetName);
                            break;
                        case Commons.GenerateMode.ModifyTest:
                            reports.docSpec.docTypeIndic.value = "OECD12";
                            reports.docSpec.InitializeWhenModify();
                            reports.docSpec.corrFileSerialNumber.value = "00000000";
                            if (ws.SheetName.Equals("TW"))
                            {
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 12, "ENT", ws.SheetName);
                                reports.docSpec.corrDocRefId.value = refidFront + string.Format(refidEnd, 11, "ENT", ws.SheetName);
                            }                                
                            else
                            {
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 12, "REP", ws.SheetName);
                                reports.docSpec.docRefId.value = refidFront + string.Format(refidEnd, 11, "REP", ws.SheetName);
                            }                                
                            break;
                    }
                    reports.resCountryCode.value = ws.SheetName;
                    row_Summary = ws_Summary.GetRow(rowidx_Summary);
                    cell_Summary = row_Summary.GetCell(0);
                    while(cell_Summary != null && !string.IsNullOrEmpty(cell_Summary.StringCellValue) && !cell_Summary.StringCellValue.Equals(ws.SheetName))
                    {
                        rowidx_Summary++;
                        row_Summary = ws_Summary.GetRow(rowidx_Summary);
                        cell_Summary = row_Summary.GetCell(0);
                    }
                    if(cell_Summary == null || !cell_Summary.StringCellValue.Equals(ws.SheetName))
                    {
                        string msg = $"Cannot Find Nation [{ws.SheetName}] in the Summary Page";
                        log.Error(msg);
                        throw new Exception(msg);
                    }

                    // 先寫 Summary
                    cell_Summary = row_Summary.GetCell(1);
                    reports.summary.revenues.unrelated.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(2);
                    reports.summary.revenues.related.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(3);
                    reports.summary.revenues.total.value = GetCellStringValue(cell_Summary);
                    
                    cell_Summary = row_Summary.GetCell(4);
                    reports.summary.profitOrLoss.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(5);
                    reports.summary.taxPaid.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(6);
                    reports.summary.taxAccrued.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(7);
                    reports.summary.capital.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(8);
                    reports.summary.earnings.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(9);
                    reports.summary.nbEmployees.value = GetCellStringValue(cell_Summary);                    

                    cell_Summary = row_Summary.GetCell(10);
                    reports.summary.assets.value = GetCellStringValue(cell_Summary);                    

                    rowidx_Summary = 1;

                    // 這裡開始是各國別分頁資訊
                    rowIndex = 1;
                    
                    while ((row = ws.GetRow(rowIndex)) != null)
                    {
                        CbcBodyHK.CbcReports.ConstEntities entity = new CbcBodyHK.CbcReports.ConstEntities();
                        CbcBodyHK.CbcReports.ConstEntities.ConstEntity constEntity = new CbcBodyHK.CbcReports.ConstEntities.ConstEntity();
                        constEntity.resCountryCode.value = ws.SheetName;
                        cell = row.GetCell(1);
                        constEntity.tIN.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(3);
                        constEntity.iN.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(4);
                        constEntity.name.value = GetCellStringValue(cell);                        

                        constEntity.address.legalAddressType = "OECD303";
                        constEntity.address.country.value = ws.SheetName;
                        cell = row.GetCell(5);
                        constEntity.address.addressFix.street.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(6);
                        constEntity.address.addressFix.buildingIdentifier.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(7);
                        constEntity.address.addressFix.suiteIdentifier.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(8);
                        constEntity.address.addressFix.floorIdentifier.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(9);
                        constEntity.address.addressFix.districtName.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(10);
                        constEntity.address.addressFix.pOB.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(11);
                        constEntity.address.addressFix.postCode.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(12);                        
                        constEntity.address.addressFix.city.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(13);                        
                        constEntity.address.addressFix.countrySubentity.value = GetCellStringValue(cell);                        
                        entity.constEntity = constEntity;

                        cell = row.GetCell(14);
                        entity.bizActivities.value = GetCellStringValue(cell);                        

                        cell = row.GetCell(15);
                        entity.otherEntityInfo.value = GetCellStringValue(cell);
                        
                        reports.constEntities.Add(entity);
                        rowIndex++;
                    }

                    oecdhk.CbcBody.cbcReports.Add(reports);
                    rowIndex = 1;
                }
                wb.Close();
                fs.Close();
            }
            #endregion

            serializer = new XmlSerializer(typeof(CbcOECDHK));
            ns = new XmlSerializerNamespaces();
            ns.Add("cbc", "http://www.ird.gov.hk/AEOI/cbc/v1");
            ns.Add("stf", "http://www.ird.gov.hk/AEOI/cbctypes/v1");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            outputPath = Path.Combine(dirInfo.FullName, outputPath);
            try
            {
                using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, oecdhk, ns);
                }
                log.DebugFormat("Xml {0} 序列化完成，新增至資料庫...", outputPath);
                FileInfo outFileInfo = new FileInfo(outputPath);
                FileDownload file = new FileDownload();
                file.Id = timenow;
                file.FileName = $"{oecdhk.MessageSpec.MessageRefId}.xml";
                file.FileStream = File.ReadAllBytes(outputPath);
                file.CreateDate = DateTime.Now;
                file = file.Insert(file);
                // 要把檔案刪掉                                
                dirInfo.Delete(true);
                log.DebugFormat("Delete Dir {0}", dirInfo.FullName);
            }
            catch(Exception e)
            {
                log.DebugFormat("ErrorType : {0},Message : {1},StackTrace : {2}", e.GetType(), e.Message, e.StackTrace);
            }
            return JsonConvert.SerializeObject(new { Id = timenow });
        }

        private string GetCellStringValue(ICell cell)
        {
            if(cell != null)
            {
                cell.SetCellType(CellType.String);
                return cell.StringCellValue;
            }
            return string.Empty;
        }
    }
}