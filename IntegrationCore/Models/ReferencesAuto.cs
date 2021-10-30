using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class UnauthorizedResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string typeField;

        private string titleField;

        private string statusField;

        private string detailField;

        private string instanceField;

        private string additionalProp1Field;

        private string additionalProp2Field;

        private string additionalProp3Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
                this.RaisePropertyChanged("title");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
                this.RaisePropertyChanged("detail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string instance
        {
            get
            {
                return this.instanceField;
            }
            set
            {
                this.instanceField = value;
                this.RaisePropertyChanged("instance");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string additionalProp1
        {
            get
            {
                return this.additionalProp1Field;
            }
            set
            {
                this.additionalProp1Field = value;
                this.RaisePropertyChanged("additionalProp1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string additionalProp2
        {
            get
            {
                return this.additionalProp2Field;
            }
            set
            {
                this.additionalProp2Field = value;
                this.RaisePropertyChanged("additionalProp2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string additionalProp3
        {
            get
            {
                return this.additionalProp3Field;
            }
            set
            {
                this.additionalProp3Field = value;
                this.RaisePropertyChanged("additionalProp3");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat", ConfigurationName = "HRArray.HRMurasalat_ptt")]
    public interface HRMurasalat_ptt
    {

        // CODEGEN: Generating message contract since the operation GetEmployeeDetails is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetEmployeeDetailsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetEmployeeDetailsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetEmployeeDetailsResponse1 GetEmployeeDetails(GetEmployeeDetailsRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetEmployeeDetailsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetEmployeeDetailsResponse")]
        System.Threading.Tasks.Task<GetEmployeeDetailsResponse1> GetEmployeeDetailsAsync(GetEmployeeDetailsRequest1 request);

        // CODEGEN: Generating message contract since the operation GetListOfAllDepartments is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfAllDepartmentsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfAllDepartmentsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetListOfAllDepartmentsResponse1 GetListOfAllDepartments(GetListOfAllDepartmentsRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfAllDepartmentsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfAllDepartmentsResponse")]
        System.Threading.Tasks.Task<GetListOfAllDepartmentsResponse1> GetListOfAllDepartmentsAsync(GetListOfAllDepartmentsRequest1 request);

        // CODEGEN: Generating message contract since the operation GetListOfUpdatedDepartments is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedDepartmentsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedDepartmentsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetListOfUpdatedDepartmentsResponse1 GetListOfUpdatedDepartments(GetListOfUpdatedDepartmentsRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedDepartmentsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedDepartmentsResponse")]
        System.Threading.Tasks.Task<GetListOfUpdatedDepartmentsResponse1> GetListOfUpdatedDepartmentsAsync(GetListOfUpdatedDepartmentsRequest1 request);

        // CODEGEN: Generating message contract since the operation GetListOfUpdatedEmployees is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedEmployeesRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedEmployeesResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetListOfUpdatedEmployeesResponse1 GetListOfUpdatedEmployees(GetListOfUpdatedEmployeesRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedEmployeesRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedEmployeesResponse")]
        System.Threading.Tasks.Task<GetListOfUpdatedEmployeesResponse1> GetListOfUpdatedEmployeesAsync(GetListOfUpdatedEmployeesRequest1 request);

        // CODEGEN: Generating message contract since the operation GetListOfUpdatedOccupations is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedOccupationsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedOccupationsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetListOfUpdatedOccupationsResponse1 GetListOfUpdatedOccupations(GetListOfUpdatedOccupationsRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedOccupationsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfUpdatedOccupationsResponse")]
        System.Threading.Tasks.Task<GetListOfUpdatedOccupationsResponse1> GetListOfUpdatedOccupationsAsync(GetListOfUpdatedOccupationsRequest1 request);

        // CODEGEN: Generating message contract since the operation GetListOfVacations is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfVacationsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfVacationsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(UnauthorizedResponse), Action = "http://www.w3.org/2005/08/addressing/soap/fault", Name = "UnauthorizedResponse", Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        GetListOfVacationsResponse1 GetListOfVacations(GetListOfVacationsRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfVacationsRequest", ReplyAction = "http://xmlns.oracle.com/NatServicesSBApplication/MoiIntegration/HRMurasalat/HRMur" +
            "asalat_ptt/GetListOfVacationsResponse")]
        System.Threading.Tasks.Task<GetListOfVacationsResponse1> GetListOfVacationsAsync(GetListOfVacationsRequest1 request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetEmployeeDetailsRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string employeeIDField;

        private string militaryNumberField;

        private string emiratesIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string EmployeeID
        {
            get
            {
                return this.employeeIDField;
            }
            set
            {
                this.employeeIDField = value;
                this.RaisePropertyChanged("EmployeeID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string MilitaryNumber
        {
            get
            {
                return this.militaryNumberField;
            }
            set
            {
                this.militaryNumberField = value;
                this.RaisePropertyChanged("MilitaryNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string EmiratesID
        {
            get
            {
                return this.emiratesIDField;
            }
            set
            {
                this.emiratesIDField = value;
                this.RaisePropertyChanged("EmiratesID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetEmployeeDetailsResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private GetEmployeeDetailsResponseContent contentField;

        private GetEmployeeDetailsResponseErrors[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GetEmployeeDetailsResponseContent Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Errors", Order = 1)]
        public GetEmployeeDetailsResponseErrors[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetEmployeeDetailsResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string employeeIDField;

        private string militaryNumberField;

        private string arabicEmployeeNameField;

        private string englishEmployeeNameField;

        private string externalEmailField;

        private string internalEmailField;

        private string phonceNumberField;

        private string rankIDField;

        private string rankNameField;

        private string occupationGroupIDField;

        private string occupationGroupNameField;

        private string occupationJobIDField;

        private string occupationJobNameField;

        private string jobTileGroupIDField;

        private string jobTileGroupNameField;

        private string jobTitleIDField;

        private string jobTitleNameField;

        private string genderField;

        private string dateOfBirthField;

        private string emiratesIDField;

        private string categoryIDField;

        private string categoryNameField;

        private string countryNameField;

        private string lastDepartmentIDField;

        private string lastDepartmentNameField;

        private string mainDepartmentIDField;

        private string mainDepartmentNameField;

        private string subDepartmentIDField;

        private string subDepartmentNameField;

        private string departmentIDField;

        private string departmentNameField;

        private string sectionIDField;

        private string sectionNameField;

        private string branchIDField;

        private string branchNameField;

        private string employeeStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string EmployeeID
        {
            get
            {
                return this.employeeIDField;
            }
            set
            {
                this.employeeIDField = value;
                this.RaisePropertyChanged("EmployeeID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string MilitaryNumber
        {
            get
            {
                return this.militaryNumberField;
            }
            set
            {
                this.militaryNumberField = value;
                this.RaisePropertyChanged("MilitaryNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ArabicEmployeeName
        {
            get
            {
                return this.arabicEmployeeNameField;
            }
            set
            {
                this.arabicEmployeeNameField = value;
                this.RaisePropertyChanged("ArabicEmployeeName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string EnglishEmployeeName
        {
            get
            {
                return this.englishEmployeeNameField;
            }
            set
            {
                this.englishEmployeeNameField = value;
                this.RaisePropertyChanged("EnglishEmployeeName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ExternalEmail
        {
            get
            {
                return this.externalEmailField;
            }
            set
            {
                this.externalEmailField = value;
                this.RaisePropertyChanged("ExternalEmail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string InternalEmail
        {
            get
            {
                return this.internalEmailField;
            }
            set
            {
                this.internalEmailField = value;
                this.RaisePropertyChanged("InternalEmail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string PhonceNumber
        {
            get
            {
                return this.phonceNumberField;
            }
            set
            {
                this.phonceNumberField = value;
                this.RaisePropertyChanged("PhonceNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 7)]
        public string RankID
        {
            get
            {
                return this.rankIDField;
            }
            set
            {
                this.rankIDField = value;
                this.RaisePropertyChanged("RankID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string RankName
        {
            get
            {
                return this.rankNameField;
            }
            set
            {
                this.rankNameField = value;
                this.RaisePropertyChanged("RankName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 9)]
        public string OccupationGroupID
        {
            get
            {
                return this.occupationGroupIDField;
            }
            set
            {
                this.occupationGroupIDField = value;
                this.RaisePropertyChanged("OccupationGroupID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string OccupationGroupName
        {
            get
            {
                return this.occupationGroupNameField;
            }
            set
            {
                this.occupationGroupNameField = value;
                this.RaisePropertyChanged("OccupationGroupName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 11)]
        public string OccupationJobID
        {
            get
            {
                return this.occupationJobIDField;
            }
            set
            {
                this.occupationJobIDField = value;
                this.RaisePropertyChanged("OccupationJobID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string OccupationJobName
        {
            get
            {
                return this.occupationJobNameField;
            }
            set
            {
                this.occupationJobNameField = value;
                this.RaisePropertyChanged("OccupationJobName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 13)]
        public string JobTileGroupID
        {
            get
            {
                return this.jobTileGroupIDField;
            }
            set
            {
                this.jobTileGroupIDField = value;
                this.RaisePropertyChanged("JobTileGroupID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string JobTileGroupName
        {
            get
            {
                return this.jobTileGroupNameField;
            }
            set
            {
                this.jobTileGroupNameField = value;
                this.RaisePropertyChanged("JobTileGroupName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 15)]
        public string JobTitleID
        {
            get
            {
                return this.jobTitleIDField;
            }
            set
            {
                this.jobTitleIDField = value;
                this.RaisePropertyChanged("JobTitleID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string JobTitleName
        {
            get
            {
                return this.jobTitleNameField;
            }
            set
            {
                this.jobTitleNameField = value;
                this.RaisePropertyChanged("JobTitleName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
                this.RaisePropertyChanged("DateOfBirth");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string EmiratesID
        {
            get
            {
                return this.emiratesIDField;
            }
            set
            {
                this.emiratesIDField = value;
                this.RaisePropertyChanged("EmiratesID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 20)]
        public string CategoryID
        {
            get
            {
                return this.categoryIDField;
            }
            set
            {
                this.categoryIDField = value;
                this.RaisePropertyChanged("CategoryID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string CategoryName
        {
            get
            {
                return this.categoryNameField;
            }
            set
            {
                this.categoryNameField = value;
                this.RaisePropertyChanged("CategoryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string CountryName
        {
            get
            {
                return this.countryNameField;
            }
            set
            {
                this.countryNameField = value;
                this.RaisePropertyChanged("CountryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string LastDepartmentID
        {
            get
            {
                return this.lastDepartmentIDField;
            }
            set
            {
                this.lastDepartmentIDField = value;
                this.RaisePropertyChanged("LastDepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string LastDepartmentName
        {
            get
            {
                return this.lastDepartmentNameField;
            }
            set
            {
                this.lastDepartmentNameField = value;
                this.RaisePropertyChanged("LastDepartmentName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string MainDepartmentID
        {
            get
            {
                return this.mainDepartmentIDField;
            }
            set
            {
                this.mainDepartmentIDField = value;
                this.RaisePropertyChanged("MainDepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public string MainDepartmentName
        {
            get
            {
                return this.mainDepartmentNameField;
            }
            set
            {
                this.mainDepartmentNameField = value;
                this.RaisePropertyChanged("MainDepartmentName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public string SubDepartmentID
        {
            get
            {
                return this.subDepartmentIDField;
            }
            set
            {
                this.subDepartmentIDField = value;
                this.RaisePropertyChanged("SubDepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public string SubDepartmentName
        {
            get
            {
                return this.subDepartmentNameField;
            }
            set
            {
                this.subDepartmentNameField = value;
                this.RaisePropertyChanged("SubDepartmentName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public string DepartmentID
        {
            get
            {
                return this.departmentIDField;
            }
            set
            {
                this.departmentIDField = value;
                this.RaisePropertyChanged("DepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 30)]
        public string DepartmentName
        {
            get
            {
                return this.departmentNameField;
            }
            set
            {
                this.departmentNameField = value;
                this.RaisePropertyChanged("DepartmentName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 31)]
        public string SectionID
        {
            get
            {
                return this.sectionIDField;
            }
            set
            {
                this.sectionIDField = value;
                this.RaisePropertyChanged("SectionID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 32)]
        public string SectionName
        {
            get
            {
                return this.sectionNameField;
            }
            set
            {
                this.sectionNameField = value;
                this.RaisePropertyChanged("SectionName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 33)]
        public string BranchID
        {
            get
            {
                return this.branchIDField;
            }
            set
            {
                this.branchIDField = value;
                this.RaisePropertyChanged("BranchID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 34)]
        public string BranchName
        {
            get
            {
                return this.branchNameField;
            }
            set
            {
                this.branchNameField = value;
                this.RaisePropertyChanged("BranchName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 35)]
        public string EmployeeStatus
        {
            get
            {
                return this.employeeStatusField;
            }
            set
            {
                this.employeeStatusField = value;
                this.RaisePropertyChanged("EmployeeStatus");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetEmployeeDetailsResponseErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetEmployeeDetailsRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetEmployeeDetailsRequest GetEmployeeDetailsRequest;

        public GetEmployeeDetailsRequest1()
        {
        }

        public GetEmployeeDetailsRequest1(GetEmployeeDetailsRequest GetEmployeeDetailsRequest)
        {
            this.GetEmployeeDetailsRequest = GetEmployeeDetailsRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetEmployeeDetailsResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetEmployeeDetailsResponse GetEmployeeDetailsResponse;

        public GetEmployeeDetailsResponse1()
        {
        }

        public GetEmployeeDetailsResponse1(GetEmployeeDetailsResponse GetEmployeeDetailsResponse)
        {
            this.GetEmployeeDetailsResponse = GetEmployeeDetailsResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfAllDepartmentsRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string departmentNameField;

        private string departmentIDField;

        private string parentIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string DepartmentName
        {
            get
            {
                return this.departmentNameField;
            }
            set
            {
                this.departmentNameField = value;
                this.RaisePropertyChanged("DepartmentName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DepartmentID
        {
            get
            {
                return this.departmentIDField;
            }
            set
            {
                this.departmentIDField = value;
                this.RaisePropertyChanged("DepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ParentID
        {
            get
            {
                return this.parentIDField;
            }
            set
            {
                this.parentIDField = value;
                this.RaisePropertyChanged("ParentID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfAllDepartmentsResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private List<GetListOfAllDepartmentsResponseContent> contentField;

        private GetListOfAllDepartmentsResponseListOfErrors[] listOfErrorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public List<GetListOfAllDepartmentsResponseContent> Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ListOfErrors", Order = 1)]
        public GetListOfAllDepartmentsResponseListOfErrors[] ListOfErrors
        {
            get
            {
                return this.listOfErrorsField;
            }
            set
            {
                this.listOfErrorsField = value;
                this.RaisePropertyChanged("ListOfErrors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfAllDepartmentsResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string departmentIDField;

        private string departmentArabicNameField;

        private string departmentEnglishNameField;

        private string departmentStatusField;

        private string departmentParentIDField;

        private string departmentClassCodeField;

        private string occupationGroupIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string DepartmentID
        {
            get
            {
                return this.departmentIDField;
            }
            set
            {
                this.departmentIDField = value;
                this.RaisePropertyChanged("DepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DepartmentArabicName
        {
            get
            {
                return this.departmentArabicNameField;
            }
            set
            {
                this.departmentArabicNameField = value;
                this.RaisePropertyChanged("DepartmentArabicName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string DepartmentEnglishName
        {
            get
            {
                return this.departmentEnglishNameField;
            }
            set
            {
                this.departmentEnglishNameField = value;
                this.RaisePropertyChanged("DepartmentEnglishName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string DepartmentStatus
        {
            get
            {
                return this.departmentStatusField;
            }
            set
            {
                this.departmentStatusField = value;
                this.RaisePropertyChanged("DepartmentStatus");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string DepartmentParentID
        {
            get
            {
                return this.departmentParentIDField;
            }
            set
            {
                this.departmentParentIDField = value;
                this.RaisePropertyChanged("DepartmentParentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 5)]
        public string DepartmentClassCode
        {
            get
            {
                return this.departmentClassCodeField;
            }
            set
            {
                this.departmentClassCodeField = value;
                this.RaisePropertyChanged("DepartmentClassCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 6)]
        public string OccupationGroupID
        {
            get
            {
                return this.occupationGroupIDField;
            }
            set
            {
                this.occupationGroupIDField = value;
                this.RaisePropertyChanged("OccupationGroupID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfAllDepartmentsResponseListOfErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfAllDepartmentsRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfAllDepartmentsRequest GetListOfAllDepartmentsRequest;

        public GetListOfAllDepartmentsRequest1()
        {
        }

        public GetListOfAllDepartmentsRequest1(GetListOfAllDepartmentsRequest GetListOfAllDepartmentsRequest)
        {
            this.GetListOfAllDepartmentsRequest = GetListOfAllDepartmentsRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfAllDepartmentsResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfAllDepartmentsResponse GetListOfAllDepartmentsResponse;

        public GetListOfAllDepartmentsResponse1()
        {
        }

        public GetListOfAllDepartmentsResponse1(GetListOfAllDepartmentsResponse GetListOfAllDepartmentsResponse)
        {
            this.GetListOfAllDepartmentsResponse = GetListOfAllDepartmentsResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedDepartmentsRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string entryDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string EntryDate
        {
            get
            {
                return this.entryDateField;
            }
            set
            {
                this.entryDateField = value;
                this.RaisePropertyChanged("EntryDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedDepartmentsResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private List<GetListOfUpdatedDepartmentsResponseContent> contentField;

        private GetListOfUpdatedDepartmentsResponseListOfErrors[] listOfErrorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public List<GetListOfUpdatedDepartmentsResponseContent> Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ListOfErrors", Order = 1)]
        public GetListOfUpdatedDepartmentsResponseListOfErrors[] ListOfErrors
        {
            get
            {
                return this.listOfErrorsField;
            }
            set
            {
                this.listOfErrorsField = value;
                this.RaisePropertyChanged("ListOfErrors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedDepartmentsResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string serialNumberField;

        private string departmentIDField;

        private string transactionTypeField;

        private string changedColumnField;

        private string oldValueField;

        private string newValueField;

        private string transactionDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string SerialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DepartmentID
        {
            get
            {
                return this.departmentIDField;
            }
            set
            {
                this.departmentIDField = value;
                this.RaisePropertyChanged("DepartmentID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string TransactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
                this.RaisePropertyChanged("TransactionType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ChangedColumn
        {
            get
            {
                return this.changedColumnField;
            }
            set
            {
                this.changedColumnField = value;
                this.RaisePropertyChanged("ChangedColumn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string OldValue
        {
            get
            {
                return this.oldValueField;
            }
            set
            {
                this.oldValueField = value;
                this.RaisePropertyChanged("OldValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string NewValue
        {
            get
            {
                return this.newValueField;
            }
            set
            {
                this.newValueField = value;
                this.RaisePropertyChanged("NewValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string TransactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
                this.RaisePropertyChanged("TransactionDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedDepartmentsResponseListOfErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedDepartmentsRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedDepartmentsRequest GetListOfUpdatedDepartmentsRequest;

        public GetListOfUpdatedDepartmentsRequest1()
        {
        }

        public GetListOfUpdatedDepartmentsRequest1(GetListOfUpdatedDepartmentsRequest GetListOfUpdatedDepartmentsRequest)
        {
            this.GetListOfUpdatedDepartmentsRequest = GetListOfUpdatedDepartmentsRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedDepartmentsResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedDepartmentsResponse GetListOfUpdatedDepartmentsResponse;

        public GetListOfUpdatedDepartmentsResponse1()
        {
        }

        public GetListOfUpdatedDepartmentsResponse1(GetListOfUpdatedDepartmentsResponse GetListOfUpdatedDepartmentsResponse)
        {
            this.GetListOfUpdatedDepartmentsResponse = GetListOfUpdatedDepartmentsResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedEmployeesRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string entryDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string EntryDate
        {
            get
            {
                return this.entryDateField;
            }
            set
            {
                this.entryDateField = value;
                this.RaisePropertyChanged("EntryDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedEmployeesResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private List<GetListOfUpdatedEmployeesResponseContent> contentField;

        private GetListOfUpdatedEmployeesResponseListOfErrors[] listOfErrorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public List<GetListOfUpdatedEmployeesResponseContent> Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ListOfErrors", Order = 1)]
        public GetListOfUpdatedEmployeesResponseListOfErrors[] ListOfErrors
        {
            get
            {
                return this.listOfErrorsField;
            }
            set
            {
                this.listOfErrorsField = value;
                this.RaisePropertyChanged("ListOfErrors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedEmployeesResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string employeeIDField;

        private string serialNumberField;

        private string transactionTypeField;

        private string oldValueField;

        private string newValueField;

        private string transactionDateField;

        private string columnNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string EmployeeID
        {
            get
            {
                return this.employeeIDField;
            }
            set
            {
                this.employeeIDField = value;
                this.RaisePropertyChanged("EmployeeID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string SerialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string TransactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
                this.RaisePropertyChanged("TransactionType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string OldValue
        {
            get
            {
                return this.oldValueField;
            }
            set
            {
                this.oldValueField = value;
                this.RaisePropertyChanged("OldValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string NewValue
        {
            get
            {
                return this.newValueField;
            }
            set
            {
                this.newValueField = value;
                this.RaisePropertyChanged("NewValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string TransactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
                this.RaisePropertyChanged("TransactionDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ColumnName
        {
            get
            {
                return this.columnNameField;
            }
            set
            {
                this.columnNameField = value;
                this.RaisePropertyChanged("ColumnName");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedEmployeesResponseListOfErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedEmployeesRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedEmployeesRequest GetListOfUpdatedEmployeesRequest;

        public GetListOfUpdatedEmployeesRequest1()
        {
        }

        public GetListOfUpdatedEmployeesRequest1(GetListOfUpdatedEmployeesRequest GetListOfUpdatedEmployeesRequest)
        {
            this.GetListOfUpdatedEmployeesRequest = GetListOfUpdatedEmployeesRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedEmployeesResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedEmployeesResponse GetListOfUpdatedEmployeesResponse;

        public GetListOfUpdatedEmployeesResponse1()
        {
        }

        public GetListOfUpdatedEmployeesResponse1(GetListOfUpdatedEmployeesResponse GetListOfUpdatedEmployeesResponse)
        {
            this.GetListOfUpdatedEmployeesResponse = GetListOfUpdatedEmployeesResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedOccupationsRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string entryDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string EntryDate
        {
            get
            {
                return this.entryDateField;
            }
            set
            {
                this.entryDateField = value;
                this.RaisePropertyChanged("EntryDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedOccupationsResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private GetListOfUpdatedOccupationsResponseContent contentField;

        private GetListOfUpdatedOccupationsResponseListOfErrors[] listOfErrorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GetListOfUpdatedOccupationsResponseContent Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ListOfErrors", Order = 1)]
        public GetListOfUpdatedOccupationsResponseListOfErrors[] ListOfErrors
        {
            get
            {
                return this.listOfErrorsField;
            }
            set
            {
                this.listOfErrorsField = value;
                this.RaisePropertyChanged("ListOfErrors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedOccupationsResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string occupationIDField;

        private string occupationGroupIDField;

        private string occupationArabicNameField;

        private string occupationEnglishNameField;

        private string transactionTypeField;

        private string transactionDateField;

        private string lastDepartmentIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string OccupationID
        {
            get
            {
                return this.occupationIDField;
            }
            set
            {
                this.occupationIDField = value;
                this.RaisePropertyChanged("OccupationID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string OccupationGroupID
        {
            get
            {
                return this.occupationGroupIDField;
            }
            set
            {
                this.occupationGroupIDField = value;
                this.RaisePropertyChanged("OccupationGroupID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string OccupationArabicName
        {
            get
            {
                return this.occupationArabicNameField;
            }
            set
            {
                this.occupationArabicNameField = value;
                this.RaisePropertyChanged("OccupationArabicName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string OccupationEnglishName
        {
            get
            {
                return this.occupationEnglishNameField;
            }
            set
            {
                this.occupationEnglishNameField = value;
                this.RaisePropertyChanged("OccupationEnglishName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string TransactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
                this.RaisePropertyChanged("TransactionType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string TransactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
                this.RaisePropertyChanged("TransactionDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string LastDepartmentID
        {
            get
            {
                return this.lastDepartmentIDField;
            }
            set
            {
                this.lastDepartmentIDField = value;
                this.RaisePropertyChanged("LastDepartmentID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfUpdatedOccupationsResponseListOfErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedOccupationsRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedOccupationsRequest GetListOfUpdatedOccupationsRequest;

        public GetListOfUpdatedOccupationsRequest1()
        {
        }

        public GetListOfUpdatedOccupationsRequest1(GetListOfUpdatedOccupationsRequest GetListOfUpdatedOccupationsRequest)
        {
            this.GetListOfUpdatedOccupationsRequest = GetListOfUpdatedOccupationsRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfUpdatedOccupationsResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfUpdatedOccupationsResponse GetListOfUpdatedOccupationsResponse;

        public GetListOfUpdatedOccupationsResponse1()
        {
        }

        public GetListOfUpdatedOccupationsResponse1(GetListOfUpdatedOccupationsResponse GetListOfUpdatedOccupationsResponse)
        {
            this.GetListOfUpdatedOccupationsResponse = GetListOfUpdatedOccupationsResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfVacationsRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string entryDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string EntryDate
        {
            get
            {
                return this.entryDateField;
            }
            set
            {
                this.entryDateField = value;
                this.RaisePropertyChanged("EntryDate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfVacationsResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private GetListOfVacationsResponseContent contentField;

        private GetListOfVacationsResponseListOfErrors[] listOfErrorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GetListOfVacationsResponseContent Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
                this.RaisePropertyChanged("Content");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ListOfErrors", Order = 1)]
        public GetListOfVacationsResponseListOfErrors[] ListOfErrors
        {
            get
            {
                return this.listOfErrorsField;
            }
            set
            {
                this.listOfErrorsField = value;
                this.RaisePropertyChanged("ListOfErrors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfVacationsResponseContent : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string employeeIDField;

        private string militaryNumberField;

        private string transactionDateField;

        private string transactionSerialField;

        private string transactionSubTypeField;

        private string transactionSubTypeArabicDescriptionField;

        private string transactionSubTypeEnglishDescriptionField;

        private string startDateField;

        private string endDateField;

        private string numberOfDaysField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string EmployeeID
        {
            get
            {
                return this.employeeIDField;
            }
            set
            {
                this.employeeIDField = value;
                this.RaisePropertyChanged("EmployeeID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string MilitaryNumber
        {
            get
            {
                return this.militaryNumberField;
            }
            set
            {
                this.militaryNumberField = value;
                this.RaisePropertyChanged("MilitaryNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string TransactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
                this.RaisePropertyChanged("TransactionDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 3)]
        public string TransactionSerial
        {
            get
            {
                return this.transactionSerialField;
            }
            set
            {
                this.transactionSerialField = value;
                this.RaisePropertyChanged("TransactionSerial");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 4)]
        public string TransactionSubType
        {
            get
            {
                return this.transactionSubTypeField;
            }
            set
            {
                this.transactionSubTypeField = value;
                this.RaisePropertyChanged("TransactionSubType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string TransactionSubTypeArabicDescription
        {
            get
            {
                return this.transactionSubTypeArabicDescriptionField;
            }
            set
            {
                this.transactionSubTypeArabicDescriptionField = value;
                this.RaisePropertyChanged("TransactionSubTypeArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string TransactionSubTypeEnglishDescription
        {
            get
            {
                return this.transactionSubTypeEnglishDescriptionField;
            }
            set
            {
                this.transactionSubTypeEnglishDescriptionField = value;
                this.RaisePropertyChanged("TransactionSubTypeEnglishDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
                this.RaisePropertyChanged("StartDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
                this.RaisePropertyChanged("EndDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 9)]
        public string NumberOfDays
        {
            get
            {
                return this.numberOfDaysField;
            }
            set
            {
                this.numberOfDaysField = value;
                this.RaisePropertyChanged("NumberOfDays");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema")]
    public partial class GetListOfVacationsResponseListOfErrors : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorArabicDescriptionField;

        private string errorEnglishDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorArabicDescription
        {
            get
            {
                return this.errorArabicDescriptionField;
            }
            set
            {
                this.errorArabicDescriptionField = value;
                this.RaisePropertyChanged("ErrorArabicDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorEnglishDescription
        {
            get
            {
                return this.errorEnglishDescriptionField;
            }
            set
            {
                this.errorEnglishDescriptionField = value;
                this.RaisePropertyChanged("ErrorEnglishDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfVacationsRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfVacationsRequest GetListOfVacationsRequest;

        public GetListOfVacationsRequest1()
        {
        }

        public GetListOfVacationsRequest1(GetListOfVacationsRequest GetListOfVacationsRequest)
        {
            this.GetListOfVacationsRequest = GetListOfVacationsRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class GetListOfVacationsResponse1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema", Order = 0)]
        public GetListOfVacationsResponse GetListOfVacationsResponse;

        public GetListOfVacationsResponse1()
        {
        }

        public GetListOfVacationsResponse1(GetListOfVacationsResponse GetListOfVacationsResponse)
        {
            this.GetListOfVacationsResponse = GetListOfVacationsResponse;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface HRMurasalat_pttChannel : HRMurasalat_ptt, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HRMurasalat_pttClient : System.ServiceModel.ClientBase<HRMurasalat_ptt>, HRMurasalat_ptt
    {

        public HRMurasalat_pttClient()
        {
        }

        public HRMurasalat_pttClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public HRMurasalat_pttClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public HRMurasalat_pttClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public HRMurasalat_pttClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetEmployeeDetailsResponse1 HRMurasalat_ptt.GetEmployeeDetails(GetEmployeeDetailsRequest1 request)
        {
            return base.Channel.GetEmployeeDetails(request);
        }

        public GetEmployeeDetailsResponse GetEmployeeDetails(GetEmployeeDetailsRequest GetEmployeeDetailsRequest)
        {
            GetEmployeeDetailsRequest1 inValue = new GetEmployeeDetailsRequest1();
            inValue.GetEmployeeDetailsRequest = GetEmployeeDetailsRequest;
            GetEmployeeDetailsResponse1 retVal = ((HRMurasalat_ptt)(this)).GetEmployeeDetails(inValue);
            return retVal.GetEmployeeDetailsResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetEmployeeDetailsResponse1> HRMurasalat_ptt.GetEmployeeDetailsAsync(GetEmployeeDetailsRequest1 request)
        {
            return base.Channel.GetEmployeeDetailsAsync(request);
        }

        public System.Threading.Tasks.Task<GetEmployeeDetailsResponse1> GetEmployeeDetailsAsync(GetEmployeeDetailsRequest GetEmployeeDetailsRequest)
        {
            GetEmployeeDetailsRequest1 inValue = new GetEmployeeDetailsRequest1();
            inValue.GetEmployeeDetailsRequest = GetEmployeeDetailsRequest;
            return ((HRMurasalat_ptt)(this)).GetEmployeeDetailsAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetListOfAllDepartmentsResponse1 HRMurasalat_ptt.GetListOfAllDepartments(GetListOfAllDepartmentsRequest1 request)
        {
            return base.Channel.GetListOfAllDepartments(request);
        }

        public GetListOfAllDepartmentsResponse GetListOfAllDepartments(GetListOfAllDepartmentsRequest GetListOfAllDepartmentsRequest)
        {
            GetListOfAllDepartmentsRequest1 inValue = new GetListOfAllDepartmentsRequest1();
            inValue.GetListOfAllDepartmentsRequest = GetListOfAllDepartmentsRequest;
            GetListOfAllDepartmentsResponse1 retVal = ((HRMurasalat_ptt)(this)).GetListOfAllDepartments(inValue);
            return retVal.GetListOfAllDepartmentsResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetListOfAllDepartmentsResponse1> HRMurasalat_ptt.GetListOfAllDepartmentsAsync(GetListOfAllDepartmentsRequest1 request)
        {
            return base.Channel.GetListOfAllDepartmentsAsync(request);
        }

        public System.Threading.Tasks.Task<GetListOfAllDepartmentsResponse1> GetListOfAllDepartmentsAsync(GetListOfAllDepartmentsRequest GetListOfAllDepartmentsRequest)
        {
            GetListOfAllDepartmentsRequest1 inValue = new GetListOfAllDepartmentsRequest1();
            inValue.GetListOfAllDepartmentsRequest = GetListOfAllDepartmentsRequest;
            return ((HRMurasalat_ptt)(this)).GetListOfAllDepartmentsAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetListOfUpdatedDepartmentsResponse1 HRMurasalat_ptt.GetListOfUpdatedDepartments(GetListOfUpdatedDepartmentsRequest1 request)
        {
            return base.Channel.GetListOfUpdatedDepartments(request);
        }

        public GetListOfUpdatedDepartmentsResponse GetListOfUpdatedDepartments(GetListOfUpdatedDepartmentsRequest GetListOfUpdatedDepartmentsRequest)
        {
            GetListOfUpdatedDepartmentsRequest1 inValue = new GetListOfUpdatedDepartmentsRequest1();
            inValue.GetListOfUpdatedDepartmentsRequest = GetListOfUpdatedDepartmentsRequest;
            GetListOfUpdatedDepartmentsResponse1 retVal = ((HRMurasalat_ptt)(this)).GetListOfUpdatedDepartments(inValue);
            return retVal.GetListOfUpdatedDepartmentsResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetListOfUpdatedDepartmentsResponse1> HRMurasalat_ptt.GetListOfUpdatedDepartmentsAsync(GetListOfUpdatedDepartmentsRequest1 request)
        {
            return base.Channel.GetListOfUpdatedDepartmentsAsync(request);
        }

        public System.Threading.Tasks.Task<GetListOfUpdatedDepartmentsResponse1> GetListOfUpdatedDepartmentsAsync(GetListOfUpdatedDepartmentsRequest GetListOfUpdatedDepartmentsRequest)
        {
            GetListOfUpdatedDepartmentsRequest1 inValue = new GetListOfUpdatedDepartmentsRequest1();
            inValue.GetListOfUpdatedDepartmentsRequest = GetListOfUpdatedDepartmentsRequest;
            return ((HRMurasalat_ptt)(this)).GetListOfUpdatedDepartmentsAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetListOfUpdatedEmployeesResponse1 HRMurasalat_ptt.GetListOfUpdatedEmployees(GetListOfUpdatedEmployeesRequest1 request)
        {
            return base.Channel.GetListOfUpdatedEmployees(request);
        }

        public GetListOfUpdatedEmployeesResponse GetListOfUpdatedEmployees(GetListOfUpdatedEmployeesRequest GetListOfUpdatedEmployeesRequest)
        {
            GetListOfUpdatedEmployeesRequest1 inValue = new GetListOfUpdatedEmployeesRequest1();
            inValue.GetListOfUpdatedEmployeesRequest = GetListOfUpdatedEmployeesRequest;
            GetListOfUpdatedEmployeesResponse1 retVal = ((HRMurasalat_ptt)(this)).GetListOfUpdatedEmployees(inValue);
            return retVal.GetListOfUpdatedEmployeesResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetListOfUpdatedEmployeesResponse1> HRMurasalat_ptt.GetListOfUpdatedEmployeesAsync(GetListOfUpdatedEmployeesRequest1 request)
        {
            return base.Channel.GetListOfUpdatedEmployeesAsync(request);
        }

        public System.Threading.Tasks.Task<GetListOfUpdatedEmployeesResponse1> GetListOfUpdatedEmployeesAsync(GetListOfUpdatedEmployeesRequest GetListOfUpdatedEmployeesRequest)
        {
            GetListOfUpdatedEmployeesRequest1 inValue = new GetListOfUpdatedEmployeesRequest1();
            inValue.GetListOfUpdatedEmployeesRequest = GetListOfUpdatedEmployeesRequest;
            return ((HRMurasalat_ptt)(this)).GetListOfUpdatedEmployeesAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetListOfUpdatedOccupationsResponse1 HRMurasalat_ptt.GetListOfUpdatedOccupations(GetListOfUpdatedOccupationsRequest1 request)
        {
            return base.Channel.GetListOfUpdatedOccupations(request);
        }

        public GetListOfUpdatedOccupationsResponse GetListOfUpdatedOccupations(GetListOfUpdatedOccupationsRequest GetListOfUpdatedOccupationsRequest)
        {
            GetListOfUpdatedOccupationsRequest1 inValue = new GetListOfUpdatedOccupationsRequest1();
            inValue.GetListOfUpdatedOccupationsRequest = GetListOfUpdatedOccupationsRequest;
            GetListOfUpdatedOccupationsResponse1 retVal = ((HRMurasalat_ptt)(this)).GetListOfUpdatedOccupations(inValue);
            return retVal.GetListOfUpdatedOccupationsResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetListOfUpdatedOccupationsResponse1> HRMurasalat_ptt.GetListOfUpdatedOccupationsAsync(GetListOfUpdatedOccupationsRequest1 request)
        {
            return base.Channel.GetListOfUpdatedOccupationsAsync(request);
        }

        public System.Threading.Tasks.Task<GetListOfUpdatedOccupationsResponse1> GetListOfUpdatedOccupationsAsync(GetListOfUpdatedOccupationsRequest GetListOfUpdatedOccupationsRequest)
        {
            GetListOfUpdatedOccupationsRequest1 inValue = new GetListOfUpdatedOccupationsRequest1();
            inValue.GetListOfUpdatedOccupationsRequest = GetListOfUpdatedOccupationsRequest;
            return ((HRMurasalat_ptt)(this)).GetListOfUpdatedOccupationsAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetListOfVacationsResponse1 HRMurasalat_ptt.GetListOfVacations(GetListOfVacationsRequest1 request)
        {
            return base.Channel.GetListOfVacations(request);
        }

        public GetListOfVacationsResponse GetListOfVacations(GetListOfVacationsRequest GetListOfVacationsRequest)
        {
            GetListOfVacationsRequest1 inValue = new GetListOfVacationsRequest1();
            inValue.GetListOfVacationsRequest = GetListOfVacationsRequest;
            GetListOfVacationsResponse1 retVal = ((HRMurasalat_ptt)(this)).GetListOfVacations(inValue);
            return retVal.GetListOfVacationsResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetListOfVacationsResponse1> HRMurasalat_ptt.GetListOfVacationsAsync(GetListOfVacationsRequest1 request)
        {
            return base.Channel.GetListOfVacationsAsync(request);
        }

        public System.Threading.Tasks.Task<GetListOfVacationsResponse1> GetListOfVacationsAsync(GetListOfVacationsRequest GetListOfVacationsRequest)
        {
            GetListOfVacationsRequest1 inValue = new GetListOfVacationsRequest1();
            inValue.GetListOfVacationsRequest = GetListOfVacationsRequest;
            return ((HRMurasalat_ptt)(this)).GetListOfVacationsAsync(inValue);
        }
    }
}