using System;
using System.Windows.Forms;

namespace Json.Viewer
{
    internal class AjaxNetDateTime : ICustomTextProvider
    {
        private static readonly long epoch = new DateTime(1970, 1, 1).Ticks;

        public string GetText(JsonObject jsonObject)
        {
            string text = (string)jsonObject.Value;
            return "Ajax.Net Date:" + ConvertJSTicksToDateTime(Convert.ToInt64(text.Substring(1, text.Length - 2))).ToString();
        }

        private DateTime ConvertJSTicksToDateTime(long ticks)
        {
            return new DateTime(ticks * 10000 + epoch);
        }

        public string DisplayName => "Ajax.Net DateTime";

        public bool CanVisualize(JsonObject jsonObject)
        {
            if (jsonObject.JsonType == JsonType.Value && jsonObject.Value is string)
            {
                string text = (string)jsonObject.Value;
                return text.Length > 2 && text[0] == '@' && text[text.Length - 1] == '@';
            }
            return false;
        }
    }

    internal class CustomDate : ICustomTextProvider
    {
        public string GetText(JsonObject jsonObject)
        {
            int year, month, day, hour, min, second, ms;
            year = (int)(long)jsonObject.Fields["y"].Value;
            month = (int)(long)jsonObject.Fields["M"].Value;
            day = (int)(long)jsonObject.Fields["d"].Value;
            hour = (int)(long)jsonObject.Fields["h"].Value;
            min = (int)(long)jsonObject.Fields["m"].Value;
            second = (int)(long)jsonObject.Fields["s"].Value;
            ms = (int)(long)jsonObject.Fields["ms"].Value;
            return new DateTime(year, month, day, hour, min, second, ms).ToString();
        }

        public string DisplayName => "Date";

        public bool CanVisualize(JsonObject jsonObject)
        {
            return jsonObject.ContainsFields("y", "M", "d", "h", "m", "s", "ms");
        }
    }

    internal class Sample : IJsonVisualizer
    {
        private TextBox tb;

        public Control GetControl(JsonObject jsonObject)
        {
            if (tb == null)
            {
                tb = new TextBox();
                tb.Multiline = true;
            }
            return tb;
        }

        public void Visualize(JsonObject jsonObject)
        {
            tb.Text = string.Format("Array {0} has {1} items", jsonObject.Id, jsonObject.Fields.Count);
        }

        public string DisplayName => "Sample";

        public bool CanVisualize(JsonObject jsonObject)
        {
            return jsonObject.JsonType == JsonType.Array && jsonObject.ContainsFields("[0]");
        }
    }
}