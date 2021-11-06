using System;
using System.Text;
using IntegrationCore.IntegrationLibrary;
using Microsoft.VisualBasic;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log.config", Watch = true)]

public class Logger : IDisposable
{
    private DateTime _Now = new DateTime();
    private string _FunctionScope = "";
    // Private _LogType As Enum_LogType
    // Private _Message As String = ""
    // Public Shared IsBusy As Boolean
    // Public Shared PendingEvents As Boolean
    public static string LogFolder;
    public bool WriteMsgAtTheEnd = false;
    public string Message = "";
    private static bool _EnableLogging = false;
    private static bool _EnableLoggingInfo = false;
    private static bool _EnableDBLogging = false;
    private static log4net.ILog _Log;



    public static bool EnableLogging
    {
        get
        {
            if (!_EnableLogging)
            {
                string EnableLoggingElement;
                IntegrationSettings.LoadSettings();
                EnableLoggingElement = (string)IntegrationSettings.GetSetting("EnableLogging");

                if (EnableLoggingElement != null)
                {
                    bool Result;
                    if (bool.TryParse(EnableLoggingElement, out Result))
                        _EnableLogging = Result;
                    else
                        _EnableLogging = false;
                }
                else
                    _EnableLogging = false;
            }
            return _EnableLogging;
        }
        set
        {
            _EnableLogging = value;
        }
    }
    public static bool EnableLoggingInfo
    {
        get
        {
            if (!_EnableLoggingInfo)
            {
                string EnableLoggingElement;
                IntegrationSettings.LoadSettings();
                EnableLoggingElement = (string)IntegrationSettings.GetSetting("EnableLoggingInfo");

                if (EnableLoggingElement != null)
                {
                    bool Result;
                    if (bool.TryParse(EnableLoggingElement, out Result))
                        _EnableLoggingInfo = Result;
                    else
                        _EnableLoggingInfo = false;
                }
                else
                    _EnableLoggingInfo = false;
            }
            return _EnableLoggingInfo;
        }
        set
        {
            _EnableLoggingInfo = value;
        }
    }

    public static bool EnableDBLogging
    {
        get
        {
            if (!EnableLogging)
                _EnableDBLogging = false;
            else if (!_EnableDBLogging)
            {
                string EnableDBLoggingElement;

                EnableDBLoggingElement = (string)IntegrationSettings.GetSetting("EnableLogging");

                if (EnableDBLoggingElement != null)
                {
                    bool Result;
                    if (bool.TryParse(EnableDBLoggingElement, out Result))
                        _EnableDBLogging = Result;
                    else
                        _EnableDBLogging = false;
                }
                else
                    _EnableDBLogging = false;
            }
            return _EnableDBLogging;
        }
        set
        {
            _EnableDBLogging = value;
        }
    }


    public static log4net.ILog Log
    {
        get
        {
            if (_Log == null)
                _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return _Log;
        }
        set
        {
            _Log = value;
        }
    }

    public enum Enum_LogType
    {
        _PerfStart = 0,
        _PerfEnd = 1,
        _Error = 2,
        _Info = 3,
        _Critical = 4,
        _file = 5
     }

    class Performance
    {
        public string FunctionName;
        public string Scope;
        public DateTime LastCallTime;
        public Int64 NumberOfCalls;
        public double AverageDuratin;
        public double CurrentDuration;
        public double SumDuration;
        public DateTime LogDay;
        public int ThreadId;

        public Performance()
        {
            LogDay = DateTime.Now;
        }
    }
    public static bool DirectoryExistsChecked = false;

