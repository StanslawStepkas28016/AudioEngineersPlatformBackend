using System;

namespace AudioEngineersPlatformBackend.Exceptions;

public class LocalizedGeneralException : Exception
{
    public string ErrorKey { get; }
    public object[] FormatParameters { get; }

    public LocalizedGeneralException(string errorKey, params object[] formatParameters)
    {
        ErrorKey = errorKey;
        FormatParameters = formatParameters;
    }
}