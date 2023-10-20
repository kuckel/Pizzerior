using Ninject;
using Pizzerior.Services;
using Pizzerior.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pizzerior
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel? container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureContainer();
            ComposeObjects();
           
        }
        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            container.Bind<ILoggerService>().To<LoggerService>().InTransientScope();
            container.Bind<IPizzeriaService>().To<PizzeriaService>().InTransientScope();

        }

        private void ComposeObjects()
        {

            // Show the main window.
            MainWindow mw = new MainWindow();
            var viewModelLocator = new ViewModelLocator(container);
            mw.DataContext = viewModelLocator.MainViewModel;  //container.Get<MainViewModel>();
            mw.Title = "PizzeriaGuiden";
            mw.Show();

            //viewModelLocator.MainViewModel;

        }

    }
}