    public Logger(string FunctionScope, string Message = "", bool WriteMsgAtTheEnd = false)
    {
        string _Scope = "";
        _Now = DateTime.Now;
        _FunctionScope = FunctionScope;

        if (EnableLogging)
        {
            this.WriteMsgAtTheEnd = WriteMsgAtTheEnd;
            if (WriteMsgAtTheEnd)
                this.Message = Message;
            System.Diagnostics.StackFrame CallingFunctionStackFrame = new System.Diagnostics.StackFrame(1, true);
            _Scope = CallingFunctionStackFrame.GetMethod().DeclaringType.Namespace + "." + CallingFunctionStackFrame.GetMethod().DeclaringType.Name
             + "." + CallingFunctionStackFrame.GetMethod().Name;
            if (FunctionScope.Trim() != "")
                _Scope = _Scope + "." + FunctionScope;
            DateTime _NowAfterReflection = DateTime.Now;
            AddToLogFile(_Scope, Enum_LogType._PerfStart, Message, "(" + (_NowAfterReflection - _Now).TotalMilliseconds.ToString() + ")");
        }


        _FunctionScope = _Scope;
    }
    private void AddToLogFileWithPerformance(bool Critical = false)
    {
        if (EnableLogging | Critical)
        {
            int CurrThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
            string ScopeWithThread = _FunctionScope + ":" + CurrThread;

            string FolderPath;

            Performance currentfunctionPerformance = new Performance();
            double CurrentDuration;
            try
            {
                CurrentDuration = (DateTime.Now - _Now).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                CurrentDuration = -1;
            }// very big number


            currentfunctionPerformance.CurrentDuration = CurrentDuration;



            try
            {
                if (!string.IsNullOrEmpty(LogFolder))
                    FolderPath = LogFolder + @"\" + "Logs";
                else
                    FolderPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
                if (!string.IsNullOrEmpty(FolderPath))
                {
                    System.IO.DirectoryInfo LogDirectoryInfo = new System.IO.DirectoryInfo(FolderPath);
                    if (!DirectoryExistsChecked)
                    {
                        if (!LogDirectoryInfo.Exists)
                            System.IO.Directory.CreateDirectory(FolderPath);
                        DirectoryExistsChecked = true;
                    }

                    StringBuilder str = new StringBuilder();
                    str.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    str.Append(".");
                    str.Append(DateTime.Now.Millisecond);
                    str.Append(";");
                    // Mazen: Added for Performance while logging
                    // If HostName <> "" Then
                    // HostName = Dns.GetHostName()
                    // End If
                    // str.Append(HostName)
                    // str.Append(";")
                    str.Append(ScopeWithThread);
                    str.Append(";");
                    str.Append(currentfunctionPerformance.CurrentDuration);
                    str.Append(";");
                    str.Append(Enum_LogType._PerfEnd);
                    str.Append(";");
                    if (currentfunctionPerformance.CurrentDuration > 1000)
                        str.Append(Enum_LogType._Critical);

                    if (this.WriteMsgAtTheEnd)
                        str.Append(this.Message.Replace(Constants.vbCrLf, @"\n"));


                    // Beacuse we depend on new line as line separator, we must remove any new line in the message
                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    // str.Append(Message.Replace(vbCrLf, "\n"))
                    str.Append(Constants.vbNewLine);
                    if (Log != null)
                    {
                        if (Log.IsInfoEnabled)
                            Log.Info(str);
                    }
                    else
                        System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + "loggingError.txt", "Log is nothing : Date:" + DateTime.Now.ToString() + " scope:" + _FunctionScope + Constants.vbCrLf);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + "loggingError.txt", "Error catch : Date:" + DateTime.Now.ToString() + " scope:" + _FunctionScope + Constants.vbCrLf);
            }
        }
        else
            return;
    }

    public static void AddToLogFile(string Scope, Enum_LogType Type, string Message, string AdditionalInfo = "", bool TrackInterface = false, bool PrintInterface = false)
    {
        // Loggrr typr info
        int CurrThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
        string ScopeWithThread = Scope + ":" + CurrThread;

        if (PrintInterface)
        {
            if (Type == Enum_LogType._Info)
            {
                Jobs.AddLogJobESB(ScopeWithThread, Message, (int)IntegrationCore.Helper.Enum.responseCode.Success, DateTime.Now, DateTime.Now, 0, Message, (int)IntegrationCore.Helper.Enum.scheduleType.Done );
            }
            else if (Type == Enum_LogType._Error)
            {
                Jobs.AddLogJobESB(ScopeWithThread, Message, (int)IntegrationCore.Helper.Enum.responseCode.Failed, DateTime.Now, DateTime.Now, 0, Message, (int)IntegrationCore.Helper.Enum.scheduleType.Done);
            }
        }

        if (EnableLogging || Type == Enum_LogType._Error /*|| Type == Enum_LogType._Info*/)
        {
            // Dim FilePath As String = ""
            string FolderPath;
            // Dim SR As System.IO.StreamWriter = Nothing
            Performance currentfunctionPerformance = new Performance();


            try
            {
                if (!string.IsNullOrEmpty(LogFolder))
                    FolderPath = LogFolder + @"\Logs";
                else
                    FolderPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
                if (!string.IsNullOrEmpty(FolderPath))
                {
                    System.IO.DirectoryInfo LogDirectoryInfo = new System.IO.DirectoryInfo(FolderPath);
                    if (!DirectoryExistsChecked)
                    {
                        if (!LogDirectoryInfo.Exists)
                            System.IO.Directory.CreateDirectory(FolderPath);
                        DirectoryExistsChecked = true;
                    }

                    // FilePath = FolderPath & "\" & getLogFileName()

                    // Dim str As String = cnstlineSeparator & Now.ToString() & ";"
                    StringBuilder str = new StringBuilder();
                    str.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    str.Append(".");
                    str.Append(DateTime.Now.Millisecond);
                    str.Append(";");
                    // Mazen: Added for Performance while logging
                    // If HostName <> "" Then
                    // HostName = Dns.GetHostName()
                    // End If
                    // str.Append(HostName)

                    // str.Append(";")
                    str.Append(ScopeWithThread);
                    str.Append(";");
                    str.Append(";");
                    str.Append(Type);
                    str.Append(";");

                    if (!string.IsNullOrWhiteSpace(AdditionalInfo))
                        str.Append(";" + AdditionalInfo + ";");

                    // Beacuse we depend on new line as line separator, we must remove any new line in the message
                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^                    
                    if ((Type == Enum_LogType._Error) || (Type == Enum_LogType._Info && TrackInterface))
                    {
                        str.Append(Message.Replace(Constants.vbCrLf, @"\n"));
                    }

                    str.Append(Constants.vbNewLine);
                    if (Log != null)
                    {
                        // If Type = Enum_LogType._InfoStart Or Type = Enum_LogType._InfoEnd Or Type = Enum_LogType._Info Then
                        Log.Info(str);
                    }
                    else
                        System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + "loggingError.txt", "Log is nothing : Date:" + DateTime.Now.ToString() + " scope:" + Scope + Constants.vbCrLf);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + "loggingError.txt", "Error catch : Date:" + DateTime.Now.ToString() + " scope:" + Scope + Constants.vbCrLf);
            }
        }
        else
            return;
    }

    private void AddToLogAsnc()
    {
    }
    private static string getLogFileName()
    {
        return DateTime.Now.ToString("yyyyMMdd") + ".log";
    }

    private bool disposedValue; // To detect redundant calls

    // IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
            }
            if (Logger.EnableLogging)

                // Dim CallingFunctionStackFrame As Diagnostics.StackFrame = New Diagnostics.StackFrame(1, True)



                // Dim _Scope = CallingFunctionStackFrame.GetMethod.DeclaringType.Namespace & "." & CallingFunctionStackFrame.GetMethod.DeclaringType.Name _
                // & "." & CallingFunctionStackFrame.GetMethod.Name
                // If _FunctionScope.Trim <> "" Then
                // _Scope = _Scope & "." & _FunctionScope
                // End If
                AddToLogFileWithPerformance();
        }
        this.disposedValue = true;
    }

    // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    // Protected Overrides Sub Finalize()
    // ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    // Dispose(False)
    // MyBase.Finalize()
    // End Sub

    // This code added by Visual Basic to correctly implement the disposable pattern.
    public void Dispose()
    {
        if (!EnableLogging)
        {
            if ((DateTime.Now - _Now).TotalMilliseconds > 1000)
            {
                string _Scope = "";
                System.Diagnostics.StackFrame CallingFunctionStackFrame = new System.Diagnostics.StackFrame(1, true);
                _Scope = CallingFunctionStackFrame.GetMethod().DeclaringType.Namespace + "." + CallingFunctionStackFrame.GetMethod().DeclaringType.Name
                 + "." + CallingFunctionStackFrame.GetMethod().Name;
                // If Trim(FunctionScope) <> "" Then
                _Scope = _Scope; // & "." & FunctionScope
                _FunctionScope = _Scope + "." + _FunctionScope;

                AddToLogFileWithPerformance(true);
            }
        }
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
