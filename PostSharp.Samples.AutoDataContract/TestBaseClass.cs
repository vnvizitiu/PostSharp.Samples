using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.AutoDataContract
{
    [AutoDataContract]
    class TestBaseClass
    {
        public int SerializedProperty1 { get; set; }

        public string SerializedProperty2 { get; set; }

        [NotDataMember]
        public int NonSerializedProperty3 { get; set; }
    }
}
