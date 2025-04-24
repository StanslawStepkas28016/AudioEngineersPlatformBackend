using System;

namespace AudioEngineersPlatformBackend.Exceptions;

public class LocalizedArgumentException : Exception
{
    public string ErrorKey { get; }
    public object[] FormatParameters { get; }

    public LocalizedArgumentException(string errorKey, params object[] formatParameters)
    {
        ErrorKey = errorKey;
        FormatParameters = formatParameters;
    }
}