﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AudioEngineersPlatformBackend.Resources {
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("AudioEngineersPlatformBackend.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string UsernameAlreadyTaken {
            get {
                return ResourceManager.GetString("UsernameAlreadyTaken", resourceCulture);
            }
        }
        
        public static string ProvidedDataIsNullOrEmpty {
            get {
                return ResourceManager.GetString("ProvidedDataIsNullOrEmpty", resourceCulture);
            }
        }
        
        public static string EmailIncorrect {
            get {
                return ResourceManager.GetString("EmailIncorrect", resourceCulture);
            }
        }
        
        public static string EmailAlreadyTaken {
            get {
                return ResourceManager.GetString("EmailAlreadyTaken", resourceCulture);
            }
        }
        
        public static string PhoneNumberIncorrect {
            get {
                return ResourceManager.GetString("PhoneNumberIncorrect", resourceCulture);
            }
        }
        
        public static string PhoneNumberAlreadyTaken {
            get {
                return ResourceManager.GetString("PhoneNumberAlreadyTaken", resourceCulture);
            }
        }
        
        public static string RoleNotFound {
            get {
                return ResourceManager.GetString("RoleNotFound", resourceCulture);
            }
        }
        
        public static string AdminSecretInvalid {
            get {
                return ResourceManager.GetString("AdminSecretInvalid", resourceCulture);
            }
        }
        
        public static string VerificationCodeInvalid {
            get {
                return ResourceManager.GetString("VerificationCodeInvalid", resourceCulture);
            }
        }
        
        public static string VerificationCodeInvalidFormat {
            get {
                return ResourceManager.GetString("VerificationCodeInvalidFormat", resourceCulture);
            }
        }
        
        public static string VerificationCodeExpired {
            get {
                return ResourceManager.GetString("VerificationCodeExpired", resourceCulture);
            }
        }
    }
}
