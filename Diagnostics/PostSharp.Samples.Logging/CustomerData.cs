using PostSharp.Patterns.Formatters;

namespace PostSharp.Samples.Logging
{
    class CustomerData : PostSharp.Patterns.Formatters.IFormattable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        void Patterns.Formatters.IFormattable.Format(UnsafeStringBuilder stringBuilder, FormattingRole role)
        {
            stringBuilder.Append("{CustomerData FirstName=\"");
            stringBuilder.Append(this.FirstName);
            stringBuilder.Append("\", LastName=\"");
            stringBuilder.Append(this.LastName);
            stringBuilder.Append("}");
        }
    }
}
