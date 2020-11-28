using System;
using System.Collections.Generic;
using System.Text;

namespace SugarSite.Enties
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ValidateVerifyCodeAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ValidateReduired : Attribute
    {


    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ValidateUnique : Attribute
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
        public string Message { get; set; }
        public string DbColumnName { get; set; }
        public string PrimaryKey { get; set; }

        public ValidateUnique(string tableName, string dbColumnName, string primaryKey)
        {
            this.TableName = tableName;
            this.DbColumnName = dbColumnName;
            this.PrimaryKey = primaryKey;
        }

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ValidateLength : Attribute
    {
        public int Min { get; set; }
        public int Max { get; set; }
        private ValidateLength()
        {

        }

        public ValidateLength(int min, int max)
        {
            this.Max = max;
            this.Min = min;
        }
    }

    public class ValidateEqual : Attribute
    {

        public string EqualPropertyName { get; set; }
        private ValidateEqual()
        {

        }

        public ValidateEqual(string equalPropertyName)
        {
            this.EqualPropertyName = equalPropertyName;
        }
    }

    public class ConvertMd5 : Attribute
    {
        public string FieldName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PropertyName : Attribute
    {
        public string Name { get; set; }
        private PropertyName()
        {

        }

        public PropertyName(string name)
        {
            this.Name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DisplayName : Attribute
    {
        public string Name { get; set; }
        public DisplayName(string name)
        {
            this.Name = name;
        }

    }

}
