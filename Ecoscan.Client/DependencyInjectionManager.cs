using Ecoscan.Client.ExportsServices;
using Ecoscan.Client.ParsingServices;
using Ninject.Modules;

namespace Ecoscan.Client
{
    public class DependencyInjectionManager : NinjectModule
    {
        public override void Load()
        {
            Bind<IParsingService>().To<CommandLineParsingService>().Named("CommandLine");
            Bind<IParsingService>().To<ConfigurationParsingService>().Named("ConfigurationFile");

            Bind<IExportService>().To<PdfExportService>().Named(".pdf");
            Bind<IExportService>().To<TiffExportService>().Named(".tiff");
            Bind<IExportService>().To<TiffExportService>().Named(".tif");
            Bind<IExportService>().To<SingleImageEachExportService>().Named(".jpeg");
            Bind<IExportService>().To<SingleImageEachExportService>().Named(".jpg");
            Bind<IExportService>().To<SingleImageEachExportService>().Named(".gif");
            Bind<IExportService>().To<SingleImageEachExportService>().Named(".bmp");
            Bind<IExportService>().To<SingleImageEachExportService>().Named(".png");
        }
    }
}