using System.Windows.Forms;

namespace Json.Viewer
{
    public interface IJsonViewerPlugin
    {
        string DisplayName { get; }

        bool CanVisualize(JsonObject jsonObject);
    }

    public interface ICustomTextProvider : IJsonViewerPlugin
    {
        string GetText(JsonObject jsonObject);
    }

    public interface IJsonVisualizer : IJsonViewerPlugin
    {
        Control GetControl(JsonObject jsonObject);

        void Visualize(JsonObject jsonObject);
    }
}