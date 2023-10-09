using Push_License.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Input;
using ConnectLib;

namespace Push_License.ViewModel
{
    class LicenseViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private DateTime zeroTime = DateTime.MinValue;
        private string _MY_END_TIME = "23:59:59";
        private string _MY_TIME_ZONE = "+09:00";
        private string[] LicenseTypes = { "License 1", "License 2", "License 3" };

        #region Validation Interface

        // Validation Dictionary : Property를 Key로, Validation Result 메세지를 
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        // WPF에서 사용하는 Validation Result 확인용 변수와 이벤트 핸들러
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        #endregion

        #region ViewModel Variables

        private string _companyCode;
        public string CompanyCode
        {
            get { return _companyCode; }
            set
            {
                _companyCode = value;
                OnPropertyChanged(nameof(CompanyCode));
                UpdateCompanyCodeButtonEnabled();
            }
        }

        private string _companyCodeMessage;
        public string CompanyCodeMessage
        {
            get { return _companyCodeMessage; }
            set
            {
                _companyCodeMessage = value;
                OnPropertyChanged(nameof(CompanyCodeMessage));
                ClearOtherMessage();
            }
        }

        private string _licenseKey;
        public string LicenseKey
        {
            get { return _licenseKey; }
            set
            {
                _licenseKey = value;
                OnPropertyChanged(nameof(LicenseKey));
                UpdateLicenseKeyButtonEnabled();
            }
        }

        private string _licenseKeyMessage;
        public string LicenseKeyMessage
        {
            get { return _licenseKeyMessage; }
            set
            {
                _licenseKeyMessage = value;
                OnPropertyChanged(nameof(LicenseKeyMessage));
            }
        }
        

        private string _licenseCount;
        public string LicenseCount
        {
            get { return _licenseCount; }
            set
            {
                _licenseCount = value;
                OnPropertyChanged(nameof(LicenseCount));
                UpdateLicenseCountEnabled();
            }
        }

        private int _licenseType;
        public int LicenseType
        {
            get { return _licenseType; }
            set
            {
                _licenseType = value;
                OnPropertyChanged(nameof(LicenseType));
                UpdateLicenseType();
            }
        }

