using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CbcXmlGenerator.Models
{
    public class CbcBodyGB
    {
        [XmlElement(ElementName = "ReportingEntity")]
        public ReportingEntity reportingEntity { get; set; }

        [XmlElement(ElementName = "CbcReports")]
        public List<CbcReports> cbcReports { get; set; }

        [XmlElement(ElementName = "AdditionalInfo")]
        public List<AdditionalInfo> addtionalInfo { get; set; }

        public class ReportingEntity
        {
            [XmlElement(ElementName = "Entity")]
            public Entity entity { get; set; }

            [XmlElement(ElementName = "ReportingRole")]
            public ReportingRole reportingRole { get; set; }

            [XmlElement(ElementName = "DocSpec")]
            public DocSpec docSpec { get; set; }

            public class Entity
            {
                [XmlElement(ElementName = "ResCountryCode")]
                public CbcReports.ResCountryCode resCountryCode { get; set; }

                [XmlElement(ElementName = "TIN")]
                public TIN tin { get; set; }

                [XmlElement(ElementName = "IN")]
                public List<IN> _in { get; set; }
                public bool ShouldSerialize_in()
                {
                    return _in != null && _in.Count > 0;
                }

                [XmlElement(ElementName = "Name")]
                public Name name { get; set; }

                [XmlElement(ElementName = "Address")]
                public Address address { get; set; }

                public class TIN
                {
                    private string _tin;

                    [XmlAttribute]
                    public string issuedBy { get; set; }

                    [XmlText]
                    public string tin
                    {
                        get
                        {
                            if (string.IsNullOrEmpty(_tin) || _tin.Contains("NOTIN"))
                                return string.Empty;
                            else
                                return _tin;
                        }
                        set
                        {
                            _tin = value;
                        }
                    }
                }

                public class IN
                {
                    [XmlAttribute]
                    public string issuedBy { get; set; }

                    [XmlAttribute]
                    public string INType { get; set; }

                    [XmlText]
                    public string value { get; set; }
                }

                public class Name
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class Address
                {
                    [XmlAttribute]
                    public string legalAddressType { get; set; }

                    [XmlElement(ElementName = "CountryCode")]
                    public CountryCode countryCode { get; set; }

                    [XmlElement(ElementName = "AddressFix")]
                    public AddressFix addressFix { get; set; }

                    public string AddressFree { get; set; }

                    public class CountryCode
                    {
                        [XmlText]
                        public string value { get; set; }
                    }

                    public class AddressFix
                    {
                        [XmlElement(ElementName = "Street")]
                        public Street street { get; set; }
                        public bool ShouldSerializestreet()
                        {
                            return !string.IsNullOrEmpty(street.value);
                        }

                        [XmlElement(ElementName = "BuildingIdentifier")]
                        public BuildingIdentifier buildingIdentifier { get; set; }
                        public bool ShouldSerializebuildingIdentifier()
                        {
                            return !string.IsNullOrEmpty(buildingIdentifier.value);
                        }

                        [XmlElement(ElementName = "SuiteIdentifier")]
                        public SuiteIdentifier suiteIdentifier { get; set; }
                        public bool ShouldSerializesuiteIdentifier()
                        {
                            return !string.IsNullOrEmpty(suiteIdentifier.value);
                        }

                        [XmlElement(ElementName = "FloorIdentifier")]
                        public FloorIdentifier floorIdentifier { get; set; }
                        public bool ShouldSerializefloorIdentifier()
                        {
                            return !string.IsNullOrEmpty(floorIdentifier.value);
                        }

                        [XmlElement(ElementName = "DistrictName")]
                        public DistrictName districtName { get; set; }
                        public bool ShouldSerializedistrictName()
                        {
                            return !string.IsNullOrEmpty(districtName.value);
                        }

                        [XmlElement(ElementName = "POB")]
                        public POB pOB { get; set; }
                        public bool ShouldSerializepOB()
                        {
                            return !string.IsNullOrEmpty(pOB.value);
                        }

                        [XmlElement(ElementName = "PostCode")]
                        public PostCode postCode { get; set; }
                        public bool ShouldSerializepostCode()
                        {
                            return !string.IsNullOrEmpty(postCode.value);
                        }

                        [XmlElement(ElementName = "City")]
                        public City city { get; set; }
                        public bool ShouldSerializecity()
                        {
                            return !string.IsNullOrEmpty(city.value);
                        }

                        [XmlElement(ElementName = "CountrySubentity")]
                        public CountrySubentity countrySubentity { get; set; }
                        public bool ShouldSerializecountrySubentity()
                        {
                            return !string.IsNullOrEmpty(countrySubentity.value);
                        }

                        public class Street
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class BuildingIdentifier
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class SuiteIdentifier
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class FloorIdentifier
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class DistrictName
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class POB
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class PostCode
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class City
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class CountrySubentity
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public AddressFix()
                        {
                            street = new Street();
                            buildingIdentifier = new BuildingIdentifier();
                            suiteIdentifier = new SuiteIdentifier();
                            floorIdentifier = new FloorIdentifier();
                            districtName = new DistrictName();
                            pOB = new POB();
                            postCode = new PostCode();
                            city = new City();
                            countrySubentity = new CountrySubentity();
                        }
                    }

                    public Address()
                    {
                        countryCode = new CountryCode();
                        addressFix = new AddressFix();
                    }
                }

                public Entity()
                {
                    resCountryCode = new CbcReports.ResCountryCode();
                    tin = new TIN();
                    _in = new List<IN>();
                    name = new Name();
                    address = new Address();
                }
            }

            public class ReportingRole
            {
                [XmlText]
                public string value { get; set; }
            }

            [XmlType(Namespace = "urn:oecd:ties:stf:v4")]
            public class DocSpec
            {
                [XmlElement(ElementName = "DocTypeIndic")]
                public DocTypeIndic docTypeIndic { get; set; }

                [XmlElement(ElementName = "DocRefId")]
                public DocRefId docRefId { get; set; }

                public DocSpec()
                {
                    docTypeIndic = new DocTypeIndic();
                    docRefId = new DocRefId();
                }

                public class DocTypeIndic
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class DocRefId
                {
                    [XmlText]
                    public string value { get; set; }
                }
            }

            public ReportingEntity()
            {
                entity = new Entity();
                reportingRole = new ReportingRole();
                docSpec = new DocSpec();
            }
        }

        public class CbcReports
        {
            [XmlElement(ElementName = "DocSpec")]
            public ReportingEntity.DocSpec docSpec { get; set; }

            [XmlElement(ElementName = "ResCountryCode")]
            public ResCountryCode resCountryCode { get; set; }

            [XmlElement(ElementName = "Summary")]
            public Summary summary { get; set; }

            [XmlElement(ElementName = "ConstEntities")]
            public List<ConstEntities> constEntities { get; set; }

            public class ResCountryCode
            {
                [XmlText]
                public string value { get; set; }
            }

            public class Summary
            {
                [XmlElement(ElementName = "Revenues")]
                public Revenues revenues { get; set; }

                [XmlElement(ElementName = "ProfitOrLoss")]
                public ProfitOrLoss profitOrLoss { get; set; }

                [XmlElement(ElementName = "TaxPaid")]
                public TaxPaid taxPaid { get; set; }

                [XmlElement(ElementName = "TaxAccrued")]
                public TaxAccrued taxAccrued { get; set; }

                [XmlElement(ElementName = "Capital")]
                public Capital capital { get; set; }

                [XmlElement(ElementName = "Earnings")]
                public Earnings earnings { get; set; }
            
                [XmlElement(ElementName = "NbEmployees")]
                public NbEmployees nbEmployees { get; set; }

                [XmlElement(ElementName = "Assets")]
                public Assets assets { get; set; }

                public class Revenues
                {
                    [XmlElement(ElementName = "Unrelated")]
                    public Unrelated unrelated { get; set; }

                    [XmlElement(ElementName = "Related")]
                    public Related related { get; set; }

                    [XmlElement(ElementName = "Total")]
                    public Total total { get; set; }

                    public class Unrelated : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Unrelated() : base() { }
                    }

                    public class Related : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Related() : base() { }
                    }

                    public class Total : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Total() : base() { }
                    }

                    public Revenues()
                    {
                        unrelated = new Unrelated();
                        related = new Related();
                        total = new Total();
                    }
                }
                public class ProfitOrLoss : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public ProfitOrLoss() : base() { }
                }
                public class TaxPaid : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public TaxPaid() : base() { }
                }
                public class TaxAccrued : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value;

                    public TaxAccrued() : base() { }
                }
                public class Capital : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Capital() : base() { }
                }
                public class Earnings : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Earnings() : base() { }
                }
                public class NbEmployees
                {
                    [XmlText]
                    public string value { get; set; }
                }
                public class Assets : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Assets() : base() { }
                }

                public Summary()
                {
                    revenues = new Revenues();
                    profitOrLoss = new ProfitOrLoss();
                    taxPaid = new TaxPaid();
                    taxAccrued = new TaxAccrued();
                    capital = new Capital();
                    earnings = new Earnings();
                    nbEmployees = new NbEmployees();
                    assets = new Assets();
                }
            }

            public class ConstEntities
            {
                [XmlElement(ElementName = "ConstEntity")]
                public ReportingEntity.Entity constEntity { get; set; }

                [XmlElement(ElementName = "IncorpCountryCode")]
                public IncorpCountryCode incorpCountryCode { get; set; }

                [XmlElement(ElementName = "BizActivities")]
                public BizActivities bizActivities { get; set; }

                [XmlElement(ElementName = "OtherEntityInfo")]
                public OtherEntityInfo otherEntityInfo { get; set; }

                public ConstEntities()
                {
                    constEntity = new ReportingEntity.Entity();
                    incorpCountryCode = new IncorpCountryCode();
                    bizActivities = new BizActivities();
                    otherEntityInfo = new OtherEntityInfo();
                }

                public class IncorpCountryCode
                {
                    [XmlText]
                    public string value { get; set; }
                }
                public class OtherEntityInfo
                {
                    [XmlText]
                    public string value { get; set; }
                }
                public class BizActivities
                {
                    [XmlText]
                    public string value { get; set; }
                }
            }

            public CbcReports()
            {
                docSpec = new ReportingEntity.DocSpec();
                resCountryCode = new ResCountryCode();
                summary = new Summary();
                constEntities = new List<ConstEntities>();
            }
        }

        public class AdditionalInfo
        {
            public ReportingEntity.DocSpec DocSpec { get; set; }

            [XmlElement(ElementName = "OtherInfo")]
            public OtherInfo otherInfo { get; set; }

            public CbcBodyGB.CbcReports.ResCountryCode ResCountryCode { get; set; }

            [XmlElement(ElementName = "SummaryRef")]
            public SummaryRef summaryRef { get; set; }

            public AdditionalInfo()
            {
                DocSpec = new ReportingEntity.DocSpec();
            }

            public class OtherInfo
            {
                [XmlText]
                public string value { get; set; }
            }

            public class SummaryRef
            {
                [XmlText]
                public string value { get; set; }
            }
        }

        public CbcBodyGB()
        {
            reportingEntity = new ReportingEntity();
            cbcReports = new List<CbcReports>();
            addtionalInfo = new List<AdditionalInfo>();
        }
    }

    public class CbcBodyHK
    {
        [XmlElement(ElementName = "CbcReports")]
        public List<CbcReports> cbcReports { get; set; }

        public class CbcReports
        {
            [XmlElement(ElementName = "DocSpec")]
            public DocSpec docSpec { get; set; }

            [XmlElement(ElementName = "ResCountryCode")]
            public ResCountryCode resCountryCode { get; set; }

            [XmlElement(ElementName = "Summary")]
            public Summary summary { get; set; }

            [XmlElement(ElementName = "ConstEntities")]
            public List<ConstEntities> constEntities { get; set; }

            public CbcReports()
            {
                docSpec = new DocSpec();
                resCountryCode = new ResCountryCode();
                summary = new Summary();
                constEntities = new List<ConstEntities>();
            }

            public class DocSpec
            {
                [XmlElement(Namespace = "http://www.ird.gov.hk/AEOI/cbctypes/v1", ElementName = "DocTypeIndic")]
                public DocTypeIndic docTypeIndic { get; set; }

                [XmlElement(Namespace = "http://www.ird.gov.hk/AEOI/cbctypes/v1", ElementName = "DocRefId")]
                public DocRefId docRefId { get; set; }

                [XmlElement(Namespace = "http://www.ird.gov.hk/AEOI/cbctypes/v1", ElementName = "CorrFileSerialNumber")]
                public CorrFileSerialNumber corrFileSerialNumber { get; set; }
                public bool ShouldSerializecorrFileSerialNumber()
                {
                    return corrFileSerialNumber != null;
                }

                [XmlElement(Namespace = "http://www.ird.gov.hk/AEOI/cbctypes/v1", ElementName = "CorrDocRefId")]
                public CorrDocRefId corrDocRefId { get; set; }
                public bool ShouldSerializecorrDocRefId()
                {
                    return corrDocRefId != null;
                }

                public DocSpec()
                {
                    docTypeIndic = new DocTypeIndic();
                    docRefId = new DocRefId();
                }

                public void InitializeWhenModify()
                {
                    corrFileSerialNumber = new CorrFileSerialNumber();
                    corrDocRefId = new CorrDocRefId();
                }

                public class DocTypeIndic
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class DocRefId
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class CorrFileSerialNumber
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class CorrDocRefId
                {
                    [XmlText]
                    public string value { get; set; }
                }
            }

            public class ResCountryCode
            {
                [XmlText]
                public string value { get; set; }
            }

            public class Summary
            {
                [XmlElement(ElementName = "Revenues")]
                public Revenues revenues { get; set; }

                [XmlElement(ElementName = "ProfitOrLoss")]
                public ProfitOrLoss profitOrLoss { get; set; }

                [XmlElement(ElementName = "TaxPaid")]
                public TaxPaid taxPaid { get; set; }

                [XmlElement(ElementName = "TaxAccrued")]
                public TaxAccrued taxAccrued { get; set; }

                [XmlElement(ElementName = "Capital")]
                public Capital capital { get; set; }

                [XmlElement(ElementName = "Earnings")]
                public Earnings earnings { get; set; }

                [XmlElement(ElementName = "NbEmployees")]
                public NbEmployees nbEmployees { get; set; }

                [XmlElement(ElementName = "Assets")]
                public Assets assets { get; set; }

                public Summary()
                {
                    revenues = new Revenues();
                    profitOrLoss = new ProfitOrLoss();
                    taxPaid = new TaxPaid();
                    taxAccrued = new TaxAccrued();
                    capital = new Capital();
                    earnings = new Earnings();
                    nbEmployees = new NbEmployees();
                    assets = new Assets();
                }

                public class Revenues
                {
                    [XmlElement(ElementName = "Unrelated")]
                    public Unrelated unrelated { get; set; }

                    [XmlElement(ElementName = "Related")]
                    public Related related { get; set; }

                    [XmlElement(ElementName = "Total")]
                    public Total total { get; set; }

                    public Revenues()
                    {
                        unrelated = new Unrelated();
                        related = new Related();
                        total = new Total();
                    }

                    public class Unrelated : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Unrelated() : base() { }
                    }

                    public class Related : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Related() : base() { }
                    }

                    public class Total : Models.Commons.CurrCode
                    {
                        [XmlText]
                        public string value { get; set; }

                        public Total() : base() { }
                    }
                }

                public class ProfitOrLoss : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public ProfitOrLoss() : base() { }
                }

                public class TaxPaid : Models.Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public TaxPaid() : base() { }
                }

                public class TaxAccrued : Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public TaxAccrued() : base() { }
                }

                public class Capital : Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Capital() : base() { }
                }

                public class Earnings : Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Earnings() : base() { }
                }

                public class NbEmployees
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class Assets : Commons.CurrCode
                {
                    [XmlText]
                    public string value { get; set; }

                    public Assets() : base() { }
                }
            }

            public class ConstEntities
            {
                [XmlElement(ElementName = "ConstEntity")]
                public ConstEntity constEntity { get; set; }

                [XmlElement(ElementName = "BizActivities")]
                public BizActivities bizActivities { get; set; }

                [XmlElement(ElementName = "OtherEntityInfo")]
                public OtherEntityInfo otherEntityInfo { get; set; }

                public ConstEntities()
                {
                    constEntity = new ConstEntity();
                    bizActivities = new BizActivities();
                    otherEntityInfo = new OtherEntityInfo();
                }

                public class ConstEntity
                {
                    [XmlElement(ElementName = "ResCountryCode")]
                    public CbcBodyHK.CbcReports.ResCountryCode resCountryCode { get; set; }

                    [XmlElement(ElementName = "TIN")]
                    public TIN tIN { get; set; }

                    [XmlElement(ElementName = "IN")]
                    public IN iN { get; set; }
                    public bool ShouldSerializeiN()
                    {
                        return iN != null && !string.IsNullOrEmpty(iN.value);
                    }

                    [XmlElement(ElementName = "Name")]
                    public Name name { get; set; }

                    [XmlElement(ElementName = "Address")]
                    public Address address { get; set; }

                    public ConstEntity()
                    {
                        resCountryCode = new ResCountryCode();
                        tIN = new TIN();
                        iN = new IN();
                        name = new Name();
                        address = new Address();
                    }                    

                    public class TIN : Commons.IssuedBy
                    {
                        private string _val;

                        [XmlText]
                        public string val
                        {
                            get
                            {
                                if (string.IsNullOrEmpty(_val) || _val.Contains("NOTIN"))
                                    return string.Empty;
                                else
                                    return _val;
                            }
                            set
                            {
                                _val = value;
                            }
                        }

                        public TIN() : base() { }
                    }

                    public class IN : Commons.IssuedBy
                    {
                        [XmlText]
                        public string value { get; set; }

                        public IN() : base() { }
                    }

                    public class Name
                    {
                        [XmlText]
                        public string value { get; set; }
                    }

                    public class Address
                    {
                        [XmlAttribute]
                        public string legalAddressType { get; set; }

                        [XmlElement(ElementName = "CountryCode")]
                        public CountryCode country { get; set; }

                        [XmlElement(ElementName = "AddressFix")]
                        public AddressFix addressFix { get; set; }

                        public Address()
                        {
                            country = new CountryCode();
                            addressFix = new AddressFix();
                        }

                        public class CountryCode
                        {
                            [XmlText]
                            public string value { get; set; }
                        }

                        public class AddressFix
                        {
                            [XmlElement(ElementName = "Street")]
                            public Street street { get; set; }
                            public bool ShouldSerializestreet()
                            {
                                return !string.IsNullOrEmpty(street.value);
                            }

                            [XmlElement(ElementName = "BuildingIdentifier")]
                            public BuildingIdentifier buildingIdentifier { get; set; }
                            public bool ShouldSerializebuildingIdentifier()
                            {
                                return !string.IsNullOrEmpty(buildingIdentifier.value);
                            }

                            [XmlElement(ElementName = "SuiteIdentifier")]
                            public SuiteIdentifier suiteIdentifier { get; set; }
                            public bool ShouldSerializesuiteIdentifier()
                            {
                                return !string.IsNullOrEmpty(suiteIdentifier.value);
                            }

                            [XmlElement(ElementName = "FloorIdentifier")]
                            public FloorIdentifier floorIdentifier { get; set; }
                            public bool ShouldSerializefloorIdentifier()
                            {
                                return !string.IsNullOrEmpty(floorIdentifier.value);
                            }

                            [XmlElement(ElementName = "DistrictName")]
                            public DistrictName districtName { get; set; }
                            public bool ShouldSerializedistrictName()
                            {
                                return !string.IsNullOrEmpty(districtName.value);
                            }

                            [XmlElement(ElementName = "POB")]
                            public POB pOB { get; set; }
                            public bool ShouldSerializepOB()
                            {
                                return !string.IsNullOrEmpty(pOB.value);
                            }

                            [XmlElement(ElementName = "PostCode")]
                            public PostCode postCode { get; set; }
                            public bool ShouldSerializepostCode()
                            {
                                return !string.IsNullOrEmpty(postCode.value);
                            }

                            [XmlElement(ElementName = "City")]
                            public City city { get; set; }
                            public bool ShouldSerializecity()
                            {
                                return !string.IsNullOrEmpty(city.value);
                            }

                            [XmlElement(ElementName = "CountrySubentity")]
                            public CountrySubentity countrySubentity { get; set; }
                            public bool ShouldSerializecountrySubentity()
                            {
                                return !string.IsNullOrEmpty(countrySubentity.value);
                            }

                            public AddressFix()
                            {
                                street = new Street();
                                buildingIdentifier = new BuildingIdentifier();
                                suiteIdentifier = new SuiteIdentifier();
                                floorIdentifier = new FloorIdentifier();
                                districtName = new DistrictName();
                                pOB = new POB();
                                postCode = new PostCode();
                                city = new City();
                                countrySubentity = new CountrySubentity();
                            }

                            public class Street
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class BuildingIdentifier
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class SuiteIdentifier
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class FloorIdentifier
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class DistrictName
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class POB
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class PostCode
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class City
                            {
                                [XmlText]
                                public string value { get; set; }
                            }

                            public class CountrySubentity
                            {
                                [XmlText]
                                public string value { get; set; }
                            }
                        }
                    }
                }

                public class BizActivities
                {
                    [XmlText]
                    public string value { get; set; }
                }

                public class OtherEntityInfo
                {
                    [XmlText]
                    public string value { get; set; }
                }

            }
        }

        public CbcBodyHK()
        {
            cbcReports = new List<CbcReports>();
        }
    }
}