using Microsoft.VisualStudio.DebuggerVisualizers;

using System.Diagnostics;

[assembly: DebuggerVisualizer(typeof(Json.Visualizer.JsonVisualizer),
    Target = typeof(string), Description = "JSON")]

namespace Json.Visualizer
{
    public class JsonVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            using (JsonVisualizerForm form = new JsonVisualizerForm())
            {
                form.Viewer.Json = objectProvider.GetObject().ToString();
                windowService.ShowDialog(form);
            }
        }
    }
}