        // Date should be like this : 2023-07-01T00:00:00+09:00
        private DateTime _start = DateTime.Now;
        public DateTime Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged(nameof(Start));
                UpdateLicenseCreateButtonEnabled();
            }
        }

        // Date should be like this : 2023-07-01T00:00:00+09:00
        private DateTime _end = DateTime.Now;
        public DateTime End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged(nameof(End));
                UpdateLicenseCreateButtonEnabled();
            }
        }

        private bool _isCompanyCodeEnabled;
        public bool IsCompanyCodeEnabled
        {
            get { return _isCompanyCodeEnabled; }
            set
            {
                _isCompanyCodeEnabled = value;
                OnPropertyChanged(nameof(IsCompanyCodeEnabled));
            }
        }

        private bool _isAvailableCompanyCode;
        public bool IsAvailableCompanyCode
        {
            get { return _isAvailableCompanyCode; }
            set
            {
                _isAvailableCompanyCode = value;
                OnPropertyChanged(nameof(IsAvailableCompanyCode));
                UpdateLicenseCreateButtonEnabled();
            }
        }

        private bool _isAvailableInputLicenseKey;
        public bool IsAvailableInputLicenseKey
        {
            get { return _isAvailableInputLicenseKey; }
            set
            {
                _isAvailableInputLicenseKey = value;
                OnPropertyChanged(nameof(IsAvailableInputLicenseKey));
                UpdateLicenseCreateButtonEnabled();
            }
        }

        private bool _isAvailableLicenseKey;
        public bool IsAvailableLicenseKey
        {
            get { return _isAvailableLicenseKey; }
            set
            {
                _isAvailableLicenseKey = value;
                OnPropertyChanged(nameof(IsAvailableLicenseKey));
                UpdateLicenseCreateButtonEnabled();
            }
        }

        private bool _isLicenseKeyEnabled;
        public bool IsLicenseKeyEnabled
        {
            get { return _isLicenseKeyEnabled; }
            set
            {
                _isLicenseKeyEnabled = value;
                OnPropertyChanged(nameof(IsLicenseKeyEnabled));
            }
        }

        private bool _isAvailableInputLicenseCount;
        public bool IsAvailableInputLicenseCount
        {
            get { return _isAvailableInputLicenseCount; }
            set
            {
                _isAvailableInputLicenseCount = value;
                OnPropertyChanged(nameof(IsAvailableInputLicenseCount));
            }
        }

        private bool _isLicenseCountEnabled;
        public bool IsLicenseCountEnabled
        {
            get { return _isLicenseCountEnabled; }
            set
            {
                _isLicenseCountEnabled = value;
                OnPropertyChanged(nameof(IsLicenseCountEnabled));
            }
        }

        private bool _isLicenseCreateEnabled;
        public bool IsLicenseCreateEnabled
        {
            get { return _isLicenseCreateEnabled; }
            set
            {
                _isLicenseCreateEnabled = value;
                OnPropertyChanged(nameof(IsLicenseCreateEnabled));
            }
        }

        #endregion

        public LicenseViewModel()
        {
            CompanyCodeMessage = "";

            Start = zeroTime;
            End = zeroTime;

            IsAvailableInputLicenseKey = false;
            IsAvailableInputLicenseCount = false;
            IsAvailableCompanyCode = false;
            IsAvailableLicenseKey = false;

            LicenseCreateCommand = new ButtonCommand(ExecuteLicenseCreateCommand);
            CompanyCodeCheckCommand = new ButtonCommand(ExecuteCompanyCodeCheckCommand);
            LicenseKeyCheckCommand = new ButtonCommand(ExecuteLicenseKeyCheckCommand);
        }

        #region Updates

        private void UpdateCompanyCodeButtonEnabled()
        {
            CompanyCodeMessage = "";
            ClearInputFields();
            IsAvailableCompanyCode = false;
            IsAvailableInputLicenseKey = false;
            IsCompanyCodeEnabled = (!string.IsNullOrEmpty(CompanyCode) && Regex.IsMatch(CompanyCode, "^[A-Z]+\\d+$") && CompanyCode.Length < 10);
            if (!IsCompanyCodeEnabled)
                IsLicenseKeyEnabled = false;
            UpdateLicenseCreateButtonEnabled();
        }

        private void UpdateLicenseKeyButtonEnabled()
        {
            LicenseKeyMessage = "";
            IsAvailableLicenseKey = false;
            IsLicenseKeyEnabled = (!string.IsNullOrEmpty(LicenseKey) && Regex.IsMatch(LicenseKey, "^[A-Z]+$") && LicenseKey.Length < 4);
            if (IsLicenseKeyEnabled)
                IsAvailableInputLicenseCount = true;
            else
                IsAvailableInputLicenseCount = false;
            UpdateLicenseCreateButtonEnabled();
        }

        private void UpdateLicenseCountEnabled()
        {
            IsLicenseCountEnabled = (!string.IsNullOrEmpty(LicenseCount) && Regex.IsMatch(LicenseCount, "^[0-9]+$") && LicenseCount.Length < 4);
            UpdateLicenseCreateButtonEnabled();
        }

        private void UpdateLicenseType()
        {
            //IsCompanyCodeEnabled = false;
            IsAvailableCompanyCode = false;
            //IsLicenseKeyEnabled = false;
            //IsLicenseCountEnabled = false;
            IsAvailableLicenseKey = false;
            IsLicenseCreateEnabled = false;

            CompanyCodeMessage = "";
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            LicenseKey = "";
            LicenseCount = "";
        }

        private void UpdateLicenseCreateButtonEnabled()
        {
            if (IsCompanyCodeEnabled && IsAvailableCompanyCode && IsLicenseKeyEnabled && IsLicenseCountEnabled && Start != zeroTime && End != zeroTime
                && IsAvailableLicenseKey)
                IsLicenseCreateEnabled = true;
            else
                IsLicenseCreateEnabled = false;
        }

        #endregion

        #region Button Command

        public ICommand LicenseCreateCommand { get; }
        public ICommand CompanyCodeCheckCommand { get; }
        public ICommand LicenseKeyCheckCommand { get; }

        private void ExecuteLicenseCreateCommand(object obj)
        {
            CreateLicense();
        }

        private void ExecuteCompanyCodeCheckCommand(object obj)
        {
            CheckCompanyCodeExists();
        }

        private void ExecuteLicenseKeyCheckCommand(object obj)
        {
            CheckLicenseKeyExists();
        }

        #endregion

        #region Function

        private void ClearOtherMessage()
        {
            LicenseKeyMessage = "";
        }

        private async void CreateLicense()
        {
            string[] res = await Connect.Instance._wsLicenseInput(CompanyCode, LicenseKey, int.Parse(LicenseCount), Start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss" + _MY_TIME_ZONE),
                End.ToString("yyyy'-'MM'-'dd'T'" + _MY_END_TIME + _MY_TIME_ZONE), LicenseTypes[LicenseType]);
            if (res == null)
            {
                MessageBox.Show("서버 연결 오류");
                return;
            }
            else if (res.Length < 2)
            {
                MessageBox.Show(res[0]);
                return;
            }

            // res 에 따른 처리
            if (res[3] == "created")
                MessageBox.Show("라이선스가 생성되었습니다.");
            else if (res[0] == "null")
                MessageBox.Show("Company Code 조회 오류");
            else if (res[1] == "error")
                MessageBox.Show("Company Code 생성 오류");
            else if (res[2] == "error")
                MessageBox.Show("License Key 무결성 검사 에러");
            else if (res[3] == "not matched")
                MessageBox.Show("일관성 없는 License Key 감지 오류 : \"" + res[2] + "\"가 다른 테이블에 존재합니다.");
            else if (res[3] == "error")
                MessageBox.Show("License Key 생성 오류");

            return;
        }

        private async void CheckCompanyCodeExists()
        {
            ClearInputFields();
            IsAvailableCompanyCode = false;
            IsAvailableInputLicenseKey = false;
            // CompanyCode Check WebSocket Query
            string[] res = await Connect.Instance._wsCheckCompanyCode(CompanyCode, LicenseTypes[LicenseType]);
            //var validationResults = new List<ValidationResult>();
            //ValidationResult validationResult = null;

            if (res == null)
            {
                MessageBox.Show("서버 연결 오류");
                return;
            }
            else if (res.Length < 2)
            {
                MessageBox.Show(res[0]);
                return;
            }

            IsAvailableCompanyCode = true;
            IsAvailableInputLicenseKey = true;

            // Extra Validates
            // Company Code 없음
            if (res[0] == "false")
            {
                //IsAvailableInputLicenseKey = false;
                // 나중엔 그냥 MessageForm 을 따로 만들어 처리하자
                //validationResult = new ValidationResult($"{CompanyCode}는 존재하지 않는 회사코드이므로 생성 가능합니다.");
                CompanyCodeMessage = $"{CompanyCode}는 존재하지 않는 회사코드이므로 생성 가능합니다.";
            }
            else
            {
                if (res[0] == "true" && res[1] == "false")
                    CompanyCodeMessage = "이 회사코드로 발급된 LicenseKey는 없습니다.";
                else if (res[0] == "true" && res[1] != "false")
                {
                    CompanyCodeMessage = $"이 회사코드의 최신 키는 {res[1]}입니다.";
                    Match match = Regex.Match(res[1], @"([A-Z]+)(\d+)");
                    LicenseKey = match.Groups[1].Value;
                    IsAvailableInputLicenseKey = false;
                }
            }
        }

        private async void CheckLicenseKeyExists()
        {
            IsAvailableLicenseKey = false;
            var validationResults = new List<ValidationResult>();
            ValidationResult validationResult = null;
            bool res_bool = false;

            string res = await Connect.Instance._wsCheckLicenseKey(CompanyCode, LicenseKey, LicenseTypes[LicenseType]);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("서버 연결 오류");
                return;
            }

            try
            {
                res_bool = bool.Parse(res);
            }
            catch
            {
                MessageBox.Show(res);
                return;
            }

            if (!res_bool)
                validationResult = new ValidationResult($"{LicenseKey}는 이미 다른 회사코드에 존재합니다.");
            else
            {
                LicenseKeyMessage = "사용 가능한 라이선스 키입니다.";
                IsAvailableLicenseKey = true;
            }

            if (validationResult != null)
                validationResults.Add(validationResult);

            if (_errorsByPropertyName.ContainsKey("LicenseKey"))
                _errorsByPropertyName.Remove("LicenseKey");

            if (validationResults.Any())
                _errorsByPropertyName.Add("LicenseKey", validationResults.Select(c => c.ErrorMessage).ToList());

            OnErrorsChanged("LicenseKey");
        }

        #endregion
    }
}
