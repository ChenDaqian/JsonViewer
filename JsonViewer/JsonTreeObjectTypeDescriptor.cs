using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Json.Viewer
{
    internal class JsonTreeObjectTypeDescriptor : ICustomTypeDescriptor
    {
        private readonly JsonObject _jsonObject;
        private PropertyDescriptorCollection _propertyCollection;

        public JsonTreeObjectTypeDescriptor(JsonObject jsonObject)
        {
            _jsonObject = jsonObject;
            InitPropertyCollection();
        }

        private void InitPropertyCollection()
        {
            List<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();

            foreach (JsonObject field in _jsonObject.Fields)
            {
                PropertyDescriptor pd = new JsonTreeObjectPropertyDescriptor(field);
                propertyDescriptors.Add(pd);
            }
            _propertyCollection = new PropertyDescriptorCollection(propertyDescriptors.ToArray());
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(_jsonObject, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(_jsonObject, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(_jsonObject, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(_jsonObject, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(_jsonObject, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(_jsonObject, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(_jsonObject, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return _propertyCollection;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return _jsonObject;
        }
    }

    internal class JsonTreeObjectPropertyDescriptor : PropertyDescriptor
    {
        private readonly JsonObject _jsonObject;
        private JsonTreeObjectTypeDescriptor[] _jsonObjects;

        public JsonTreeObjectPropertyDescriptor(JsonObject jsonObject)
            : base(jsonObject.Id, null)
        {
            _jsonObject = jsonObject;
            if (_jsonObject.JsonType == JsonType.Array)
                InitJsonObject();
        }

        private void InitJsonObject()
        {
            List<JsonTreeObjectTypeDescriptor> jsonObjectList = new List<JsonTreeObjectTypeDescriptor>();
            foreach (JsonObject field in _jsonObject.Fields)
            {
                jsonObjectList.Add(new JsonTreeObjectTypeDescriptor(field));
            }
            _jsonObjects = jsonObjectList.ToArray();
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType => null;

        public override object GetValue(object component)
        {
            switch (_jsonObject.JsonType)
            {
                case JsonType.Array:
                    return _jsonObjects;

                case JsonType.Object:
                    return _jsonObject;

                default:
                    return _jsonObject.Value;
            }
        }

        public override bool IsReadOnly => false;

        public override Type PropertyType
        {
            get
            {
                switch (_jsonObject.JsonType)
                {
                    case JsonType.Array:
                        return typeof(object[]);

                    case JsonType.Object:
                        return typeof(JsonObject);

                    default:
                        return _jsonObject.Value == null ? typeof(string) : _jsonObject.Value.GetType();
                }
            }
        }

        public override void ResetValue(object component)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetValue(object component, object value)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override object GetEditor(Type editorBaseType)
        {
            return base.GetEditor(editorBaseType);
        }
    }
}