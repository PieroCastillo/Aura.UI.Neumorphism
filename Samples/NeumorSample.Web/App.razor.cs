using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace NeumorSample.Web
{
    public partial class App
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            WebAppBuilder.Configure<NeumorSample.App>()
                .UseReactiveUI()
                .SetupWithSingleViewLifetime();
        }
    }
}