using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using log4net;

namespace CbcXmlGenerator.Models
{
    public class GBGenerator : IGenerator
    {
        ILog log = log4net.LogManager.GetLogger(typeof(GBGenerator));

        public string Year { get; set; }
        public Commons.GenerateMode Mode { get; set; }

        

        public GBGenerator(string year,Commons.GenerateMode mode)
        {
            Year = year;
            Mode = mode;
        }

        public string Generate(string path)
        {
            string timenow = DateTime.Now.ToString("yyyyMMdd'T'HH-mm-ss");
            FileInfo excelInfo = new FileInfo(path);
            DirectoryInfo dirInfo = excelInfo.Directory;
            string outputPath;

            CbcOECDGB oecdgb = new CbcOECDGB();

            #region 讀取 Excel 產生 Xml
            IWorkbook wb = null;
            ISheet ws = null;
            IRow row = null;
            ICell cell = null;
            int rowIndex = 1;
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
                string _CBCID = "XBCBC1000000023";

                #region 設定 MessageSpec

                oecdgb.MessageSpec.SendingEntityIN = _CBCID;
                oecdgb.MessageSpec.TransmittingCountry = "GB";
                oecdgb.MessageSpec.ReceivingCountry = "GB";
                oecdgb.MessageSpec.MessageType = "CBC";
                oecdgb.MessageSpec.Language = "EN";

                oecdgb.MessageSpec.MessageTypeIndic = Mode == Commons.GenerateMode.Create
                    ? "CBC401" : "CBC402";
                oecdgb.MessageSpec.ReportingPeriod = $"{Year}-12-31";
                oecdgb.MessageSpec.Timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");

                #endregion

                #region 設定 CbcBody/ReportingEntity
                oecdgb.CbcBody.reportingEntity.entity.resCountryCode.value = "GB";
                oecdgb.CbcBody.reportingEntity.entity.tin.issuedBy = "GB";
                oecdgb.CbcBody.reportingEntity.entity.tin.tin = "2367000221";
                oecdgb.CbcBody.reportingEntity.entity.name.value = "Shanghai Commercial Bank Limited";
                oecdgb.CbcBody.reportingEntity.entity.address.legalAddressType = "OECD303";
                oecdgb.CbcBody.reportingEntity.entity.address.countryCode.value = "GB";
                oecdgb.CbcBody.reportingEntity.entity.address.addressFix.street.value = "65 Cornhill";
                oecdgb.CbcBody.reportingEntity.entity.address.addressFix.city.value = "London";
                oecdgb.CbcBody.reportingEntity.reportingRole.value = "CBC701";
                if (Mode == Commons.GenerateMode.Create)
                    oecdgb.CbcBody.reportingEntity.docSpec.docTypeIndic.value = "OECD1";
                else
                    oecdgb.CbcBody.reportingEntity.docSpec.docTypeIndic.value = "OECD2";
                oecdgb.CbcBody.reportingEntity.docSpec.docRefId.value = oecdgb.MessageSpec.MessageRefId +
                    "_03036306" + oecdgb.CbcBody.reportingEntity.docSpec.docTypeIndic.value + "ENTTW";
                #endregion

                // 先處理 Summary
                #region 設定Summary
                ws = wb.GetSheetAt(0);
                row = ws.GetRow(rowIndex);
                while(row != null)
                {
                    // Country Code
                    cell = row.GetCell(0);
                    string countrycode = cell.StringCellValue;
                    var temp = new CbcBodyGB.CbcReports();
                    if (Mode == Commons.GenerateMode.Create)
                        temp.docSpec.docTypeIndic.value = "OECD1";
                    else
                        temp.docSpec.docTypeIndic.value = "OECD2";
                    temp.docSpec.docRefId.value = $"{oecdgb.MessageSpec.MessageRefId}_03036306{temp.docSpec.docTypeIndic.value}REP{countrycode}";
                    temp.resCountryCode.value = countrycode;

                    // Unrelated
                    cell = row.GetCell(1);
                    if(cell != null)
                    {
                        temp.summary.revenues.unrelated.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.revenues.unrelated.value = "0";
                    }

                    // Related
                    cell = row.GetCell(2);
                    if (cell != null)
                    {
                        temp.summary.revenues.related.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.revenues.related.value = "0";
                    }

                    // Total
                    cell = row.GetCell(3);
                    if(cell != null)
                    {
                        temp.summary.revenues.total.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.revenues.total.value = "0";
                    }

                    // ProfitOrLoss
                    cell = row.GetCell(4);
                    if(cell != null)
                    {
                        temp.summary.profitOrLoss.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.profitOrLoss.value = "0";
                    }

                    // TaxPaid
                    cell = row.GetCell(5);
                    if(cell != null)
                    {
                        temp.summary.taxPaid.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.taxPaid.value = "0";
                    }

                    // TaxAccrued
                    cell = row.GetCell(6);
                    if(cell != null)
                    {
                        temp.summary.taxAccrued.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.taxAccrued.value = "0";
                    }

                    // Capital
                    cell = row.GetCell(7);
                    if(cell != null)
                    {
                        temp.summary.capital.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.capital.value = "0";
                    }

                    // Earnings
                    cell = row.GetCell(8);
                    if(cell != null)
                    {
                        temp.summary.earnings.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.earnings.value = "0";
                    }

                    // NbEmployee
                    cell = row.GetCell(9);
                    if(cell != null)
                    {
                        temp.summary.nbEmployees.value = GetCellStringValue(cell);                        
                    }
                    else
                    {
                        temp.summary.nbEmployees.value = "0";
                    }

                    // Assets
                    cell = row.GetCell(10);
                    if(cell != null)
                    {
                        cell.SetCellType(CellType.String);
                        temp.summary.assets.value = cell.StringCellValue;
                    }
                    else
                    {
                        temp.summary.assets.value = "0";
                    }

                    oecdgb.CbcBody.cbcReports.Add(temp);
                    rowIndex++;
                    row = ws.GetRow(rowIndex);
                }
                #endregion

                #region 加入 ConstEntities
                for(int i = 1;i < numofSheet;i++)
                {
                    string countryCode;
                    rowIndex = 1;
                    ws = wb.GetSheetAt(i);
                    countryCode = ws.SheetName;

                    log.DebugFormat("Read Sheet {0},Sheet Name : {1}", i, countryCode);

                    var cbcReports = oecdgb.CbcBody.cbcReports.Where(c => c.resCountryCode.value.Equals(countryCode)).FirstOrDefault();
                    if(cbcReports != null)
                    {
                        row = ws.GetRow(rowIndex);
                        while(row != null)
                        {
                            var tempEntity = new CbcBodyGB.ReportingEntity.Entity();
                            var tempEntities = new CbcBodyGB.CbcReports.ConstEntities();

                            // ResCountryCode
                            tempEntity.resCountryCode.value = countryCode;

                            // TIN CountryCode
                            cell = row.GetCell(0);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.tin.issuedBy = cell.StringCellValue;
                            }

                            // TIN
                            cell = row.GetCell(1);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.tin.tin = cell.StringCellValue;
                            }
                            else
                            {
                                tempEntity.tin.tin = "NOTIN";
                            }

                            // IN CountryCode && IN
                            cell = row.GetCell(2);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                var tempIN = new CbcBodyGB.ReportingEntity.Entity.IN();
                                tempIN.issuedBy = cell.StringCellValue;
                                cell = row.GetCell(3);
                                cell.SetCellType(CellType.String);
                                if(cell != null)
                                {
                                    tempIN.value = cell.StringCellValue;
                                }
                                tempEntity._in.Add(tempIN);
                            }

                            // Name
                            cell = row.GetCell(4);
                            cell.SetCellType(CellType.String);
                            tempEntity.name.value = cell.StringCellValue;

                            // Street
                            cell = row.GetCell(5);
                            tempEntity.address.legalAddressType = "OECD303";
                            tempEntity.address.countryCode.value = countryCode;
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.street.value = cell.StringCellValue;
                            }

