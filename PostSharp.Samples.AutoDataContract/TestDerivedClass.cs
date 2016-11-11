namespace PostSharp.Samples.AutoDataContract
{
    class TestDerivedClass : TestBaseClass
    {
        public int SerializedProperty4 { get; set; }

        public string SerializedProperty5 { get; set; }

        [NotDataMember]
        public int NonSerializedProperty6 { get; set; }
    }
}