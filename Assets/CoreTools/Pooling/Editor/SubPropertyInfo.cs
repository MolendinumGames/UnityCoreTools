namespace CoreTools.Pooling.CoreEditor
{
    internal struct SubPropertyInfo
    {
        public string PropertyName;
        public string Description;

        public SubPropertyInfo(string propertyName, string description)
        {
            this.PropertyName = propertyName;
            this.Description = description;
        }
    }
}