                            // Building
                            cell = row.GetCell(6);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.buildingIdentifier.value = cell.StringCellValue;
                            }

                            // Suite
                            cell = row.GetCell(7);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.suiteIdentifier.value = cell.StringCellValue;
                            }

                            // Floor
                            cell = row.GetCell(8);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.floorIdentifier.value = cell.StringCellValue;
                            }

                            // DistrictName
                            cell = row.GetCell(9);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.districtName.value = cell.StringCellValue;
                            }

                            // POB
                            cell = row.GetCell(10);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.pOB.value = cell.StringCellValue;
                            }

                            // PostCode
                            cell = row.GetCell(11);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.postCode.value = cell.StringCellValue;
                            }

                            // City
                            cell = row.GetCell(12);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.city.value = cell.StringCellValue;
                            }

                            // CountrySubEntity
                            cell = row.GetCell(13);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntity.address.addressFix.countrySubentity.value = cell.StringCellValue;
                            }

                            // BizActivities
                            cell = row.GetCell(14);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntities.bizActivities.value = cell.StringCellValue;
                            }

                            // OtherEntityInfo
                            cell = row.GetCell(15);
                            if(cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                tempEntities.otherEntityInfo.value = cell.StringCellValue;
                            }

                            // IncorpCountryCode
                            tempEntities.incorpCountryCode.value = countryCode;
                            tempEntities.constEntity = tempEntity;
                            cbcReports.constEntities.Add(tempEntities);
                            rowIndex++;
                            row = ws.GetRow(rowIndex);
                        }
                    }
                }
                #endregion

                wb.Close();
                fs.Close();
            }
            #endregion

            serializer = new XmlSerializer(typeof(CbcOECDGB));
            ns = new XmlSerializerNamespaces();
            ns.Add("cbc", "urn:oecd:ties:cbc:v1");
            ns.Add("stf", "urn:oecd:ties:stf:v4");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            outputPath = Path.Combine(dirInfo.FullName, $"{oecdgb.MessageSpec.MessageRefId}.xml");
            try
            {
                using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, oecdgb, ns);
                }
                log.DebugFormat("Xml {0} 序列化完成，新增至資料庫...", outputPath);                
                FileDownload file = new FileDownload();
                file.Id = timenow;
                file.FileName = $"{oecdgb.MessageSpec.MessageRefId}.xml";
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
            if (cell != null)
            {
                cell.SetCellType(CellType.String);
                return cell.StringCellValue;
            }
            return string.Empty;
        }

    }
}