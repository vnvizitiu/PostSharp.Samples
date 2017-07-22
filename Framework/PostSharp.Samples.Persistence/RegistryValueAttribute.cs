using System;
using Microsoft.Win32;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace PostSharp.Samples.Persistence
{
    [PSerializable]
    [LinesOfCodeAvoided(5)]
    [MulticastAttributeUsage(TargetMemberAttributes = MulticastAttributes.Static)]
    public sealed class RegistryValueAttribute : LocationInterceptionAspect
    {
        bool isFetched;
        string keyFullName;
        private string keyName;
        string valueName;

        public RegistryValueAttribute(string keyName)
        {
            this.keyName = keyName;
        }
        

        public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
        {
            this.keyFullName = "HKEY_CURRENT_USER\\Software\\" + this.keyName;
            this.valueName = targetLocation.Name;
        }


        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (this.isFetched)
            {
                args.ProceedGetValue();
                return;
            }
            else
            {
                this.isFetched = true;

                var stringValue = Registry.GetValue(this.keyFullName, this.valueName, null) as string;
                if (stringValue != null)
                {
                    var value = Convert.ChangeType(stringValue, args.Location.LocationType);
                    args.SetNewValue(value);
                    args.Value = value;
                }
                else
                {
                    args.ProceedGetValue();
                }
            }
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            args.ProceedSetValue();
            Registry.SetValue(this.keyFullName, this.valueName, Convert.ToString(args.Value));
            this.isFetched = true;

        }

    }
}